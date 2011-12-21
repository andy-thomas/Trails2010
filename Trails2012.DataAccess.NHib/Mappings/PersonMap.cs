using Trails2012.Domain;
using FluentNHibernate.Mapping;

namespace Trails2012.DataAccess.NHib.Mappings
{
    public class PersonMap : ClassMap<Person>
    {
        public PersonMap()
        {
            Id(p => p.Id).Column("PersonId");
            Map(p => p.FirstName);
            Map(p => p.LastName);
            Map(p => p.DateOfBirth);
            Map(p => p.Gender);
            //Map(p => p.FullName).ReadOnly();

        }
    }
}
