using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using Trails2012.DataAccess;
using Trails2012.Domain;

namespace Trails2012.Controllers
{
    public class TripController : Controller
    {
        private readonly IRepository _repository;
        //private List<Region> _regions;

        [ImportingConstructor]
        public TripController(IRepository repository)
        {
            _repository = repository;
        }


        //
        // GET: /Trip/
        public ActionResult Index()
        {
            //ViewData["regions"] = _regions;
            List<Trip> trips  = new List<Trip>(_repository.ListIncluding<Trip>(t => t.Persons, t => t.Trail));
            return View(trips);
        }

        //
        // GET: /Trip/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Trip/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Trip/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Trip/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Trip/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [GridAction]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Trip trip = _repository.GetById<Trip>(id);
            //_repository.Delete(trip);
            //_repository.SaveChanges();
            //ViewData["regions"] = _regions;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Search(string searchTerm)
        {
            List<Trip> trips =
                new List<Trip>(
                    _repository.ListIncluding<Trip>(t => t.Persons, t => t.Trail).Where(
                        t => t.Trail.Name.ToUpper().Contains(searchTerm.ToUpper())));
            ViewData["SearchTerm"] = searchTerm;
            return View("Index", trips);
        }

    
        public JsonResult GetTrailNames(string searchTerm, int maxResults)
        {
             IEnumerable<Trail> trails = _repository.List<Trail>().Where(t => t.Name.ToUpper().Contains(searchTerm.ToUpper())).Take(10).ToList();
             List<object> results = new List<object>();
            foreach (Trail trail in trails)
            {
                results.Add(new {Id = trail.Id, Name = trail.Name});
            }

            return Json(results);           
        }

    }
}
