using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using KyomuServer.Database;

namespace KyomuServer
{

    class sFusen //付箋情報を扱う
    {
        public static JArray GetFusenAllData(int accountID, out int statusCode)
        {
            JArray UserFusen = new JArray();
            using (var db = new KyomuDbContext())
            {
                foreach (var fusen in db.Fusens)
                {
                    if (fusen.fusenID.Equals(accountID))
                    {
                        UserFusen.Add(Util.FusenToJobj(fusen));
                    }
                }
            }
            statusCode = 200;
            return UserFusen;
        }

        public static JObject CreateFusen(int accountID, out int statusCode)
        {
            //DBに接続してアカウントID,付箋IDを持つ行を追加
            //成功したらそのまま返す/失敗したらJObjectに入れて返す
            //fusenidが一意になるように
            
            JObject jobj = new JObject();
            using (var db = new KyomuDbContext())
            {
                //accountがあるかの関数
                //fusenidの発行をする)
                string FusenID;
                bool same = true;
                do
                {
                    FusenID = Guid.NewGuid().ToString("N").Substring(0, 20);
                    foreach (var fusen in db.Fusens)
                        if (!fusen.fusenID.Equals(FusenID))
                            same = false;
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
        }

        public static JObject UpdateFusen(int accountID, string fusenID, JObject fusenData, out int statusCode)
        {
            using (var db = new KyomuDbContext())
            {
                JObject jobj = new JObject();
                try
                {
                    var target = db.Fusens.Single(x => x.fusenID == fusenID);
                    target.title = fusenData["title"].Value<string>();
                    target.tag = fusenData["tag"].Value<string[]>();
                    target.text = fusenData["text"].Value<string>();
                    target.color = fusenData["color"].Value<string>();
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
        }

        public static JObject DeleteFusen(int accountID, string fusenID, out int statusCode)
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
    }
}