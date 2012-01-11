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
    public class TripTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            Database.SetInitializer<TrailsContext>(new TrailsInitializer());          
        }

        [TestMethod]
        public void ShouldGetAllTrips()
        {
            using (TrailsContext context = new TrailsContext())
            {
                DbSet<Trip> trips = context.Set<Trip>();  // or use context.Trips;
                trips.Load();
                Assert.IsTrue(trips.Local.Count > 0);
                Console.WriteLine("There are {0} trips", trips.Count());
            }
        }

        [TestMethod]
        public void ShouldGetTripById()
        {
            const int id = 1;
            using (TrailsContext context = new TrailsContext())
            {
                DbSet<Trip> trips = context.Trips;
                if (trips != null)
                {
                    Trip trip = trips.Find(id);
                    Assert.AreEqual(trip.Id, id);
                    Console.WriteLine("Trip found: id = {0}, Date = {1}, TimeTaken = {2}, Transport Type = {3}, Number of people on trip = {4}",
                        trip.Id, trip.Date, trip.TimeTaken??0, trip.TransportType.TransportTypeName, trip.Persons.Count);
                }
            }
        }

        [TestMethod]
        public void ShouldGetTripsByTransportType()
        {
            using (TrailsContext context = new TrailsContext())
            {
                DbSet<Trip> trips = context.Trips;
                var tripQuery = from p in trips
                                 where p.TransportType.TransportTypeName.Contains("Hike") 
                                    select p;
                List<Trip> tripList = tripQuery.ToList();
                Assert.IsTrue(tripList.Count > 0);
                Console.WriteLine("There are {0} trips with the specified transport type", tripList.Count());
            }
        }

        [TestMethod]
        public void ShouldAddNewTrip()
        {
            // To turn on DTS on my computer, 
            // see http://alexduggleby.com/2008/08/24/msdtc-unavailable-for-sql-express-transactions-or-who-took-my-msdtc-settings-on-vista/
            // or http://geekswithblogs.net/narent/archive/2006/10/09/93544.aspx
           using (new TransactionScope()) 
            {
                using (TrailsContext context = new TrailsContext())
                {
                    DbSet<Trip> trips = context.Trips;
                    trips.Load();
                    int priorCount = trips.Local.Count;

                    DbSet<Person> persons = context.Persons;
                    persons.Load();

                    Trip newTrip = new Trip
                    {
                        Date = DateTime.Today,
                        TimeTaken = 3.1M,
                        TransportTypeId = 1,
                        TrailId = 1,
                        Persons = new List<Person>{persons.Local.ElementAt(0), persons.Local.ElementAt(2)}
                    };

                    // Add the new trip to the context
                    Trip trip = trips.Add(newTrip);

                    // Save the context to the database, causing the database to be updated
                    context.SaveChanges();
                    // At this stage, the database has the new row, so the identity has been incremented and assigned 
                    Assert.IsTrue(trip.Id > 0);
                    Assert.IsTrue(newTrip.Id > 0);
 
                    trips.Load();
                    int postCount = trips.Local.Count;

                    Assert.AreEqual(postCount, priorCount + 1);
                }
                // Rollback the transaction, to make the test repeatable
            }
        }


    }


}
