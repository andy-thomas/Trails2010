using System;
using System.Collections.Generic;
using System.Data.Entity;
using Trails2012.Domain;

namespace Trails2012.DataAccess.EF.Test
{
    public class TrailsInitializer : DropCreateDatabaseIfModelChanges<TrailsContext>
    {
        // This class handles the re-generation of the database whenever the domain model and/or 
        // mapping configuration changes 
        // (by comparing context against the metadata in the EdmMetadata table on the database, if it exists)
        // Whenever this happens, the database is regenerated from scratch, so all of the existing data is lost
        // This method seeds the database with known data
        // This data is used in the automated integration tests in this project.

        protected override void Seed(TrailsContext context)
        {
            base.Seed(context);
            Console.WriteLine("Seeding data...");

            // Seed Persons
            var persons = new List<Person>
                              {
                                  new Person {FirstName = "Alf", LastName = "Smith", Gender = "M"},
                                  new Person {FirstName = "Bert", LastName = "Jones", Gender = "M"},
                                  new Person {FirstName = "Charlie", LastName = "Robertson", Gender = "M"}
                              };

            persons.ForEach(p=>context.Persons.Add(p));

            // Seed Regions
            var regions = new List<Region>
                              {
                                  new Region {Name = "Banff National Park - South", Description = "Banff National Park"},
                                  new Region {Name = "Jasper National Park"},
                                  new Region {Name = "Kootenay National Park"},
                                  new Region {Name = "Banff National Park - North"},
                                  new Region {Name = "Kananaskis Country"}
                              };
            regions.ForEach(r => context.Regions.Add(r));

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
            locations.ForEach(l => context.Locations.Add(l));

            // Seed Difficulty Levels
            var difficulties = new List<Difficulty>
                                   {
                                       new Difficulty {DifficultyType = "Easy"},
                                       new Difficulty {DifficultyType = "Moderate"},
                                       new Difficulty {DifficultyType = "Challenging"},
                                       new Difficulty {DifficultyType = "Difficult"}
                                   };
            difficulties.ForEach(d => context.Difficulties.Add(d));

            // Seed TrailType Levels
            var trailTypes = new List<TrailType>
                                   {
                                       new TrailType {TrailTypeName = "Land"},
                                       new TrailType {TrailTypeName = "Air"},
                                       new TrailType {TrailTypeName = "Water"}
                                   };
            trailTypes.ForEach(t => context.TrailTypes.Add(t));

            // Seed Trails
            var trails = new List<Trail>
                              {
                                  new Trail {Name = "Johnstone Canyon", Description = "Johnstone Canyon", Distance = 4.8M, ElevationGain = 200, EstimatedTime = 2, LocationId = 1, TrailType = trailTypes[0], DifficultyId = 1, ReturnOnEffort = 9.2M, OverallGrade = 7.2M, Notes="Good Waterfalls scenery. Good in all seasons. Take crampons in winter/spring"},
                                  new Trail {Name = "Burstall Pass", Description = "Burstall Pass", Distance = 8.53M, ElevationGain = 390, EstimatedTime = 4, LocationId = 2, TrailType = trailTypes[0], DifficultyId = 1, ReturnOnEffort = 8.1M, OverallGrade = 8.1M, Notes="Excellent view from the pass"},
                                  new Trail {Name = "Helen Lake", Description = "Helen Lake", Distance = 8M, ElevationGain = 480, EstimatedTime = 4.5M, LocationId = 3, TrailType = trailTypes[0], DifficultyId = 4, ReturnOnEffort = 7.8M, OverallGrade = 7.8M, Notes="Views across to the Dolomite range."},
                                  new Trail {Name = "Chester Lake", Description = "Chester Lake", Distance = 8.11M, ElevationGain = 520, EstimatedTime = 4.5M, LocationId = 3, TrailType = trailTypes[0], DifficultyId = 2, ReturnOnEffort = 6.9M, OverallGrade = 6.9M, Notes="Don't stop at Chester Lake - go on to Three Lake Valley"}
                              };

            trails.ForEach(t => context.Trails.Add(t));

        }
    }
}
