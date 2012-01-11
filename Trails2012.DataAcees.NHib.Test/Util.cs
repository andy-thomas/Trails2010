using System;
using System.Collections.Generic;
using Trails2012.Domain;

namespace Trails2012.DataAccess.NHib.Test
{
    class Util
    {
        internal static void SeedData(IRepository repository)
        {
            Console.WriteLine("Seeding data...");

            // Seed Persons
            var persons = new List<Person>
                              {
                                  new Person {FirstName = "Arthur", LastName = "Smith", Gender = "M"},
                                  new Person {FirstName = "Bert", LastName = "Jones", Gender = "M"},
                                  new Person {FirstName = "Charlie", LastName = "Robertson", Gender = "M"}
                              };

            persons.ForEach(p => repository.Insert(p));

            // Seed Regions
            var regions = new List<Region>
                              {
                                  new Region {Name = "Banff National Park - South", Description = "Banff National Park"},
                                  new Region {Name = "Jasper National Park"},
                                  new Region {Name = "Kootenay National Park"},
                                  new Region {Name = "Banff National Park - North"},
                                  new Region {Name = "Kananaskis Country"}
                              };
            regions.ForEach(r => repository.Insert(r));

            // Seed Locations
            var locations = new List<Location>
                                {
                                    new Location
                                        {
                                            Name = "Bow Valley Parkway",
                                            Description = "Road from Banff to Lake Louise",
                                            Directions = "Turn left off main highway 2km north of Banff",
                                            Region = regions[0]
                                        },
                                    new Location {Name = "Banff Townsite area", Region = regions[0]},
                                    new Location
                                        {
                                            Name = "Bow Lake area",
                                            Description = "20 minutes north of Lake Louise on IceField Parkway",
                                            Region = regions[3]
                                        },
                                    new Location {Name = "Smith Dorrien Highway", Region = regions[4]}
                                };
            locations.ForEach(l => repository.Insert(l));

            // Seed Difficulty Levels
            var difficulties = new List<Difficulty>
                                   {
                                       new Difficulty {DifficultyType = "Easy"},
                                       new Difficulty {DifficultyType = "Moderate"},
                                       new Difficulty {DifficultyType = "Challenging"},
                                       new Difficulty {DifficultyType = "Difficult"}
                                   };
            difficulties.ForEach(d => repository.Insert(d));

            // Seed TrailType Levels
            var trailTypes = new List<TrailType>
                                 {
                                     new TrailType {TrailTypeName = "Land"},
                                     new TrailType {TrailTypeName = "Air"},
                                     new TrailType {TrailTypeName = "Water"}
                                 };
            trailTypes.ForEach(t => repository.Insert(t));

            // Seed TransportType Levels
            var transportTypes = new List<TransportType>
                                     {
                                         new TransportType {TransportTypeName = "Hike"},
                                         new TransportType {TransportTypeName = "Cycle"},
                                         new TransportType {TransportTypeName = "Canoe"},
                                         new TransportType {TransportTypeName = "Ski"},
                                         new TransportType {TransportTypeName = "Snowshoe"},
                                         new TransportType {TransportTypeName = "Aeroplane"}
                                     };
            transportTypes.ForEach(t => repository.Insert(t));

            // Seed Trails
            var trails = new List<Trail>
                             {
                                 new Trail
                                     {
                                         Name = "Johnstone Canyon",
                                         Description = "Johnstone Canyon",
                                         Distance = 4.8M,
                                         ElevationGain = 200,
                                         EstimatedTime = 2,
                                         Location = locations[0],
                                         TrailType = trailTypes[0],
                                         Difficulty = difficulties[0],
                                         ReturnOnEffort = 9.2M,
                                         OverallGrade = 7.2M,
                                         Notes =
                                             "Good Waterfalls scenery. Good in all seasons. Take crampons in winter/spring",
                                         Longitude = -102,
                                         Latitude = 50
                                     },
                                 new Trail
                                     {
                                         Name = "Burstall Pass",
                                         Description = "Burstall Pass",
                                         Distance = 8.53M,
                                         ElevationGain = 390,
                                         EstimatedTime = 4,
                                         Location = locations[1],
                                         TrailType = trailTypes[0],
                                         Difficulty = difficulties[0],
                                         ReturnOnEffort = 8.1M,
                                         OverallGrade = 8.1M,
                                         Notes = "Excellent view from the pass"
                                     },
                                 new Trail
                                     {
                                         Name = "Helen Lake",
                                         Description = "Helen Lake",
                                         Distance = 8M,
                                         ElevationGain = 480,
                                         EstimatedTime = 4.5M,
                                         Location = locations[2],
                                         TrailType = trailTypes[0],
                                         Difficulty = difficulties[3],
                                         ReturnOnEffort = 7.8M,
                                         OverallGrade = 7.8M,
                                         Notes = "Views across to the Dolomite range."
                                     },
                                 new Trail
                                     {
                                         Name = "Chester Lake",
                                         Description = "Chester Lake",
                                         Distance = 8.11M,
                                         ElevationGain = 520,
                                         EstimatedTime = 4.5M,
                                         Location = locations[2],
                                         TrailType = trailTypes[0],
                                         Difficulty = difficulties[1],
                                         ReturnOnEffort = 6.9M,
                                         OverallGrade = 6.9M,
                                         Notes = "Don't stop at Chester Lake - go on to Three Lake Valley"
                                     }
                             };

            trails.ForEach(t => repository.Insert(t));

            // Seed Trips
            var trips = new List<Trip>
                            {
                                new Trip
                                    {
                                        Date = new DateTime(2001, 9, 1),
                                        TransportType = transportTypes[0],
                                        Trail = trails[0],
                                        Weather = "Cloudy",
                                        TimeTaken = 3.3M,
                                        Notes = "Very slippery - cramps needed.",
                                        Persons = new List<Person> 
                                        {
                                            persons[0], persons[1],persons[2]
                                        }
                                    },
                                new Trip
                                    {
                                        Date = new DateTime(2004, 7, 31),
                                        TransportType = transportTypes[1],
                                        Trail = trails[0],
                                        Weather = "Sunny",
                                        TimeTaken = 2.3M,
                                        Notes = "First time on this trail."

                                    }
                            };
            trips.ForEach(t => repository.Insert(t));

            repository.SaveChanges();
        }


    }
}
