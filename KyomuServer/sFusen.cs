using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace KyomuServer
{
    class sFusen //付箋情報を扱う
    {
        public static string Connection { set { Connection = value; } get { return Connection; } }

        public static JToken GetFusenAllData(int accountID , out int statusCode)
        {
            statusCode = 400;
            return null;
        }

    /*    public static JObject GetFusenData(int accountID, int fusenID)
        {
            return null;
        }
        */

        public static void CreateFusen(int accountID,int fusenID, out int statusCode)
        {
            statusCode = 0;
        }
        public static void UpdateFusen(int accountID, int fusenID, JObject fusenData, out int statusCode)
        {
            statusCode = 0;
        }
        public static void DeleteFusen(int accountID,int fusenID, out int statusCode)
        {
            statusCode = 0;
        }

    }
}
