using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using FoursquareClientAssignment.Entities;

namespace FoursquareClientAssignment.Database
{
    public class FoursquareClientAssignmentContext : DbContext
    {
        public FoursquareClientAssignmentContext() : base("DefaultConnection")
        {
        }

        public DbSet<QueryResult> QueryResults { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}