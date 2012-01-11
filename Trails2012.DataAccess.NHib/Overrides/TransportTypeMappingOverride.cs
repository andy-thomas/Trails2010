using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Trails2012.Domain;

namespace Trails2012.DataAccess.NHib.Overrides
{
    public class TransportTypeMappingOverride : IAutoMappingOverride<TransportType>
    {
        public void Override(AutoMapping<TransportType> mapping)
        {
            mapping.Map(x => x.TransportTypeName).Column("TransportType");

        }
    }
}
