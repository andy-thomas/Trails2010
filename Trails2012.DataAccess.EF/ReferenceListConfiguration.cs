using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using Trails2012.Domain;

namespace Trails2012.DataAccess.EF
{
    public class LocationConfiguration : EntityTypeConfiguration<Location>
    {
        public LocationConfiguration()
        {
            ToTable("Location", "dbo");
            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName("LocationId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Do this (does not require FK RegionId property on the Location class
            //HasRequired(p => p.Region).WithMany().Map(m => m.MapKey("RegionID")).WillCascadeOnDelete(false);    

            // Or this - the Telerik grid seems to want a FK property on the class
            HasRequired(p => p.Region).WithMany().HasForeignKey(p=>p.RegionId);
        }
    }

    public class RegionConfiguration : EntityTypeConfiguration<Region>
    {
        public RegionConfiguration()
        {
            ToTable("Region", "dbo");
            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName("RegionId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);      
        }
    }

    public class DifficultyConfiguration : EntityTypeConfiguration<Difficulty>
    {
        public DifficultyConfiguration()
        {
            ToTable("DifficultyType", "dbo");
            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName("DifficultyTypeId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
         }
    }

}
