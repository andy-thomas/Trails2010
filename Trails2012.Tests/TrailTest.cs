using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trails2012.DataAccess;
using Trails2012.Domain;

namespace Trails2012.Tests
{
    [TestClass]
    public class TrailTest
    {
        private const int Id = 1;
        private CompositionContainer _container;
        private RepositoryFactory _repositoryFactory;

        [TestInitialize]
        public void Setup()
        {
            // See notes in TrailTest class about wiring up the repository
            DirectoryCatalog repositoryCatalog = new DirectoryCatalog(@".\PlugIns"); 
            _repositoryFactory = new RepositoryFactory();

            // Now, set up the MEF container and use it to hydrate the _repositoryFactory object
            _container = new CompositionContainer(repositoryCatalog);
            _container.SatisfyImportsOnce(_repositoryFactory);

        }

        [TestMethod]
        public void ShouldPlugInARepository()
        {
            Assert.IsNotNull(_repositoryFactory.Repository);
            Console.WriteLine(@"We are using the following repository: {0}", _repositoryFactory.Repository.GetType());
        }

        [TestMethod]
        public void ShouldGetAllTrails()
        {
            IEnumerable<Trail>trails = _repositoryFactory.Repository.List<Trail>();
            Assert.IsNotNull(trails);
            int count =trails.Count();
            Assert.IsTrue(count > 0);
            Console.WriteLine(@"There are {0} trails", count);
        }

        [TestMethod]
        public void ShouldGetTrailById()
        {
           Trail trail = _repositoryFactory.Repository.GetById<Trail>(Id);
           Assert.IsNotNull(trail);
           Assert.AreEqual(trail.Id, Id);
            //Console.WriteLine(@"Trail found: id = {0}, Name = {1}",
            //                 trail.Id,trail.Name);
            Console.WriteLine(
                @"Trail found: id = {0}, Name = {1}, Location = {2}, Region = {3}, TrailType = {4}, Difficulty = {5}",
                trail.Id, trail.Name, trail.Location.Name, trail.Location.Region.Name, trail.TrailType.TrailTypeName,
                trail.Difficulty.DifficultyType);

        }

        [TestMethod]
        public void ShouldGetTrailsByName()
        {
            // Get alltrails whose name contains "on" and sort by first name descending
            IQueryable<Trail>trailQuery = _repositoryFactory.Repository.Get<Trail>(
                p => p.Name.Contains("Lake"),
                q => q.OrderByDescending(p => p.Description));
            List<Trail>trailList =trailQuery.ToList();
            Assert.IsTrue(trailList.Count > 0);
            Console.WriteLine(@"There are {0} trail with the specified name",trailList.Count());
        }

        [TestMethod]
        public void ShouldAddNewTrail()
        {
            IEnumerable<Trail>trails = _repositoryFactory.Repository.List<Trail>();
            int priorCount =trails.Count();

           Trail newTrail = new Trail
                                   {
                                       Description = "testDescription",
                                       Name = "testName",
                                       LocationId = 1,
                                       TrailTypeId = 1
                                   };

            // Begin transaction
            _repositoryFactory.Repository.BeginTransaction();

            // Add the newtrail to the context
           Trail trail = _repositoryFactory.Repository.Insert(newTrail);

            // Save the context to the database, causing the database to be updated
            _repositoryFactory.Repository.SaveChanges();

            // At this stage, the database has the new row, so the identity has been incremented and assigned 
            Assert.IsTrue(trail.Id > 0);
            Assert.IsTrue(newTrail.Id > 0);

           trails = _repositoryFactory.Repository.List<Trail>();
            int postCount =trails.Count();

            Assert.AreEqual(postCount, priorCount + 1);
            // Rollback the transaction, to make the test repeatable
            _repositoryFactory.Repository.RollbackTransaction();
        }

    }
}

    
