using Microsoft.EntityFrameworkCore;
using PulseConnectLib.Definitions.Entities;
using PulseConnectLib.Definitions.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace PulseConnectServerOld.Utilities.DatabaseContexts
{
    public class StandAloneRRTMembersDBContext : DbContext
    {
        public DbSet<User> ListOfStandaloneUsers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "server=127.0.0.1;port=9000;user=root;password=pass;Database=testing_standalone_database;";
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
