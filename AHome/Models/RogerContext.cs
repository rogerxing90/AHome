using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AHome.Models
{
    public class RogerContext : DbContext
    {
        public RogerContext()
        {
            this.Configuration.LazyLoadingEnabled = true;
        }
        public DbSet<News> News { get; set; }
        public DbSet<NewsType> NewsTypes { get; set; }
        public DbSet<CraftType> CraftTypes { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Master> Masters { get; set; }
        public DbSet<Master_cert> MasterCerts { get; set; }
        public DbSet<Master_Type> MasterTypes { get; set; }
        public DbSet<VMasterType> VMasterTypes { get; set; }
        public DbSet<VProductCraftType> VProductCraftTypes { get; set; }
        public DbSet<Web_User> Web_Users { get; set; }
        public DbSet<Web_UserGroup> Web_UserGroups { get; set; }
        public DbSet<Web_Sys_Function> Web_Sys_Functions { get; set; }



    }
}