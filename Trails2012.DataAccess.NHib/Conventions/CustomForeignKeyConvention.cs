using System;
using FluentNHibernate;
using FluentNHibernate.Conventions;

namespace Trails2012.DataAccess.NHib.Conventions
{
    public class CustomForeignKeyConvention : ForeignKeyConvention
    {
        protected override string GetKeyName(Member property, Type type)
        {
            // Alter foreign key convention to make the column look like
            // "ParentId" rather than FNH's default of "Parent_Id"
            if (property == null)
                return type.Name + "Id";

            return property.Name + "Id";
        }
    }
}
