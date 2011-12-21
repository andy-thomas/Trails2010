using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using Trails2012.Domain;

namespace Trails2012.DataAccess.NHib
{
    public class TrailsConfiguration : DefaultAutomappingConfiguration
    {
        private readonly List<Type> mappedTypes;

        public TrailsConfiguration()
        {
            // List all those types which have mapping classes
            // Use this to exclude classes from Automapping, which have their own explicit mappings
            mappedTypes = new List<Type>
                              {
                                  typeof (Person)
                              };
            //mappedTypes.Add(typeof(SomeOtherEntity));

        }

        public override bool ShouldMap(Type type)
        {
            bool shouldMap =  base.ShouldMap(type) &&
                    type.IsSubclassOf(typeof (EntityBase)) &&
                   !mappedTypes.Contains(type);
            return shouldMap;
        }

        //public override bool IsId(FluentNHibernate.Member member)
        //{
        //    return member.Name == member.DeclaringType.Name + "Id";
        //}

    }
}



