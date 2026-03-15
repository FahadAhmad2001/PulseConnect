using System;
using System.Collections.Generic;
using System.Text;
using Pomelo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PulseConnectLib.Definitions.Events;

namespace PulseConnectServer.Utilities.DatabaseContexts
{
    public class StandAloneRRTEventsDBContext(DbContextOptions<StandAloneRRTEventsDBContext> options) : DbContext(options)
    {
        //public DbSet<MedicalEvent> MedicalEvents { get; set; }
        public DbSet<RapidResponseEvent> ListOfStandaloneEvents { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RapidResponseEvent>()
                .ComplexProperty(u => u.PatientLocation, l =>
                {
                    l.Property(l => l.WardName).HasColumnName("PatientLocationWardName");
                    l.Property(l => l.BedNumber).HasColumnName("PatientLocationBedNumber");
                });
        }
    }
}
