using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System;
using KyomuServer.Database;

namespace KyomuServer
{

    class sFusen //付箋情報を扱う
    {
        /*
         引数 accountID:ユーザーID  
         返値 return:付箋の配列JSON  out:HTTPのステータス
         受け取ったユーザーIDからDB内の付箋を検索し、該当する付箋をJSONの配列に変換して返す
             */
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

        /*
         引数 accountID:ユーザID
         返値 return:作成された付箋のJSON out:HTTPのステータス
         クライアント側から付箋作成要求が出されたときに呼ばれる
         空の付箋を生成、そのfusenIDを設定して、DBに保存、JSONにして送信
             */
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
                    if (!sAccount.accountIDExist(accountID))
                    {
                        statusCode = 404;
                        return ServerMain.messagejson("存在しないアカウントIDです");
                    }
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

        /*
         引数 付箋JSON
         返値 return:更新された付箋のJSON out:HTTPのステータス
         クライアントで変更された付箋の情報を受け取り、DB内から該当する付箋を検索し更新
         更新結果は確認のため返される
             */
        public static JObject UpdateFusen(JObject fusenData, out int statusCode)
        {
            try
            {
                using (var db = new KyomuDbContext())
                {
                    JObject jobj = new JObject();
                    try
                    {
                        var target = db.Fusens.Single(x => x.fusenID == fusenData["fusenID"].Value<string>());
                        target.title = fusenData["title"].Value<string>();
                        target.tag = fusenData["tag"].ToObject<string[]>();
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
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                statusCode = 500;
                return ServerMain.messagejson("データベースで異常が発生しました");
            }
        }

        /*
         引数 accountID:ユーザID fusenID:付箋ID
         返値 return:削除された付箋のJSON out:HTTPのステータス
         クライアントで削除された付箋の情報を受け取り、DB内から該当する付箋を検索し該当するデータを削除
         更新結果は確認のため返される
             */
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