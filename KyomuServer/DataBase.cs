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
}

namespace KyomuServer.Database
{
    public class KyomuDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Username=****;Password=****;Database=kyomudb");
        }
    }
}