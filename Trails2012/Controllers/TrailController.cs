using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using Trails2012.DataAccess;
using Trails2012.Domain;

namespace Trails2012.Controllers
{   
    public class TrailController : Controller
    {
        private readonly IRepository _repository;

        [ImportingConstructor]
        public TrailController(IRepository repository)
        {
            _repository = repository;
        }


        //
        // GET: /Trail/

        public ViewResult Index()
        {
            return View(new List<Trail>(_repository.List<Trail>()));
        }

        //
        // GET: /Trail/Details/5

        public ViewResult Details(int id)
        {
            return View(_repository.GetById<Trail>(id));
        }

        //
        // GET: /Trail/Create

        public ActionResult Create()
        {
			ViewBag.PossibleLocations = _repository.List<Location>();
            ViewBag.PossibleTrailTypes = _repository.List<TrailType>();
            ViewBag.PossibleDifficulties = _repository.List<Difficulty>();
            return View();
        } 

        //
        // POST: /Trail/Create

        [HttpPost]
        public ActionResult Create(Trail trail)
        {
            if (ModelState.IsValid) {
                _repository.Insert(trail);
                _repository.SaveChanges();
                return RedirectToAction("Index");
            } else {
                ViewBag.PossibleLocations = _repository.List<Location>();
                ViewBag.PossibleTrailTypes = _repository.List<TrailType>();
                ViewBag.PossibleDifficulties = _repository.List<Difficulty>();
                return View();
			}
        }
        
        //
        // GET: /Trail/Edit/5
 
        public ActionResult Edit(int id)
        {
            ViewBag.PossibleLocations = _repository.List<Location>();
            ViewBag.PossibleTrailTypes = _repository.List<TrailType>();
            ViewBag.PossibleDifficulties = _repository.List<Difficulty>();
            Trail trail = _repository.GetById<Trail>(id);
            return View(trail);
        }

        //
        // POST: /Trail/Edit/5

        [HttpPost]
        public ActionResult Edit(Trail trail)
        {
            if (ModelState.IsValid) {
                _repository.Update(trail);
                _repository.SaveChanges();
                return RedirectToAction("Index");
            } else {
                ViewBag.PossibleLocations = _repository.List<Location>();
                ViewBag.PossibleTrailTypes = _repository.List<TrailType>();
                ViewBag.PossibleDifficulties = _repository.List<Difficulty>();
                return View();
			}
        }

        //
        // GET: /Trail/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(_repository.GetById<Trail>(id));
        }

        //
        // POST: /Trail/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _repository.Delete<Trail>(id);
            _repository.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
