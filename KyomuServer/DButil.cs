using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace KyomuServer
{
    class Util
    {
        public static JObject UserToJobj(Models.User user)
        {
            var json = new JObject();
            json.Add("userID", new JValue(user.id));
            json.Add("userName", new JValue(user.name));
            return json;
        }
        public static JObject FusenToJobj(Models.Fusen fusen)
        {
            var json = new JObject();
            json.Add("userID", new JValue(fusen.userID));
            json.Add("fusenID", new JValue(fusen.fusenID));
            json.Add("title", new JValue(fusen.title));
            json.Add("tag", new JArray(fusen.tag));
            json.Add("text", new JValue(fusen.text));
            json.Add("color", new JValue(fusen.color));
            return json;
        }
    }
}
