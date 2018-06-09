using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace KyomuServer
{
    class ServerTest01
    {
        public JObject getid(JObject json,out int sc)
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
        public const string EMess = @"{
            ""message"" : ""IDが存在しないなどのエラー""
        }";
        public const string Fsample = @"{
            ""userID"" : 888,
            ""fusenID"" : 666,
            ""title"" : ""すごいメモ"",
            ""tag"" : [ ""sugosa"" , ""仰天"" ],
            ""text"" : ""驚天動地奇想天外…～～～～"",
            ""color"" : ""ffffff""
        }";
        public const string Asample = @"{
            ""userID"" : 888,
            ""userName"" : ""ultraman""
        }";
        public const string Asample2 = @"{
            ""userID"" : 999,
            ""userName"" : ""ultranohaha""
        }";
        public static string Lsample = @" [
                {
                    ""userID"" : 888,
                    ""fusenID"" : 666,
                    ""title"" : ""すごいメモ"",
                    ""tag"" : [ ""sugosa"" , ""仰天"" ],
                    ""text"" : ""驚天動地奇想天外…～～～～"",
                    ""color"" : ""ffffff""
                },
                {
                    ""userID"" : 888,
                    ""fusenID"" : 1234,
                    ""title"" : ""次なるメモ"",
                    ""tag"" : [ ""sugosa"" , ""人類"" ],
                    ""text"" : ""都市計画百花繚乱…～～～～"",
                    ""color"" : ""ffffff""
                }
            ]";//配列の形で返しているが配列に名前を付けてネストさせた方が良いか悩む
    }
}
