using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using Trails2012.DataAccess;
using Trails2012.Domain;

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
            return View(new List<Region>(_repository.List<Region>()));
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

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Region/Create

        [HttpPost]
        public ActionResult Create(Region region)
        {
            if (ModelState.IsValid)
            {
                //db.Regions.Add(region);
                //db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(region);
        }
        
        //
        // GET: /Region/Edit/5
 
        public ActionResult Edit(int id)
        {
            Region region = _repository.GetById<Region>(id);
            return View(region);
        }

        //
        // POST: /Region/Edit/5

        [HttpPost]
        public ActionResult Edit(Region region)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(region).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(region);
        }

        //
        // GET: /Region/Delete/5
 
        public ActionResult Delete(int id)
        {
            Region region = _repository.GetById<Region>(id);
            return View(region);
        }

        //
        // POST: /Region/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            //Region region = db.Regions.Find(id);
            //db.Regions.Remove(region);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
            base.Dispose(disposing);
        }
    }
}