// ReSharper disable RedundantTypeArgumentsOfMethod
using System;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trails2012.Domain;

namespace Trails2012.DataAccess.EF.Test
{
    [TestClass]
    public class ReferenceListTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            Database.SetInitializer<TrailsContext>(new TrailsInitializer());
        }
        
        [TestMethod]
        public void ShouldGetAllRegions()
        {
            using (TrailsContext context = new TrailsContext())
            {
                DbSet<Region> regions = context.Regions;
                regions.Load();
                Assert.IsTrue(regions.Local.Count > 0);
                Console.WriteLine("There are {0} regions", regions.Count());
            }
        }

        [TestMethod]
        public void ShouldGetAllDifficulties()
        {
            using (TrailsContext context = new TrailsContext())
            {
                DbSet<Difficulty> difficultys = context.Difficulties;
                difficultys.Load();
                Assert.IsTrue(difficultys.Local.Count > 0);
                Console.WriteLine("There are {0} difficulty levels", difficultys.Count());
            }
        }

        [TestMethod]
        public void ShouldGetAllLocations()
        {
            using (TrailsContext context = new TrailsContext())
            {
                DbSet<Location> locations = context.Locations;
                locations.Load();
                Assert.IsTrue(locations.Local.Count > 0);
                Console.WriteLine("There are {0} locations", locations.Count());
            }
        }

        [TestMethod]
        public void ShouldGetLocationById()
        {
            const int id = 1;
            using (TrailsContext context = new TrailsContext())
            {
                DbSet<Location> locations = context.Locations;
                if (locations != null)
                {
                    Location location = locations.Find(id);
                    Assert.IsNotNull(location);
                    Assert.AreEqual(location.Id, id);
                    Assert.IsNotNull(location.Region);

                    Console.WriteLine("Location found: id = {0}, Name = {1}, Region = {2}",
                        location.Id, location.Name, location.Region.Name);
                }
            }
        }
    }
}
// ReSharper restore RedundantTypeArgumentsOfMethod
