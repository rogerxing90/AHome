using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.EntityClient;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
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
        public DbSet<SystemLog> SystemLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Web_UserGroup>()
                .HasMany(c => c.Web_Sys_Functions).WithMany(i => i.GROUPS)
                .Map(t => t.MapLeftKey("Group_ID")
                .MapRightKey("ID"));

        }

        #region ==//因为用的Code First,没有edmx配置文件，即无法组装EntityConnection的Metadata，所以只能用SqlCommand==
        //private EntityConnection BuildConnection(string connectionStringName)
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        //    EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
        //    //Metadata属性的值，是从向导生成的Config粘贴过来的
        //    entityBuilder.Metadata = "res://*/NW.csdl|res://*/NW.ssdl|res://*/NW.msl";
        //    entityBuilder.ProviderConnectionString = connectionString;
        //    entityBuilder.Provider = "System.Data.SqlClient";
        //    return new EntityConnection(entityBuilder.ToString());
        //}

        /// <summary>
        /// 执行语句,获取第一行第一列的数据
        /// <param name="cmdText">sql语句</param>
        /// <param name="parameters">可变参数</param>
        /// <returns>执行结果</returns>
        //public object ExecuteScalar(string sql, params object[] parameters)
        //{
        //    string conStr = "RogerContext";

        //    using (EntityConnection conn = BuildConnection(conStr))
        //    {
        //        conn.Open();
        //        using (EntityCommand cmd = new EntityCommand(sql))
        //        {
        //            cmd.Parameters.AddRange(parameters);
        //            return cmd.ExecuteScalar();
        //        }
        //    }
        //}

        #endregion

        /// <summary>
        /// 执行语句,获取第一行第一列的数据
        /// <param name="cmdText">sql语句</param>
        /// <param name="parameters">可变参数</param>
        /// <returns>执行结果</returns>
        public object ExecuteScalar(string sql, params object[] parameters)
        {
            string conStr = this.Database.Connection.ConnectionString;
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql,conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }

    }
}