using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trails2012.Domain;

namespace Trails2012.DataAccess.NHib.Test
{
    [TestClass]
    public class TripTest
    {
        private static IRepository _repository;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            bool useSQLite = true;
            _repository = new NHibRepository(useSQLite);
            if (useSQLite)
            {
                Util.SeedData(_repository);

                // Now clear the cached objects from the session:
                // Some objects were added to the session during the seeding process;
                // We want the unit tests to get the ones from the database afresh.
                ((NHibRepository)_repository).ClearSession();
            }
        }

        [TestMethod]
        public void ShouldGetAllTrips()
        {
            IEnumerable<Trip> trips = _repository.List<Trip>();
            Assert.IsTrue(trips != null && trips.Count() >= 0);
            Console.WriteLine("There are {0} trips", trips.Count());
        }

        [TestMethod]
        public void ShouldGetTripById()
        {
            const int id = 1;
            Trip trip = _repository.GetById<Trip>(id);
            if (trip != null)
            {
                Assert.IsNotNull(trip);
                Assert.AreEqual(trip.Id, id);
                Assert.IsNotNull(trip.Trail);   
                Assert.IsNotNull(trip.TransportType);
                Console.WriteLine("Trip found: id = {0}, Date = {1}, TimeTaken = {2}, Transport Type = {3}, Number of people on trip = {4}",
                    trip.Id, trip.Date, trip.TimeTaken ?? 0, trip.TransportType.TransportTypeName, trip.Persons.Count);
            }
        }

        [TestMethod]
        public void ShouldGetTripByIdUsingLinqOnList()
        {
            const int id = 1;
            IEnumerable<Trip> trips = _repository.List<Trip>();
            if (trips != null)
            {
                Trip trip = trips.FirstOrDefault(t => t.Id == id);
                Assert.IsNotNull(trip);
                Assert.AreEqual(trip.Id, id);
                Assert.IsNotNull(trip.Trail);   
                Assert.IsNotNull(trip.TransportType);
                Console.WriteLine("Trip found: id = {0}, Date = {1}, TimeTaken = {2}, Transport Type = {3}, Number of people on trip = {4}",
                    trip.Id, trip.Date, trip.TimeTaken ?? 0, trip.TransportType.TransportTypeName, trip.Persons.Count);
            }
        }

    }
}
