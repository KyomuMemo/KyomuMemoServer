using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace KyomuServer
{
    class sAccount  //アカウント情報を扱う
    {
        public static string Connection { set { Connection = value; } get { return Connection; } }
        public static JObject AccountCreate(JObject accountInfo)
        {
            return null;
        }

        public static JObject AccountRefer(JObject accountInfo)
        {
            return null;
        }
        
    }
}
