using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using KyomuServer.Models;

/*
 DBのテーブル構造の定義
     */
namespace KyomuServer.Models
{
    [Table("usertable")]
    public class User
    {
        [Key]
        [Column("userid")]
        public string id { get; set; }

        [Required]
        [Column("username")]
        public string name { get; set; }
    }

    [Table("fusentable")]
    public class Fusen
    {
        [Column("userid")]
        public string userID { get; set; }

        [Key]
        [Column("fusenid")]
        public string fusenID { get; set; }

        [Column("tag")]
        public string[] tag { get; set; }

        [Column("title")]
        public string title { get; set; }

        [Column("honbun")]
        public string text { get; set; }

        [Column("color")]
        public string color { get; set; }

    }
}

namespace KyomuServer.Database
{
    //データベースをC#で扱えるよう宣言するクラス
    public class KyomuDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Fusen> Fusens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Username=****;Password=****;Database=kyomudb");//PostgreSQLのデータベースとの接続情報を書く
        }
    }
}