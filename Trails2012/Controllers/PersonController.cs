using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using Trails2012.DataAccess;
using Trails2012.Domain;

namespace Trails2012.Controllers
{ 
    public class PersonController : Controller
    {
        private readonly IRepository _repository;

        [ImportingConstructor]
        public PersonController(IRepository repository)
        {
            _repository = repository;
        }

        //
        // GET: /Person/

        public ViewResult Index()
        {
            return View(new List<Person>(_repository.List<Person>()));
        }

        //
        // GET: /Person/Details/5

        public ViewResult Details(int id)
        {
            Person person = _repository.GetById<Person>(id);
            return View(person);
        }

        //
        // GET: /Person/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Person/Create

        [HttpPost]
        public ActionResult Create(Person person)
        {
            if (ModelState.IsValid)
            {
                _repository.Insert(person);
                _repository.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(person);
        }
        
        //
        // GET: /Person/Edit/5
 
        public ActionResult Edit(int id)
        {
            Person person = _repository.GetById<Person>(id);
            return View(person);
        }

        //
        // POST: /Person/Edit/5

        [HttpPost]
        public ActionResult Edit(Person person)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(person);
                _repository.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        //
        // GET: /Person/Delete/5
 
        public ActionResult Delete(int id)
        {
            Person person = _repository.GetById<Person>(id);
            return View(person);
        }

        //
        // POST: /Person/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = _repository.GetById<Person>(id);
            _repository.Delete(person);
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