using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Trails2012.DataAccess.NHib.Conventions
{
    class CustomPrimaryKeyConvention : IIdConvention
    {
        // Map the base "Id" field to the column primary kay name, 
        // e.g. Person class property "Id" maps to database column "PersonId"
        public void Apply(IIdentityInstance instance)
        {
            Type entityType = instance.EntityType;
            instance.Column(entityType.Name + "Id");
        }
    }
}
