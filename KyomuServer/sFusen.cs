using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace KyomuServer
{
    [Table("fusen")]
    public class Fusen
    {
        [Column("accountid")]
        public int Id { get; set; }

        [Key]
        [Column("fusenid")]
        public string fusenID { get; set; }

        [Column("tag")]
        public string[] tag { get; set; }

        [Column("title")]
        public string title { get; set; }

        [Column("text")]
        public string text { get; set; }

        [Column("color")]
        public string color { get; set; }

    }


    public class TestDbContext : DbContext
    {
        public DbSet<Fusen> Fusens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(sFusen.Connection);

        }
    }

    class sFusen //付箋情報を扱う
    {
        //public static String Connection { set { Connection = value; } get { return Connection; } }
        public static String Connection ="Host=localhost;Username=test;Password=8888;Database=test";
        public static JArray GetFusenAllData(int accountID, out int statusCode)
        {
            JArray UserFusen = new JArray();
            using (var db = new TestDbContext())
            {
                foreach (var fusen in db.Fusens)
                {
                    if (fusen.fusenID.Equals(accountID))
                    {
                        JObject jobj = new JObject();
                        jobj.Add("accountID", new JValue(fusen.Id));
                        jobj.Add("fusenID", new JValue(fusen.fusenID));
                        jobj.Add("title", new JValue(fusen.title));
                        jobj.Add("tag", new JValue(fusen.tag));
                        jobj.Add("text", new JValue(fusen.text));
                        jobj.Add("color", new JValue(fusen.color));
                        UserFusen.Add(jobj);
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
            //accountとfusenidが一意になるように
            
            JObject jobj = new JObject();
            using (var db = new TestDbContext())
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
                jobj.Add("accountID", new JValue(accountID));
                jobj.Add("fusenID", new JValue(FusenID));
                jobj.Add("title", new JValue(""));
                jobj.Add("tag", new JValue(""));
                jobj.Add("text", new JValue(""));
                jobj.Add("color", new JValue(""));

                db.Fusens.Add(new Fusen
                {
                    Id = accountID,
                    fusenID = FusenID,
                    title = "",
                    tag = { },
                    text = ""
                });
                db.SaveChanges();

                statusCode = 200;

            }
            return jobj;
        }

        public static JObject UpdateFusen(int accountID, string fusenID, JObject fusenData, out int statusCode)
        {
            using (var db = new TestDbContext())
            {
                JObject jobj = new JObject();
                try
                {
                    var target = db.Fusens.Single(x => x.fusenID == fusenID);
                    target.title = fusenData.GetValue("title").ToString();
                    target.tag = fusenData.GetValue("tag").ToString().Split('"', System.StringSplitOptions.RemoveEmptyEntries);
                    target.text = fusenData.GetValue("text").ToString();
                    target.color = fusenData.GetValue("color").ToString();
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
            //accountがあるかの関数

            using (var db = new TestDbContext())
            {
                try
                {
                    var target = db.Fusens.Single(x => x.fusenID == fusenID);
                    db.Remove(target);
                    db.SaveChanges();
                    statusCode = 200;
                    jobj.Add("message", new JValue("削除に成功しました"));
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