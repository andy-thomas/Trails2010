using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using Trails2012.DataAccess;
using Trails2012.Domain;
using Telerik.Web.Mvc;

namespace Trails2012.Controllers
{ 
    public class RegionController : Controller
    {
        private readonly IRepository _repository;

        [ImportingConstructor]
        public RegionController(IRepository repository)
        {
            _repository = repository;
        }

        //
        // GET: /Region/

        public ViewResult Index()
        {
            return View(new List<Region>(_repository.ListIncluding<Region>(r => r.Locations)));
        }

        public ActionResult RegionGrid()
        {
            return PartialView("_SelectAjaxEditing", new List<Region>(_repository.ListIncluding<Region>(r => r.Locations)));
        }

        [GridAction]
        public ActionResult SelectAjaxEditing()
        {
            List<Region> regions = new List<Region>(_repository.ListIncluding<Region>(r => r.Locations));
            var gridModel = new GridModel(regions);
            return PartialView(gridModel);
        }

        [HttpPost]
        [GridAction]
        public ActionResult EditAjaxEditing(Region region)
        {
            Region savedRegion = _repository.GetById<Region>(region.Id);
            if (ModelState.IsValid)
            {
                UpdateModel(savedRegion);
                _repository.Update(savedRegion);
                _repository.SaveChanges();
            }
            var regions = new List<Region>(_repository.ListIncluding<Region>(r => r.Locations));
            return PartialView("_SelectAjaxEditing", new GridModel(regions));
        }

        [GridAction]
        [HttpPost]
        public ActionResult CreateAjaxEditing(Region region)
        {
            if (ModelState.IsValid)
            {
                _repository.Insert(region);
                _repository.SaveChanges();
            }
            var regions = new List<Region>(_repository.ListIncluding<Region>(r => r.Locations));
            return PartialView("_SelectAjaxEditing", new GridModel(regions));
        }

        [GridAction]
        [HttpPost]
        public ActionResult DeleteAjaxEditing(int id)
        {
            Region region = _repository.GetById<Region>(id);
            _repository.Delete(region);
            _repository.SaveChanges();
            var regions = new List<Region>(_repository.ListIncluding<Region>(r => r.Locations));
            return PartialView("_SelectAjaxEditing", new GridModel(regions));
        }

        //
        // GET: /Region/Details/5

        public ViewResult Details(int id)
        {
            Region region = _repository.GetById<Region>(id);
            return View(region);
        }

        //
        // GET: /Region/Create

        //public ActionResult Create()
        //{
        //    return View();
        //} 

        //
        // POST: /Region/Create

        [HttpPost]
        public ActionResult Create(Region region)
        {
            if (ModelState.IsValid)
            {
                _repository.Insert(region);
                _repository.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(region);
        }

        //
        // GET: /Region/Edit/5
 
        //public ActionResult Edit(int id)
        //{
        //    Region region = _repository.GetById<Region>(id);
        //    return View(region);
        //}

        //
        // POST: /Region/Edit/5

        [HttpPost]
        public ActionResult Edit(Region region)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(region);
                _repository.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(region);
        }

        //
        // GET: /Region/Delete/5
 
        //public ActionResult Delete(int id)
        //{
        //    Region region = _repository.GetById<Region>(id);
        //    return View(region);
        //}

        //
        // POST: /Region/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Region region = _repository.GetById<Region>(id);
            _repository.Delete(region);
            _repository.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
            base.Dispose(disposing);
        }
    }


}