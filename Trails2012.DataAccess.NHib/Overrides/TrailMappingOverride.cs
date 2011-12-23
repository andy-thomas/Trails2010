using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Trails2012.Domain;

namespace Trails2012.DataAccess.NHib.Overrides
{
    public class TrailMappingOverride : IAutoMappingOverride<Trail>
    {
        public void Override(AutoMapping<Trail> mapping)
        {
            mapping.Map(x => x.Image).CustomType("BinaryBlob").Length(int.MaxValue).Nullable();  // using CustomSqlType("image"); does not work :-(

            // Andy - see comments in LocationMappingOverride
            mapping.Map(x => x.LocationId).ReadOnly();
            mapping.Map(x => x.TrailTypeId).ReadOnly();
            mapping.Map(x => x.DifficultyId).ReadOnly();

        }
    }
}
