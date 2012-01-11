using Trails2012.Domain;
using System.Data.Entity;

namespace Trails2012.DataAccess.EF
{
    public class TrailsContext : DbContext
    {
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<TransportType> TransportTypes { get; set; }
        public DbSet<TrailType> TrailTypes { get; set; }
        public DbSet<Trail> Trails { get; set; }
        public DbSet<Trip> Trips { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // See http://weblogs.asp.net/scottgu/archive/2010/07/23/entity-framework-4-code-first-custom-database-schema-mapping.aspx

            // What to do here? - this enables on lazy loading, but causes problems with JSON serialization on the Telerik grid control
            //Configuration.ProxyCreationEnabled = false;

            //Database.SetInitializer<TrailsContext>(null); // Use this to by-pass checks for changes to the domain when the database contains a EdmMetadata table

            // remove this: base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new PersonConfiguration());
            modelBuilder.Configurations.Add(new LocationConfiguration());
            modelBuilder.Configurations.Add(new RegionConfiguration());
            modelBuilder.Configurations.Add(new DifficultyConfiguration());
            modelBuilder.Configurations.Add(new TransportTypeConfiguration());
            modelBuilder.Configurations.Add(new TrailTypeConfiguration());
            modelBuilder.Configurations.Add(new TrailConfiguration());
            modelBuilder.Configurations.Add(new TripConfiguration());
            
        }
    }
}
