using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace KyomuServerMock
{
    class sFusem
    {
        public static JToken GetFusenAllData(int accountID)
        {
            if (accountID == 888) return JArray.Parse(Lsample);
            else return JObject.Parse(EMess);
        }
        public static void CreateFusen(int accountID, int fusenID, out int statusCode)
        {
            if (accountID == 888 && fusenID != 666) statusCode = 200;
            else statusCode = 440;
        }
        public static void UpdateFusen(int accountID, int fusenID, JObject fusenData, out int statusCode)
        {
            statusCode = 0;
        }
        public static void DeleteFusen(int accountID, int fusenID, out int statusCode)
        {
            statusCode = 0;
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
