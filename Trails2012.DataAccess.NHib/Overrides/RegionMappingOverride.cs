using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Trails2012.Domain;

namespace Trails2012.DataAccess.NHib.Overrides
{
    public class RegionMappingOverride : IAutoMappingOverride<Region>
    {
        public void Override(AutoMapping<Region> mapping)
        {
            mapping.IgnoreProperty(x => x.LocationCount);
        }
    }
}
