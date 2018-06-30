using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace KyomuServer
{
    class Util
    {
        public static JObject MenberToJobj(Models.User user)
        {
            var json = new JObject();
            json.Add("userID", new JValue(user.id));
            json.Add("userName", new JValue(user.name));
            return json;
        }
    }
}
