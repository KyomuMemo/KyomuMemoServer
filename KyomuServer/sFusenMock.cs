using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace KyomuServer
{
    namespace Mock
    {
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
                    {
                        var fusen = new JObject();
                        statusCode = 200;
                        fusen.Add("userID", new JValue(f.userID));
                        fusen.Add("fusenID", new JValue(f.fusenID));
                        fusen.Add("title", new JValue(f.title));
                        fusen.Add("tag", new JArray(f.tag));
                        fusen.Add("text", new JValue(f.text));
                        fusen.Add("color", new JValue(f.color));
                        jar.Add(fusen);
                    }
                }
                //if (statusCode != 200) return JObject.Parse(EMess);
                return jar;
            }
            public JObject CreateFusen(string accountID, string fusenID, out int statusCode)
            {
                statusCode = 400;
                //foreach(var f in fusens) { if (fusenID == f.fusenID && accountID == f.userID) { statusCode = 409; return JObject.Parse(EMess); } }
                fusenID = Guid.NewGuid().ToString("N").Substring(0, 12);
                statusCode = 200; fusens.Add(new Data(accountID, fusenID));
                return fusenjson(accountID,fusenID);
            }

            public JObject UpdateFusen(string accountID, string fusenID, JObject fusenData, out int statusCode)
            {
                statusCode = 409;
                foreach(var f in fusens)
                {
                    if (f.fusenID == fusenID && f.userID == accountID)
                    {
                        f.title = fusenData["title"].Value<string>();
                        f.tag = fusenData["tag"].ToObject<string[]>();
                        f.text = fusenData["text"].Value<string>();
                        f.color = fusenData["color"].Value<string>();
                        statusCode = 200;
                    }
                }
                if (statusCode == 200) return fusenData;
                else return JObject.Parse(EMess);
            }

            public JObject DeleteFusen(string accountID, string fusenID, out int statusCode)
            {
                statusCode = 409;
                for(int i=0;i<fusens.Count;i++)
                {
                    if(fusens[i].fusenID==fusenID && fusens[i].userID == accountID)
                    {
                        fusens.RemoveAt(i);
                        statusCode = 200;
                        return fusenjson(accountID,fusenID);
                    }
                }
                return JObject.Parse(EMess);
            }


            public JObject fusenjson(string userID, string fusenID)
            {
                var json = new JObject();
                json.Add("userID", new JValue(userID));
                json.Add("fusenID", new JValue(fusenID));
                return json;
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
