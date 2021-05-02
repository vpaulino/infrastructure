using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using VPFrameworks.Persistence.Abstractions;

namespace InfrastrutureClients.Persistence.EntityFramework
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationContext : DbContext
    {
        private DatabaseSettings settings;

        /// <summary>
        /// 
        /// </summary>
        public ApplicationContext(DatabaseSettings settings) 
        {
            this.settings = settings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.settings.ConnectionString, (_optionsBuilder) => 
            {
                ConfigureSqlServerOptions(_optionsBuilder);
            
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected virtual void ConfigureSqlServerOptions(SqlServerDbContextOptionsBuilder optionsBuilder)
        {   
            optionsBuilder.EnableRetryOnFailure();
            optionsBuilder.CommandTimeout(settings.CommandsTimeout);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             
            base.OnModelCreating(modelBuilder);
        }
    }
}
