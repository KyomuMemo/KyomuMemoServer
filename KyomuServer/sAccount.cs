using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;









using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


using ConsoleApplication.Models;
using Microsoft.EntityFrameworkCore;


using System.Data;

using System.Linq;
using ConsoleApplication.Database;
using Npgsql;


namespace ConsoleApplication.Models
{
    [Table("obj")]
    public class Member
    {
        [Key]
        [Column("username")]
        public string name { get; set; }

        [Required]
        [Column("userid")]
        public int id { get; set; }

        public virtual ICollection<Todo> Todos { get; set; }
    }

    [Table("todos")]
    public class Todo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("member_id")]
        public string MemberId { get; set; }

        [Required]
        [Column("content")]
        public string Content { get; set; }

        [Column("due")]
        public DateTime Due { get; set; }

        [Column("done")]
        public bool Done { get; set; }

        public virtual Member Member { get; set; }
    }
}







namespace ConsoleApplication.Database
{
    public class TestDbContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Todo> Todos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Username=postgres;Password=Basketball10;Database=test");
        }
    }
}






namespace KyomuServer
{
    class sAccount  //アカウント情報を扱う
    {
        //public static string Connection { set { Connection = value; } get { return Connection; } }
        public static JObject AccountCreate(string accountName, out int statusCode)
        {
            using (var db = new TestDbContext())
            {
                foreach (var members in db.Members)
                {
                    //入力されたNameが既にデータベースにある場合
                    if (accountName.Equals(members.name))
                    {
                        statusCode = 409;
                        JObject error = new JObject();
                        error.Add("message", new JValue("account is existed"));
                        return error;
                        // return JObject.Parse(@"{""message"":""error""}");
                    }
                }
                var newMember = new Member { name = accountName };
                //入力されたNameが既にデータベースにない場合
                Random random = new Random();
                bool loopChecker = true;
                while (loopChecker)
                {
                    newMember.id = random.Next();
                    foreach (var members in db.Members)
                    {
                        if (newMember.id.Equals(members.id))
                        {
                            loopChecker = true;
                            break;
                        }
                        else
                            loopChecker = false;
                    }
                }
                //データベースに登録
                db.Members.Add(newMember);
                db.SaveChanges();
                JObject newObj = new JObject();
                newObj.Add("userID", new JValue(newMember.id));
                newObj.Add("userName", new JValue(newMember.name));
                statusCode = 200;
                return newObj;
            }
        }

        public static JObject AccountRefer(string accountName, out int statusCode)
        {
            using (var db = new TestDbContext())
            {
                foreach (var members in db.Members)
                {
                    if (accountName.Equals(members.name))
                    {
                        JObject newObj = new JObject();
                        newObj.Add("userID", new JValue(members.id));
                        newObj.Add("userName", new JValue(members.name));
                        statusCode = 200;
                        return newObj;
                    }
                }
                //ログインしたいaccountNameがデータベース上に存在しない場合
                statusCode = 409;
                JObject error = new JObject();
                error.Add("message", new JValue("account is not found"));
                return error;
            }
        }
    }
}



namespace ConsoleApplication
{
    public class Program
    {

        public static void Main(string[] args)
        {

            int aa = 0;
            KyomuServer.sAccount.AccountCreate("yyyy", out aa);
            Console.WriteLine(aa);
            KyomuServer.sAccount.AccountRefer("ytytyty", out aa);
            Console.WriteLine(aa);
            Console.ReadLine();
        }
    }
}
