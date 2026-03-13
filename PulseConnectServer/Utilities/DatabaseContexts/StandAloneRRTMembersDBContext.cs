using Microsoft.EntityFrameworkCore;
using PulseConnectLib.Definitions.Entities;

namespace PulseConnectServer.Utilities.DatabaseContexts
{
    public class StandAloneRRTMembersDBContext(DbContextOptions<StandAloneRRTMembersDBContext> options) : DbContext(options)
    {
        public DbSet<User> ListOfStandaloneUsers { get; set; }
       
    }
}
