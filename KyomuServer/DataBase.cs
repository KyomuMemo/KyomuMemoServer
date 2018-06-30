using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using KyomuServer.Models;

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

    [Table("fusen")]
    public class Fusen
    {
        [Column("userid")]
        public int userID { get; set; }

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
    public class KyomuDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Fusen> Fusens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Username=****;Password=****;Database=kyomudb");
        }
    }
}