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
            public static JObject AccountRefer(string accountName, out int sc)
            {
                if (accountName == "ultraman")
                {
                    sc = 200; return JObject.Parse(Asample);
                }
                else
                {
                    sc = 400; return JObject.Parse(EMess);
                }
            }

            public static JObject AccountCreate(string accountName, out int statusCode)
            {
                if (accountName != "ultraman")
                {
                    statusCode = 200; var json = JObject.Parse(Asample);
                    json["userName"].Replace(accountName);
                    return json;
                }
                else
                {
                    statusCode = 400; return JObject.Parse(EMess);
                }
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
