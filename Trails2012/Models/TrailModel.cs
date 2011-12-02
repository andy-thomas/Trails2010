using System.Collections.Generic;
using Trails2012.Domain;

namespace Trails2012.Models
{
    public class TrailModel
    {
        public Trail Trail { get; set; }
        public IEnumerable<Difficulty> Difficulties { get; set; }
        public IEnumerable<Location> Locations { get; set; }
        public IEnumerable<TrailType> TrailTypes { get; set; }

    }
}