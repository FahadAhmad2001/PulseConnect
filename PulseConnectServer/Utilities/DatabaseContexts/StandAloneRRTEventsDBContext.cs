using System;
using System.Collections.Generic;
using System.Text;
using Pomelo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PulseConnectLib.Definitions.Events;

namespace PulseConnectServer.Utilities.DatabaseContexts
{
    public class StandAloneRRTEventsDBContext : DbContext
    {
        //public DbSet<MedicalEvent> MedicalEvents { get; set; }
        public DbSet<RapidResponseEvent> ListOfStandaloneEvents { get; set; }
      /*  protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RapidResponseEvent>().ToTable("StandaloneRapidResponseEvents");
           // base.OnModelCreating(modelBuilder);
        } */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "server=127.0.0.1;port=9000;user=root;password=harnessedtech88;Database=testing_standalone_database;";
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
