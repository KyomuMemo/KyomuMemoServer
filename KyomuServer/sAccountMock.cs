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
                sc = 409;
                foreach(var ac in users)
                {
                    if (accountName == ac.Name)
                    {
                        sc = 200; return userjson(ac.ID, ac.Name);
                    }
                }
                return JObject.Parse(EMess);
            }

            public JObject AccountCreate(string accountName, out int statusCode)
            {
                statusCode = 200;
                foreach(var ac in users)
                {
                    if(accountName == ac.Name)
                    {
                        statusCode = 409;
                        return JObject.Parse(EMess);
                    }
                }
                string id = Guid.NewGuid().ToString("N").Substring(0, 12);
                users.Add(new User(accountName, id));
                return userjson(id, accountName);
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
