using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace KyomuServer
{
    /*
     PostgreSQLを使用しないモック用DB
         */
    namespace Mock
    {
        class User
        {
            public string Name { get; set; }
            public string ID { get; set; }

            public User(string n, string i)
            {
                Name = n; ID = i;
            }

            public JObject ToJObject()
            {
                var json = new JObject();
                json.Add("userID", new JValue(ID));
                json.Add("userName", new JValue(Name));
                return json;
            }
        }
        class Data
        {
            public string userID { get; set; }
            public string fusenID { get; set; }
            public string title { get; set; } = "";
            public string[] tag { get; set; } = { "" };
            public string text { get; set; } = "";
            public string color { get; set; } = "";

            public Data(string ui, string fi)
            {
                userID = ui; fusenID = fi;
            }

            public JObject ToJObject()
            {
                var json = new JObject();
                json.Add("userID", new JValue(userID));
                json.Add("fusenID", new JValue(fusenID));
                json.Add("title", new JValue(title));
                json.Add("tag", new JArray(tag));
                json.Add("text", new JValue(text));
                json.Add("color", new JValue(color));
                return json;
            }
        }
    }
}
