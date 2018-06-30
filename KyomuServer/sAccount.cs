using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using KyomuServer.Models;
using KyomuServer.Database;
using System.Data;
using System.Linq;


namespace KyomuServer
{
    class sAccount  //アカウント情報を扱う
    {
        public static bool accountIDExist(int accountID)
        {
            using (var db = new KyomuDbContext())
            {
                try
                {
                    foreach (var members in db.Members)
                    {
                        if (members.id.Equals(accountID))
                            return true;
                    }
                    return false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
        }
       
        public static JObject AccountCreate(string accountName, out int statusCode)
        {
            try
            {
                using (var db = new KyomuDbContext())
                {
                    foreach (var members in db.Members)
                    {
                        //入力されたNameが既にデータベースにある場合
                        if (accountName.Equals(members.name))
                        {
                            statusCode = 409;
                            return ServerMain.messagejson("このアカウント名は既に使われています");
                        }
                    }
                    var newMember = new Member { name = accountName };
                    //入力されたNameが既にデータベースにない場合
                    byte[] data = Encoding.UTF8.GetBytes(accountName);
                    var bs = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(data);
                    StringBuilder result = new StringBuilder();
                    foreach (byte b in bs)
                    {
                        result.Append(b.ToString("x2"));
                    }
                    newMember.id = result.ToString();
                    //データベースに登録
                    db.Members.Add(newMember);
                    db.SaveChanges();
                    JObject newObj = new JObject();
                    newObj.Add("userID", new JValue(newMember.id));
                    newObj.Add("userName", new JValue(newMember.name));
                    statusCode = 200;
                    return newObj;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                statusCode = 500;
                return ServerMain.messagejson("データベースで異常が発生しました");
            }
        }

        public static JObject AccountRefer(string accountName, out int statusCode)
        {
            try
            {
                using (var db = new KyomuDbContext())
                {
                    foreach (var members in db.Members)
                    {
                        if (accountName.Equals(members.name))
                        {
                            JObject newObj = new JObject();
                            newObj.Add("userID", new JValue(members.id));
                            newObj.Add("userName", new JValue(members.name));
                            statusCode = 200;
                            return newObj;
                        }
                    }
                    //ログインしたいaccountNameがデータベース上に存在しない場合
                    statusCode = 409;
                    return ServerMain.messagejson("アカウントが見つかりませんでした");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                statusCode = 500;
                return ServerMain.messagejson("データベースで異常が発生しました");
            }
        }
    }

}


/*
namespace ConsoleApplication
{
    public class Program
    {

        public static void Main(string[] args)
        {
        
            int aa = 0;
            KyomuServer.sAccount.AccountCreate("hashas", out aa);
            Console.WriteLine(aa);
            KyomuServer.sAccount.AccountRefer("ytytyty", out aa);
            Console.WriteLine(aa);
            Console.ReadLine();
        }
    }
}
*/