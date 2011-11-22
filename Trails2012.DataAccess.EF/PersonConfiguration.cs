﻿using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using Trails2012.Domain;

namespace Trails2012.DataAccess.EF
{
    class PersonConfiguration : EntityTypeConfiguration<Person>
    {
        public PersonConfiguration()
        {
            ToTable("Person");
            HasKey(p => p.Id);

            Property(p => p.Id).HasColumnName("PersonId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.FullName).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }
    }
}
