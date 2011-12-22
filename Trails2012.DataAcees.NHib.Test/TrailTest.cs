using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trails2012.DataAccess;
using Trails2012.DataAccess.NHib;
using Trails2012.Domain;

namespace Trails2012.DataAcees.NHib.Test
{
    [TestClass]
    public class TrailTest
    {
        [TestMethod]
        public void ShouldGetAllTrails()
        {
            using (IRepository repository = new NHibRepository())
            {
                IEnumerable<Trail> trails = repository.List<Trail>();  
                Assert.IsTrue(trails != null && trails.Count() > 0);
                Console.WriteLine("There are {0} trails", trails.Count());
            }
        }

        [TestMethod]
        public void ShouldGetTrailById()
        {
            const int id = 1;
            using (IRepository repository = new NHibRepository())
            {
                Trail trail = repository.GetById<Trail>(id);
                if (trail != null)
                {
                    Assert.AreEqual(trail.Id, id);
                    Console.WriteLine("Trail found: id = {0}, Name = {1}, Location = {2}, Region = {3}, TrailType = {4}, Difficulty = {5}",
                       trail.Id, trail.Name, trail.Location.Name, trail.Location.Region.Name, trail.TrailType.TrailTypeName, trail.Difficulty.DifficultyType);
                }
            }
        }

        [TestMethod]
        public void ShouldGetTrailByIdUsingLinqOnList()
        {
            const int id = 1;
            using (IRepository repository = new NHibRepository())
            {
                IEnumerable<Trail> trails = repository.List<Trail>();
                if (trails != null)
                {
                    Trail trail = trails.FirstOrDefault(t => t.Id == id);
                    Assert.AreEqual(trail.Id, id);
                    //Assert.IsNotNull(trail.Image);
                    Console.WriteLine("Trail found: id = {0}, Name = {1}, Location = {2}, Region = {3}, TrailType = {4}, Difficulty = {5}",
                        trail.Id, trail.Name, trail.Location.Name, trail.Location.Region.Name, trail.TrailType.TrailTypeName, trail.Difficulty.DifficultyType);
                }
            }
        }

    }
}
