using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Trails2012.Domain;

namespace Trails2012.DataAccess.NHib.Overrides
{
    public class DifficultyMappingOverride :IAutoMappingOverride<Difficulty>
    {
        public void Override(AutoMapping<Difficulty> mapping)
        {
            mapping.Table("DifficultyType");
            mapping.Id(x => x.Id).Column("DifficultyTypeId");
        }
    }
}
