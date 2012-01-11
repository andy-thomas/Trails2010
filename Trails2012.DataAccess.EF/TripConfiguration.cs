using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using Trails2012.Domain;

namespace Trails2012.DataAccess.EF
{
    class TripConfiguration : EntityTypeConfiguration<Trip>
    {
        public TripConfiguration()
        {
            ToTable("Trip", "dbo");
            HasKey(t => t.Id);
            Property(t => t.Id).HasColumnName("TripId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasMany(t => t.Persons)
                .WithMany()
                .Map(tp =>
                         {
                             tp.MapLeftKey("TripId");
                             tp.MapRightKey("PersonId");
                             tp.ToTable("PersonTrip");
                         }

                );
        }
    }
}
