using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Npgsql;

namespace KyomuServer
{
    class sFusen //付箋情報を扱う
    {
        public static string Connection { set { Connection = value; } get { return Connection; } }

        public static JArray GetFusenAllData(int accountID, out int statusCode)
        {
            JArray UserFusen = new JArray();
            using (var conn = new Npgsql.NpgsqlConnection(Connection))
            {
                conn.Open();
                var dataReader = new NpgsqlCommand($@"select * from fusen where accountID = {accountID}", conn).ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        JObject jobj = new JObject();
                        int userID = (int)dataReader["accountID"];
                        jobj.Add("accountID", new JValue(userID));
                        int fusenID = (int)dataReader["fusenID"];
                        jobj.Add("fusenID", new JValue(fusenID));
                        string title = (string)dataReader["title"];
                        jobj.Add("titleID", new JValue(title));
                        string[] tag = (string[])dataReader["tag"];
                        jobj.Add("tag", new JValue(tag));
                        string text = (string)dataReader["text"];
                        jobj.Add("text", new JValue(text));
                        string color = (string)dataReader["color"];
                        jobj.Add("color", new JValue(color));
                        UserFusen.Add(jobj);
                    }

                }
                conn.Close();
            }
            statusCode = 200;
            return UserFusen;
        }

        public static JObject CreateFusen(int accountID, int fusenID, out int statusCode)
        {
            //DBに接続してアカウントID,付箋IDを持つ行を追加
            //成功したらそのまま返す/失敗したらJObjectに入れて返す
            var db = new DbContext();
            statusCode = 0;
            return null;
        }
        public static JObject UpdateFusen(int accountID, int fusenID, JObject fusenData, out int statusCode)
        {
            statusCode = 0;
            return null;
        }
        public static JObject DeleteFusen(int accountID, int fusenID, out int statusCode)
        {
            statusCode = 0;
            return null;
        }

    }
}