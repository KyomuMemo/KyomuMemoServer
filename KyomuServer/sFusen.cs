using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace KyomuServer
{
    class sFusen //付箋情報を扱う
    {
        static JObject GetFusenData(string accountID)
        {
            return null;
        }


        static int CreateFusen(int accountID, out int statusCode)
        {
            statusCode = 0;
            return 0;
        }
        static void UpdateFusen(JObject fusenData, out int statusCode)
        {
            statusCode = 0;
        }
        static void DeleteFusen(int accountID,int fusenID, out int statusCode)
        {
            statusCode = 0;
        }

    }
}
