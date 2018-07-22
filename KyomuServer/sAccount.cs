using System;
using Newtonsoft.Json.Linq;
using System.Text;
using KyomuServer.Models;
using KyomuServer.Database;
using System.Text.RegularExpressions;

namespace KyomuServer
{
    class sAccount  //アカウント情報を扱う
    {
        public static string[] NgList = { "UNDEFINED" , "undefined" , "Undefined" };
        public static string NameRX  { get => "^[0-9a-zA-Z][0-9a-zA-Z]*$"; }

        /*
         引数 accountID:ユーザID
         返値 アカウントの存在有無
         アカウントが存在するかどうかを返す
             */
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
       
        /*
         引数 accountName:ユーザネーム
         返値 return:ユーザ情報JSON(ユーザID & ユーザネーム) out:HTTPのステータスコード
         新しくアカウントを作成するときに呼び出される
         既に使われている名前や、使用不可に設定している名前の場合は失敗として返す
         そうでない場合はユーザIDを生成し名前と一緒にして返す
             */
        public static JObject AccountCreate(string accountName, out int statusCode)
        {
            try
            {
                using (var db = new KyomuDbContext())
                {
                    if (Array.Exists(NgList,str=>str==accountName) || !Regex.IsMatch(accountName,NameRX) )
                    {
                        statusCode = 403;
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

        /*
         引数 accountName:ユーザネーム
         返値 return:ユーザ情報JSON(ユーザID & ユーザネーム) out:HTTPのステータスコード
         ログインをする際に呼び出される
         該当する名前が見つからない場合は失敗として返す
         見つかった場合はユーザネームとそれに紐づいているユーザIDを一緒にして返す
             */
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
