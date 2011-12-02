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
            HasRequired(p => p.Region).WithMany().HasForeignKey(p=>p.RegionId).WillCascadeOnDelete(false);
        }
    }

    public class RegionConfiguration : EntityTypeConfiguration<Region>
    {
        public RegionConfiguration()
        {
            ToTable("Region", "dbo");
            HasKey(r => r.Id);
            Property(r => r.Id).HasColumnName("RegionId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Ignore(r => r.LocationCount);

            //HasMany(o => o.Locations).WithRequired(location => location.Region).Map(m => m.MapKey("LocationId"));
            HasMany(region => region.Locations).WithRequired(location => location.Region).HasForeignKey(location => location.RegionId).WillCascadeOnDelete(false);

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

    public class TrailTypeConfiguration : EntityTypeConfiguration<TrailType>
    {
        public TrailTypeConfiguration()
        {
            ToTable("TrailType", "dbo");
            HasKey(t => t.Id);
            Property(t => t.Id).HasColumnName("TrailTypeId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(t => t.TrailTypeName).HasColumnName("TrailType");
        }
    }
}
