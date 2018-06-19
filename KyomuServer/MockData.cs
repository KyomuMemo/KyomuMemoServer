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
            public int ID { get; set; }

            public User(string n,int i)
            {
                Name = n;ID = i;
            }
        }
        class Data
        {
            public int userID { get; set; }
            public int fusenID { get; set; }
            public string title { get; set; }
            public string[] tag { get; set; }
            public string text { get; set; }
            public string color { get; set; }
            
            public Data(int ui,int fi)
            {
                userID = ui; fusenID = fi;
            }
        }
    }
}
