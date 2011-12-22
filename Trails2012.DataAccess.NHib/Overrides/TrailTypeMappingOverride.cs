using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Trails2012.Domain;

namespace Trails2012.DataAccess.NHib.Overrides
{
    public class TrailTypeMappingOverride :IAutoMappingOverride<TrailType>
    {
        public void Override(AutoMapping<TrailType> mapping)
        {
            mapping.Map(x => x.TrailTypeName).Column("TrailType");
        }
    }
}
