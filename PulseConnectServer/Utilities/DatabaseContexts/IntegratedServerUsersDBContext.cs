using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PulseConnectLib.Definitions.Entities;

namespace PulseConnectServer.Utilities.DatabaseContexts
{
    public class IntegratedServerUsersDBContext : DbContext
    {
        public DbSet<User> AllServerUsers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "server=127.0.0.1;port=9000;user=root;password=harnessedtech88;Database=testing_combined_database;";
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
