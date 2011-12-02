using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trails2012.Domain;

namespace Trails2012.DataAccess.EF.Test
{
    [TestClass]
    public class TrailDataAccessTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            Database.SetInitializer<TrailsContext>(new TrailsInitializer());          
        }

        [TestMethod]
        public void ShouldGetAllTrails()
        {
            using (TrailsContext context = new TrailsContext())
            {
                DbSet<Trail> trails = context.Set<Trail>();  // or use context.Trails;
                trails.Load();
                Assert.IsTrue(trails.Local.Count > 0);
                Console.WriteLine("There are {0} trails", trails.Count());
            }
        }

        [TestMethod]
        public void ShouldGetTrailById()
        {
            const int id = 1;
            using (TrailsContext context = new TrailsContext())
            {
                DbSet<Trail> trails = context.Trails;
                if (trails != null)
                {
                    Trail trail = trails.Find(id);
                    Assert.AreEqual(trail.Id, id);
                    Console.WriteLine("Trail found: id = {0}, Name = {1}, Location = {2}, Region = {3}, TrailType = {4}, Difficulty = {5}",
                        trail.Id, trail.Name, trail.Location.Name, trail.Location.Region.Name, trail.TrailType.TrailTypeName, trail.Difficulty.DifficultyType);
                }
            }
        }

        [TestMethod]
        public void ShouldGetTrailsByName()
        {
            using (TrailsContext context = new TrailsContext())
            {
                DbSet<Trail> trails = context.Trails;
                var trailQuery = from p in trails
                                 where p.Name.Contains("Lake") 
                                    select p;
                List<Trail> trailList = trailQuery.ToList();
                Assert.IsTrue(trailList.Count > 0);
                Console.WriteLine("There are {0} trails with the specified name", trailList.Count());
            }
        }

        [TestMethod]
        public void ShouldAddNewTrail()
        {
            using (new TransactionScope())
            {
                using (TrailsContext context = new TrailsContext())
                {
                    DbSet<Trail> trails = context.Trails;
                    trails.Load();
                    int priorCount = trails.Local.Count;

                    Trail newTrail = new Trail
                    {
                        Name = "testTrailName",
                        Description = "testDescription",
                        Distance = 32,
                        ElevationGain = 3000M,
                        TrailTypeId = 1,
                        LocationId = 1,
                        DifficultyId = 1
                    };

                    // Add the new trail to the context
                    Trail trail = trails.Add(newTrail);

                    // Save the context to the database, causing the database to be updated
                    context.SaveChanges();
                    // At this stage, the database has the new row, so the identity has been incremented and assigned 
                    Assert.IsTrue(trail.Id > 0);
                    Assert.IsTrue(newTrail.Id > 0);
 
                    trails.Load();
                    int postCount = trails.Local.Count;

                    Assert.AreEqual(postCount, priorCount + 1);
                }
                // Rollback the transaction, to make the test repeatable
            }
        }


    }


}
