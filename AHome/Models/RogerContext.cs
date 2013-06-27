using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AHome.Models
{
    public class RogerContext : DbContext
    {
        public DbSet<News> News { get; set; }
        public DbSet<NewsType> NewsTypes { get; set; }
        public DbSet<CraftType> CraftTypes { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Province> Provinces { get; set; }
    }
}