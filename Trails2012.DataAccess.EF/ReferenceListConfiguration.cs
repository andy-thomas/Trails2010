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
            HasRequired(p => p.Region).WithMany().Map(m => m.MapKey("RegionID")).WillCascadeOnDelete(false);    
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
