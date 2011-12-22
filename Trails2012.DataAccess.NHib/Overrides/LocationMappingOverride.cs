using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Trails2012.Domain;

namespace Trails2012.DataAccess.NHib.Overrides
{
    public class LocationMappingOverride : IAutoMappingOverride<Location>
    {
        public void Override(AutoMapping<Location> mapping)
        {
            // Andy - this override sets the RegionId foreign key to read-only
            // This is to avoid the infamous "Invalid index N for this SqlParameterCollection with Count=N" error when saving a Location,
            // which occurs due to NHib trying to map RegionId twice (once through RegionId property, 
            // and once through the relationship to the Region object.
            // (I guess I could lose the RegionId property altogether and use Region.Id, but that would break my implementation 
            // of the Entity Framework repostitory plug-in)
            mapping.Map(x => x.RegionId).ReadOnly();
        }
    }
}
