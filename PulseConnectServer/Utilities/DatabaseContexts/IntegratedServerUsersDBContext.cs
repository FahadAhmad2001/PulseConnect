using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PulseConnectLib.Definitions.Entities;

namespace PulseConnectServer.Utilities.DatabaseContexts
{
    public class IntegratedServerUsersDBContext(DbContextOptions<IntegratedServerUsersDBContext> options) : DbContext(options)
    {
        public DbSet<User> AllServerUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Users");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IntegratedServerUsersDBContext).Assembly);
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("TypeOfUser")
                .HasValue<DoctorUser>("DocUser")
                .HasValue<NurseUser>("NurseUser");
            base.OnModelCreating(modelBuilder);
        }
       
    }
}
