using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace KyomuServer
{
    namespace Mock
    {
        //PostgreSQLを使わないデバッグ用のモック
        class sFusen
        {
            List<Data> fusens;
            public sFusen()
            {
                fusens = new List<Data>();
            }

            public  JToken GetFusenAllData(string accountID, out int statusCode)
            {
                var jar = new JArray();
                statusCode = 200;
                foreach(Data f in fusens)
                {
                    if (f.userID == accountID)
                        jar.Add(f.ToJObject());
                }
                return jar;
            }
            public JObject CreateFusen(string accountID, out int statusCode)
            {
                statusCode = 200;
                var fusenID = Guid.NewGuid().ToString("N").Substring(0, 12);
                var fusen = new Data(accountID, fusenID);
                fusens.Add(fusen);
                return fusen.ToJObject();
            }

            public JObject UpdateFusen(string accountID, string fusenID, JObject fusenData, out int statusCode)
            {
                foreach(var f in fusens)
                {
                    if (f.fusenID == fusenID && f.userID == accountID)
                    {
                        f.title = fusenData["title"].Value<string>();
                        f.tag = fusenData["tag"].ToObject<string[]>();
                        f.text = fusenData["text"].Value<string>();
                        f.color = fusenData["color"].Value<string>();
                        statusCode = 200;
                        return fusenData;
                    }
                }
                statusCode = 409;
                return ServerMain.messagejson("該当する付箋が見つかりません");
            }

            public JObject DeleteFusen(string accountID, string fusenID, out int statusCode)
            {
                foreach(Data f in fusens)
                {
                    if(f.fusenID==fusenID && f.userID == accountID)
                    {
                        statusCode = 200;
                        fusens.Remove(f);
                        return f.ToJObject();
                    }
                }
                statusCode = 409;
                return ServerMain.messagejson("該当する付箋が見つかりませんでした");
            }
            

            public const string EMess = @"{
            ""message"" : ""error""
            }";
            public const string Fsample = @"{
            ""userID"" : 888,
            ""fusenID"" : 666,
            ""title"" : ""すごいメモ"",
            ""tag"" : [ ""sugosa"" , ""仰天"" ],
            ""text"" : ""驚天動地奇想天外…～～～～"",
            ""color"" : ""ffffff""
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
}
