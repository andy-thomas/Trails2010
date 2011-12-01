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
            _regions = new List<Region>(_repository.List<Region>());
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
