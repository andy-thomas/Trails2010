using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using Trails2012.DataAccess;
using Trails2012.Domain;

namespace Trails2012.Controllers
{
    public class LocationController : Controller
    {
       private readonly IRepository _repository;
        private List<Region> _regions;

        [ImportingConstructor]
       public LocationController(IRepository repository)
        {
            _repository = repository;
            PopulateRegions();
        }

        private void PopulateRegions()
        {
            _regions = new List<Region>(_repository.ListIncluding<Region>(null));
            //_regions = new List<Region>(_repository.List<Region>());  // Don't do this - see comment 1 below
            ViewData["regions"] = _regions;
        }

        public ActionResult LocationGrid()
        {
            ViewData["regions"] = _regions;
            List<Location> locations = new List<Location>(_repository.ListIncluding<Location>(l => l.Region));
            return PartialView("_SelectAjaxEditing", locations);
        }

        [GridAction]
        public ActionResult SelectAjaxEditing()
        {
            ViewData["regions"] = _regions;
            List<Location> locations = new List<Location>(_repository.ListIncluding<Location>(l => l.Region));
            var gridModel = new GridModel(locations);
            return PartialView(gridModel);
        }

        [HttpPost]
        [GridAction]
        public ActionResult EditAjaxEditing(Location location)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(location);

                // see Comment 2
                Region region = _repository.GetById<Region>(location.RegionId);
                location.Region = region;
 
                _repository.SaveChanges();
            }
            ViewData["regions"] = _regions;
            var locations = new List<Location>(_repository.ListIncluding<Location>(l => l.Region));
            return PartialView("_SelectAjaxEditing", new GridModel(locations));
        }

        [GridAction]
        [HttpPost]
        public ActionResult CreateAjaxEditing(Location location)
        {
            if (ModelState.IsValid)
            {
                _repository.Insert(location);
                
                // see Comment 2
                Region region = _repository.GetById<Region>(location.RegionId);
                location.Region = region;

                _repository.SaveChanges();
            }
            ViewData["regions"] = _regions;
            var locations = new List<Location>(_repository.ListIncluding<Location>(l => l.Region));
            return PartialView("_SelectAjaxEditing", new GridModel(locations));
        }

        [GridAction]
        [HttpPost]
        public ActionResult DeleteAjaxEditing(int id)
        {
            Location location = _repository.GetById<Location>(id);
            _repository.Delete(location);
            _repository.SaveChanges();
            ViewData["regions"] = _regions;
            var locations = new List<Location>(_repository.ListIncluding<Location>(l => l.Region));
            return PartialView("_SelectAjaxEditing", new GridModel(locations));
        }



    }
}

// _Comment 1_
// Do not use the List<> method to get the list of Regions for the dropdown box. 
// This is because EF creates a list of DynamicProxies, and these are cached when the 
// Locations are subsequently loaded up a few milliseconds later.
// If the Locations are populated with lists of DynamicProxies in the Regions property, then 
// the Telerik grid chokes with a JSON serialization circular reference error.

// _Comment 2_
// Andy - The RegionId has been updated from the form.
// Assign the Region - this step is only necessary if using the current NHib reprository.
// (The current EF repository is mapped to work with the foreign key RegionId directly, 
// but NHib works through the related Region object)
