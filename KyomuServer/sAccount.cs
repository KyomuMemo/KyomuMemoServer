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
        public static string[] NgList = { "" , "undefined" , "Undefind" };

        //アカウントが存在するかどうかを返す
        public static bool accountIDExist(string accountID)
        {
            using (var db = new KyomuDbContext())
            {
                try
                {
                    foreach (var members in db.Users)
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
                    if (Array.Exists(NgList,str=>str==accountName))
                    {
                        statusCode = 404;
                        return ServerMain.messagejson("このアカウント名は使用不能です");
                    }
                    foreach (var members in db.Users)
                    {
                        //入力されたNameが既にデータベースにある場合
                        if (accountName.Equals(members.name))
                        {
                            statusCode = 409;
                            return ServerMain.messagejson("このアカウント名は既に使われています");
                        }
                    }
                    //入力されたNameがデータベースにない場合
                    byte[] data = Encoding.UTF8.GetBytes(accountName);
                    var bs = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(data);
                    StringBuilder result = new StringBuilder();
                    foreach (byte b in bs)
                    {
                        result.Append(b.ToString("x2"));
                    }
                    var newMember = new User { name = accountName,id=result.ToString() };
                    //データベースに登録
                    db.Users.Add(newMember);
                    db.SaveChanges();
                    statusCode = 200;
                    return Util.UserToJobj(newMember);
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
                    foreach (var members in db.Users)
                    {
                        if (accountName.Equals(members.name))
                        {
                            statusCode = 200;
                            return Util.UserToJobj(members);
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