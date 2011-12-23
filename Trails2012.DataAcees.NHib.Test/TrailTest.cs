using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trails2012.Domain;

namespace Trails2012.DataAccess.NHib.Test
{
    [TestClass]
    public class TrailTest
    {
        private static IRepository _repository;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            bool useSQLite = true;
            _repository = new NHibRepository(useSQLite);
            if(useSQLite) Util.SeedData(_repository);
        }

        [TestMethod]
        public void ShouldGetAllTrails()
        {
            IEnumerable<Trail> trails = _repository.List<Trail>();
            Assert.IsTrue(trails != null && trails.Count() >= 0);
            Console.WriteLine("There are {0} trails", trails.Count());
        }

        [TestMethod]
        public void ShouldGetTrailById()
        {
            const int id = 1;
            Trail trail = _repository.GetById<Trail>(id);
            if (trail != null)
            {
                Assert.IsNotNull(trail);
                Assert.AreEqual(trail.Id, id);
                Assert.IsNotNull(trail.Location);   // TODO Why does this fail when using SQLite?
                Assert.IsNotNull(trail.Difficulty);
                Assert.IsNotNull(trail.TrailType);
                Console.WriteLine(
                    "Trail found: id = {0}, Name = {1}, Location = {2}, Region = {3}, TrailType = {4}, Difficulty = {5}",
                    trail.Id, trail.Name, trail.Location.Name, trail.Location.Region.Name, trail.TrailType.TrailTypeName,
                    trail.Difficulty.DifficultyType);
            }
        }

        [TestMethod]
        public void ShouldGetTrailByIdUsingLinqOnList()
        {
            const int id = 1;
            IEnumerable<Trail> trails = _repository.List<Trail>();
            if (trails != null)
            {
                Trail trail = trails.FirstOrDefault(t => t.Id == id);
                Assert.IsNotNull(trail);
                Assert.AreEqual(trail.Id, id);
                Assert.IsNotNull(trail.Location); // TODO Why does this fail when using SQLite?
                Assert.IsNotNull(trail.Difficulty);
                Assert.IsNotNull(trail.TrailType);
                //Assert.IsNotNull(trail.Image);
                Console.WriteLine(
                    "Trail found: id = {0}, Name = {1}, Location = {2}, Region = {3}, TrailType = {4}, Difficulty = {5}",
                    trail.Id, trail.Name, trail.Location.Name, trail.Location.Region.Name, trail.TrailType.TrailTypeName,
                    trail.Difficulty.DifficultyType);
            }
        }

    }
}
