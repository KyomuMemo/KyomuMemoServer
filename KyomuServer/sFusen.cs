using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System;
using KyomuServer.Database;

namespace KyomuServer
{

    class sFusen //付箋情報を扱う
    {
        public static JToken GetFusenAllData(string accountID, out int statusCode)
        {
            try
            {
                JArray UserFusen = new JArray();
                using (var db = new KyomuDbContext())
                {
                    foreach (var fusen in db.Fusens)
                    {
                        if (fusen.userID.Equals(accountID))
                        {
                            UserFusen.Add(Util.FusenToJobj(fusen));
                        }
                    }
                }
                statusCode = 200;
                return UserFusen;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                statusCode = 500;
                return ServerMain.messagejson("データベースで異常が発生しました");
            }
        }

        public static JObject CreateFusen(string accountID, out int statusCode)
        {
            //DBに接続してアカウントID,付箋IDを持つ行を追加
            //成功したらそのまま返す/失敗したらJObjectに入れて返す
            //fusenidが一意になるように
            try
            {
                JObject jobj = new JObject();
                using (var db = new KyomuDbContext())
                {
                    //accountがあるかの関数
                    //fusenidの発行をする)
                    string FusenID;
                    bool same = false;
                    do
                    {
                        same = false;
                        FusenID = Guid.NewGuid().ToString("N").Substring(0, 20);
                        foreach (var fusen in db.Fusens)
                            if (fusen.fusenID.Equals(FusenID))
                                same = true;
                    } while (same);

                    var newfusen = new Models.Fusen
                    {
                        userID = accountID,
                        fusenID = FusenID,
                        title = "",
                        tag = new string[] { "" },
                        text = "",
                        color = ""
                    };
                    jobj = Util.FusenToJobj(newfusen);
                    db.Fusens.Add(newfusen);
                    db.SaveChanges();
                    statusCode = 200;
                }
                return jobj;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                statusCode = 500;
                return ServerMain.messagejson("データベースで異常が発生しました");
            }
        }

        public static JObject UpdateFusen(string accountID, string fusenID, JObject fusenData, out int statusCode)
        {
            if (fusenData["userID"].Value<string>() != accountID || fusenData["fusenID"].Value<string>() != fusenID)
            {
                statusCode = 403;
                return ServerMain.messagejson("APIと付箋JSONに不整合が起きています");
            }
            try
            {
                using (var db = new KyomuDbContext())
                {
                    JObject jobj = new JObject();
                    try
                    {
                        var target = db.Fusens.Single(x => x.fusenID == fusenID); Console.WriteLine("target");
                        target.title = fusenData["title"].Value<string>(); Console.WriteLine("title");
                        target.tag = fusenData["tag"].ToObject<string[]>(); Console.WriteLine("tag");
                        target.text = fusenData["text"].Value<string>(); Console.WriteLine("text");
                        target.color = fusenData["color"].Value<string>(); Console.WriteLine("color");
                        statusCode = 200;
                        db.SaveChanges();
                        return fusenData;
                    }
                    catch (Exception)
                    {
                        jobj.Add("message", new JValue("指定の付箋が見つかりません"));
                        statusCode = 409;
                        return jobj;
                    }
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                statusCode = 500;
                return ServerMain.messagejson("データベースで異常が発生しました");
            }
        }

        public static JObject DeleteFusen(string accountID, string fusenID, out int statusCode)
        {
            try
            {
                JObject jobj = new JObject();

                using (var db = new KyomuDbContext())
                {
                    try
                    {
                        var target = db.Fusens.Single(x => x.fusenID == fusenID);
                        jobj = Util.FusenToJobj(target);
                        db.Remove(target);
                        db.SaveChanges();
                        statusCode = 200;
                        return jobj;
                    }
                    catch (Exception)
                    {
                        jobj.Add("message", new JValue("指定された付箋が存在しません"));
                        statusCode = 409;
                        return jobj;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                statusCode = 500;
                return ServerMain.messagejson("データベースで異常が発生しました");
            }
        }
    }
}