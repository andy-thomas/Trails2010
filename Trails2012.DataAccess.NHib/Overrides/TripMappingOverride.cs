using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Trails2012.Domain;

namespace Trails2012.DataAccess.NHib.Overrides
{
    public class TripMappingOverride : IAutoMappingOverride<Trip>
    {
        public void Override(AutoMapping<Trip> mapping)
        {
            // Andy - see comments in LocationMappingOverride
            mapping.Map(x => x.TransportTypeId).ReadOnly();
            mapping.Map(x => x.TrailId).ReadOnly();
            mapping.IgnoreProperty(x => x.PersonsSummary);

            mapping.HasManyToMany<Person>(x => x.Persons)
                .Table("PersonTrip")
                .ParentKeyColumn("TripId")
                .ChildKeyColumn("PersonId");

        }
    }
}
