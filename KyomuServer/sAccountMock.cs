using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace KyomuServer
{
    namespace Mock
    {
        class sAccount
        {
            List<User> users;
            Random random;
            public sAccount()
            {
                users = new List<User>();
                random = new Random();
            }

            public JObject AccountRefer(string accountName, out int sc)
            {
                foreach (var ac in users)
                {
                    if (accountName == ac.Name)
                    {
                        sc = 201; return ac.ToJObject();
                    }
                }
                sc = 409;
                return ServerMain.messagejson("アカウントが見つかりませんでした");
            }

            public JObject AccountCreate(string accountName, out int statusCode)
            {
                statusCode = 200;
                foreach(var ac in users)
                {
                    if(accountName == ac.Name)
                    {
                        statusCode = 409;
                        return ServerMain.messagejson("このアカウント名は既に使われています");
                    }
                }
                string id = Guid.NewGuid().ToString("N").Substring(0, 12);
                var user = new User(accountName, id);
                users.Add(user);
                return user.ToJObject();
            }


            public JObject userjson(string userID,string userName)
            {
                var json = new JObject();
                json.Add("userID", new JValue(userID));
                json.Add("useName", new JValue(userName));
                return json;
            }


            public const string Asample = @"{
            ""userID"" : 888,
            ""userName"" : ""ultraman""
            }";
            public const string Asample2 = @"{
            ""userID"" : 999,
            ""userName"" : ""ultranohaha""
            }";
            public const string EMess = @"{
            ""message"" : ""error""
            }";
        }
    }
}
