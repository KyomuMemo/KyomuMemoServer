using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace KyomuServerMock
{
    class sAccount
    {
        public static JObject AccountRefer(JObject json, out int sc)
        {
            if (json["userName"].Value<string>() == "ultraman")
            {
                sc = 200; return JObject.Parse(Asample);
            }
            else
            {
                sc = 440; return JObject.Parse(EMess);
            }
        }

        public static JObject AccountCreate(JObject accountInfo, out int statusCode)
        {
            if (accountInfo["userName"].Value<string>() != "ultraman")
            {
                statusCode = 440; return JObject.Parse(EMess);
            }
            else
            {
                statusCode = 200; var json = JObject.Parse(Asample);
                json["userName"].Replace(accountInfo["userName"]);
                return json;
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
