using System;
using System.Text;
using System.Collections.Generic;
using FluentNHibernate.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using Trails2012.Domain;

namespace Trails2012.DataAccess.NHib.Test
{
    /// <summary>
    /// see http://wiki.fluentnhibernate.org/Persistence_specification_testing
    /// </summary>
    [TestClass]
    public class DomainEntityPersistenceSpecificationTests
    {
        private static NHibRepository _repository;
        private static ISession _session;
        private readonly DateTime _testDate = new DateTime(1955, 4, 18);

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            _repository = new NHibRepository(true); // These tests insert records, so don't set this parameter value to false :-) Always use SQLite when doing these tests.
            //Util.SeedData(_repository); //no need to seed data at this stage
            _session = _repository.GetCurrentSession();
        }
      
        // Custom mapped entities

        [TestMethod]
        public void CanSaveVanillaPerson()
        {
            new PersistenceSpecification<Person>(_session)
                .CheckProperty(x => x.FirstName, "Alf")
                .CheckProperty(x => x.DateOfBirth, _testDate)
                .CheckProperty(x => x.Gender, "M")
                .CheckProperty(x => x.LastName, "Garnett")
                .VerifyTheMappings();
        }

        // Auto-mapped entities - so I guess we do not really need to test these, unless they have configuration overrides

        [TestMethod]
        public void CanSaveVanillaRegion()
        {
            new PersistenceSpecification<Region>(_session)
                .CheckProperty(x => x.Name, "TestName")
                .CheckProperty(x => x.Description, "TestDesc")
                .VerifyTheMappings();
        }

        [TestMethod]
        public void CanSaveVanillaLocation()
        {
            // Andy - This failed with error to say that a Region was expected but got a proxy instead
            // I got around this by overriding the Equals() method in the Region class
            // so that the equality passes if all the property values match
            // e.g. see http://groups.google.com/group/fluent-nhibernate/browse_thread/thread/cb38ffc9030f6e82/60d400dd3bbc644a

            new PersistenceSpecification<Location>(_session)
                .CheckProperty(x => x.Name, "TestName")
                .CheckProperty(x => x.Description, "TestDesc")
                .CheckProperty(x => x.Directions, null)
                .CheckReference(x => x.Region, new Region{Name = "TestRegion"})
                .VerifyTheMappings();
        }
    }
}
