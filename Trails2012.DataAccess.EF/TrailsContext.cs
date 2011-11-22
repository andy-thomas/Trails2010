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
        //public DbSet<Trail> Trails { get; set; }
        //public DbSet<TrailType> TrailTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // See http://weblogs.asp.net/scottgu/archive/2010/07/23/entity-framework-4-code-first-custom-database-schema-mapping.aspx

            // remove this: base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new PersonConfiguration());
            modelBuilder.Configurations.Add(new LocationConfiguration());
            modelBuilder.Configurations.Add(new RegionConfiguration());
            modelBuilder.Configurations.Add(new DifficultyConfiguration());
            
        }
    }
}
