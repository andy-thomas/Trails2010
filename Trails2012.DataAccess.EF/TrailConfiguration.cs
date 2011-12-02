using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using Trails2012.Domain;

namespace Trails2012.DataAccess.EF
{
    class TrailConfiguration : EntityTypeConfiguration<Trail>
    {
        public TrailConfiguration()
        {
            ToTable("Trail", "dbo");
            HasKey(t => t.Id);
            Property(t => t.Id).HasColumnName("TrailId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //Ignore(t => t.Notes);
            //HasRequired(t => t.Location).WithRequiredDependent().Map(m => m.MapKey("LocationId"));
            //HasRequired(t => t.Location).WithMany().HasForeignKey(t => t.LocationId); 

            //HasRequired(t => t.TrailType).WithMany().HasForeignKey(t  => t.TrailTypeId).WillCascadeOnDelete(false);
            //HasRequired(t => t.Difficulty).WithMany().HasForeignKey(t  => t.DifficultyId).WillCascadeOnDelete(false);
            //Unable to determine the principal end of the 'Trails2012.DataAccess.EF.Trail_Location' relationship. Multiple added entities may have the same primary key.
        }
    }
}
