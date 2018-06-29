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
        [Key]
        [Column("accountid")]
        public int Id { get; set; }

        [Required]
        [Column("fusenid")]
        public string fusenID { get; set; }

        [Required]
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

        public static JObject CreateFusen(int accountID, string fusenID, out int statusCode)
        {
            //DBに接続してアカウントID,付箋IDを持つ行を追加
            //成功したらそのまま返す/失敗したらJObjectに入れて返す
            //accountとfusenidが一意になるように
            JObject jobj = new JObject();
            using (var db = new TestDbContext())
            {
                //accountがあるかの関数

                //fusenIDが被ってないかチェック
                foreach (var fusen in db.Fusens)
                {
                    if (fusen.fusenID.Equals(fusenID))
                    {
                        jobj.Add("error", new JValue("error"));
                        statusCode = 409;
                        return jobj;
                    }
                }
                jobj.Add("accountID", new JValue(accountID));
                jobj.Add("fusenID", new JValue(fusenID));
                jobj.Add("title", new JValue('\0'));
                jobj.Add("tag", new JValue('\0'));
                jobj.Add("text", new JValue('\0'));
                jobj.Add("color", new JValue('\0'));

                var NewFusen = new Fusen();
                NewFusen.Id = accountID;
                NewFusen.fusenID = fusenID;
                NewFusen.title = "\0";
                NewFusen.tag = "\0".Split() ;
                NewFusen.text = "\0";

                db.Fusens.Add (NewFusen);
                //
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
                var target = db.Fusens.Single(x => x.fusenID == fusenID);
                if (target == null)
                {
                    jobj.Add("error",new JValue("target is not found"));
                    statusCode = 409;
                    return jobj;
                }
                else
                {
                    target.title = fusenData.GetValue("title").ToString();
                    target.tag = fusenData.GetValue("tag").ToString().Split('"',System.StringSplitOptions.RemoveEmptyEntries);
                    target.text = fusenData.GetValue("text").ToString();
                    target.color = fusenData.GetValue("color").ToString();
                    statusCode = 200;

                    db.SaveChanges();
                    return fusenData;
                }
            }
        }

        public static JObject DeleteFusen(int accountID, string fusenID, out int statusCode)
        {
            JObject jobj = new JObject();
            //accountがあるかの関数

            using (var db = new TestDbContext())
            {
                var target = db.Fusens.Single(x => x.fusenID == fusenID);
                if (target == null)
                {
                    jobj.Add("error");
                    statusCode = 409;
                    return jobj;
                }
                else
                {
                    db.Remove(target);
                    db.SaveChanges();
                    statusCode = 200;
                    jobj.Add("success");
                    return jobj;
                }
            }
        }
    }
}