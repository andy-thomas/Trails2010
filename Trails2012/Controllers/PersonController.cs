using System;
using System.Collections;
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
            throw new InvalidOperationException("Oops - that wasn't supposed to happen! :-)");
            ViewData["GenderList"] = PopulateGenderList(); 
            return View(new List<Person>(_repository.List<Person>()));
        }

        //
        // GET: /Person/Details/5

        public ViewResult Details(int id)
        {
            Person person = _repository.GetById<Person>(id);
            ViewData["GenderList"] = PopulateGenderList();
            return View(person);
        }

        //
        // GET: /Person/Create

        public ActionResult Create()
        {
            Person person = new Person();
            return PartialView("_Create", person);
        } 

        //
        // POST: /Person/Create

        [HttpPost]
        public ActionResult Create(Person person)
        {
            ViewData["GenderList"] = PopulateGenderList();
            if (ModelState.IsValid)
            {
                _repository.Insert(person);
                _repository.SaveChanges();
            }
            else
            {
                if (Request.IsAjaxRequest())
                    return View("Create", person);
                else
                    return RedirectToAction("Index");
            }
            
            if (Request.IsAjaxRequest())
            {
                IList<Person> persons = new List<Person>(_repository.List<Person>());
                return PartialView("_List", persons);
            }
            else
                return RedirectToAction("Index");
        }
        
        //
        // GET: /Person/Edit/5
 
        public ActionResult Edit(int id)
        {
            Person person = _repository.GetById<Person>(id);
            return PartialView("_Edit",person);
        }

        //
        // POST: /Person/Edit/5

        [HttpPost]
        public ActionResult Edit(Person person)
        {
            try
            {
                IList<Person> persons = null;

                if (ModelState.IsValid)
                {
                    _repository.Update(person);
                    _repository.SaveChanges();
                 }
                else
                {
                    if (Request.IsAjaxRequest())
                        return View("Edit", person);
                    else
                        return RedirectToAction("Index");                 
                }
                ViewData["GenderList"] = PopulateGenderList();
                persons = new List<Person>(_repository.List<Person>());

                if (Request.IsAjaxRequest())
                    return PartialView("_List", persons);
                else
                    return RedirectToAction("Index");
            }
            catch 
            {
                return RedirectToAction("Index");
            }
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

        private IEnumerable PopulateGenderList()
        {
             return new Dictionary<string, string> { { "M", "Male" }, { "F", "Female" } };
        }
    }
}