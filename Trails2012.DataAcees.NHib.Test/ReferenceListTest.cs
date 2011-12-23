using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trails2012.Domain;

namespace Trails2012.DataAccess.NHib.Test
{
    [TestClass]
    public class ReferenceListTest
    {
        private static IRepository _repository;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            _repository = new NHibRepository(true);
            Util.SeedData(_repository);
        }

        [TestMethod]
        public void ShouldGetAllRegions()
        {
            IEnumerable<Region> regions = _repository.List<Region>();
            Assert.IsTrue(regions.Count() > 0);
            Console.WriteLine("There are {0} regions", regions.Count());

        }

        [TestMethod]
        public void ShouldGetAllDifficulties()
        {
            IEnumerable<Difficulty> difficultys = _repository.List<Difficulty>();
            Assert.IsTrue(difficultys.Count() > 0);
            Console.WriteLine("There are {0} difficulty levels", difficultys.Count());
        }

        [TestMethod]
        public void ShouldGetAllLocations()
        {
            IEnumerable<Location> locations = _repository.List<Location>();
            Assert.IsTrue(locations.Count() > 0);
            Console.WriteLine("There are {0} locations", locations.Count());
        }

        [TestMethod]
        public void ShouldGetAllLocationsIncludeRegions()
        {
            IEnumerable<Location> locations = _repository.ListIncluding<Location>(l => l.Region);
            Assert.IsNotNull(locations);
            int count = locations.Count();
            Assert.IsTrue(count > 0);
            Console.WriteLine("There are {0} locations", count);

        }

        [TestMethod]
        public void ShouldGetLocationById()
        {
            const int id = 1;
            //IEnumerable<Location> locations = repository.List<Location>();
            //if (locations != null)
            //{
            //    Location location = locations.FirstOrDefault(loc => loc.Id == id);
            //    Assert.IsNotNull(location);
            //    Assert.AreEqual(location.Id, id);
            //    Assert.IsNotNull(location.Region);

            //    Console.WriteLine("Location found: id = {0}, Name = {1}, Region = {2}",
            //        location.Id, location.Name, location.Region.Name);
            //}

            Location location = _repository.GetById<Location>(id);
            Assert.IsNotNull(location);
            Assert.AreEqual(location.Id, id);
            Assert.IsNotNull(location.Region);

            Console.WriteLine("Location found: id = {0}, Name = {1}, Region = {2}",
                              location.Id, location.Name, location.Region.Name);

        }

        [TestMethod]
        public void ShouldGetLocationByIdIncludeRegion()
        {
            const int id = 1;
            IQueryable<Location> query = _repository.Get<Location>(
                l => l.Id == id,
                q => q.OrderByDescending(l => l.Id),
                // (OK, so this is not needed here, but add it in so we can see the "order by" clause in the generated SQL)
                new Expression<Func<Location, object>>[]
                    {
                        (l => l.Region)
                    });
            Location location = query.FirstOrDefault();
            Assert.IsNotNull(location);
            Assert.AreEqual(location.Id, id);
            Assert.IsNotNull(location.Region);

            Console.WriteLine("Location found: id = {0}, Name = {1}, Region = {2}",
                              location.Id, location.Name, location.Region.Name);
        }

        [TestMethod]
        public void ShouldUpdateLocation()
        {
            const int id = 2;
            const string testDescription = "Test Description";
            _repository.BeginTransaction();

            Location location = _repository.GetById<Location>(id);
            Assert.IsNotNull(location);
            Assert.AreEqual(location.Id, id);
            Assert.AreNotEqual(location.Description, testDescription);

            location.Description = testDescription;
            _repository.Update(location);
            _repository.SaveChanges();

            Location savedLocation = _repository.GetById<Location>(id);
            Assert.AreEqual(savedLocation.Description, testDescription);

            _repository.RollbackTransaction();

        }

        [TestMethod]
        public void ShouldGetAllTrailTypes()
        {
            IEnumerable<TrailType> trailTypes = _repository.List<TrailType>();
            Assert.IsTrue(trailTypes.Count() > 0);
            Console.WriteLine("There are {0} trailTypes", trailTypes.Count());

        }
    }
}
