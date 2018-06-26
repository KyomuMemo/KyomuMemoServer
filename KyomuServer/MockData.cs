using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace KyomuServer
{
    namespace Mock
    {
        class User
        {
            public string Name { get; set; }
            public string ID { get; set; }

            public User(string n,string i)
            {
                Name = n;ID = i;
            }
        }
        class Data
        {
            public string userID { get; set; }
            public string fusenID { get; set; }
            public string title { get; set; }
            public string[] tag { get; set; }
            public string text { get; set; }
            public string color { get; set; }
            
            public Data(string ui,string fi)
            {
                userID = ui; fusenID = fi;
            }
        }
    }
}
