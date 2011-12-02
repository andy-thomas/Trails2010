using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trails2012.DataAccess;
using Trails2012.Domain;
using Trails2012.Models;

namespace Trails2012.Controllers
{
    public class TrailController1 : Controller
    {
        private readonly IRepository _repository;

        [ImportingConstructor]
        public TrailController1(IRepository repository)
        {
            _repository = repository;          
        }

        //
        // GET: /Trail/

        public ActionResult Index()
        {
            return View(new List<Trail>(_repository.List<Trail>()));
        }

        //
        // GET: /Trail/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Trail/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Trail/Create

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
        // GET: /Trail/Edit/5
 
        public ActionResult Edit(int id)
        {
            Trail trail = _repository.GetById<Trail>(id);
            IEnumerable<TrailType> trailTypes = _repository.List<TrailType>();
            //ViewData["TrailTypes"] = new SelectList(trailTypes, "Id", "TrailTypeName");
            TrailModel model = new TrailModel
                                   {
                                       Trail = trail,
                                       TrailTypes = new SelectList(trailTypes, "Id", "TrailTypeName")
                                   };
            return View(model);
        }

        //
        // POST: /Trail/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, Trail trail)
        {
            try
            {
                _repository.Update(trail);
                _repository.SaveChanges();
 
                return RedirectToAction("Index");
            }
            catch
            {
                IEnumerable<TrailType> trailTypes = _repository.List<TrailType>();
                return View();
            }
        }

        //
        // GET: /Trail/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Trail/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
