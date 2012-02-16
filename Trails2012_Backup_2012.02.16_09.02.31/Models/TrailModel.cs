using System.Web.Mvc;
using Trails2012.Domain;

namespace Trails2012.Models
{
    public class TrailModel
    {
        public Trail Trail { get; set; }
        public SelectList Difficulties { get; set; }
        public SelectList Locations { get; set; }
        public SelectList TrailTypes { get; set; }

    }
}