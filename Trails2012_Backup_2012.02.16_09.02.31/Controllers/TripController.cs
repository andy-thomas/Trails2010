using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
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
        private List<Person> _persons;

        private readonly IRepository _repository;
        //private List<Region> _regions;

        [ImportingConstructor]
        public TripController(IRepository repository)
        {
            _repository = repository;
        }


        //
        // GET: /Trip/
        public ActionResult Index(string searchTerm)
        {
            //ViewData["regions"] = _regions;
            List<Trip> trips;
            if (string.IsNullOrEmpty(searchTerm))
                trips = new List<Trip>(_repository.ListIncluding<Trip>(t => t.Persons, t => t.Trail));
            else
            {
                trips =
                    new List<Trip>(
                        _repository.ListIncluding<Trip>(t => t.Persons, t => t.Trail).Where(
                            t => t.Trail.Name.ToUpper().Contains(searchTerm.ToUpper())));
                ViewData["SearchTerm"] = searchTerm;
            }
            return View(trips);
        }

        //[GridAction]
        //public ActionResult Search(string searchTerm)
        //{
        //    List<Trip> trips;
        //    if (string.IsNullOrEmpty(searchTerm))
        //        trips = new List<Trip>(_repository.ListIncluding<Trip>(t => t.Persons, t => t.Trail));
        //    else
        //    {
        //        trips =
        //            new List<Trip>(
        //                _repository.ListIncluding<Trip>(t => t.Persons, t => t.Trail).Where(
        //                    t => t.Trail.Name.ToUpper().Contains(searchTerm.ToUpper())));
        //        ViewData["SearchTerm"] = searchTerm;
        //    }
        //    return View("Index", trips);
        //}

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
            Trip newTrip = new Trip();
            newTrip.Persons = new List<Person>();
            ViewBag.PossibleTransportTypes = _repository.List<TransportType>();
            ViewBag.PossibleTrails = _repository.List<Trail>();
            PopulatePersons();

            return View(newTrip);
        } 

        //
        // POST: /Trip/Create

        [HttpPost]
        public ActionResult Create(Trip trip, string personidlist)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    TransportType transportType = _repository.GetById<TransportType>(trip.TransportTypeId);
                    trip.TransportType = transportType;
                    Trail trail = _repository.GetById<Trail>(trip.TrailId);
                    trip.Trail = trail;

                    _repository.Insert(trip);

                    if (!string.IsNullOrEmpty(personidlist))
                    {
                        if (trip.Persons == null)
                            trip.Persons = new List<Person>();
                        string[] personIdArray = personidlist.Split(new[] {','});
                        foreach (string value in personIdArray)
                        {
                            int personId = Convert.ToInt32(value);
                            Person person = _repository.GetById<Person>(personId);
                            if(person != null)
                            {
                                trip.Persons.Add(person);
                            }
                        }
                    }

                    _repository.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.PossibleTransportTypes = _repository.List<TransportType>();
                ViewBag.PossibleTrails = _repository.List<Trail>();
                PopulatePersons();
                return View();

            }
            catch
            {
                ViewBag.PossibleTransportTypes = _repository.List<TransportType>();
                ViewBag.PossibleTrails = _repository.List<Trail>();
                PopulatePersons();
                return View();
            }
        }

        //
        // GET: /Trip/Edit/5
 
        public ActionResult Edit(int id)
        {
            ViewBag.PossibleTransportTypes = _repository.List<TransportType>();
            ViewBag.PossibleTrails = _repository.List<Trail>();

            //Trip trip = _repository.GetById<Trip>(id);
            Trip trip = _repository.Get<Trip>(t => t.Id.Equals(id), 
                null, // order by
                new Expression<Func<Trip, object>>[]
                    {
                        (t => t.Persons)
                    }) // include
                    .FirstOrDefault();

            PopulatePersons(trip.Persons.ToList());

            return View(trip);
        }

        //
        // POST: /Trip/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, Trip trip, List<Person> Persons)
        {
            try
            {
                Trip savedTrip = _repository.GetById<Trip>(trip.Id);
                if (ModelState.IsValid)
                {
                    UpdateModel(savedTrip);

                    if (trip.TransportType == null || trip.TransportType.Id != trip.TransportTypeId)
                    {
                        TransportType transportType = _repository.GetById<TransportType>(trip.TransportTypeId);
                        trip.TransportType = transportType;
                    }
                    if (trip.Trail == null || trip.Trail.Id != trip.TrailId)
                    {
                        Trail trail = _repository.GetById<Trail>(trip.TrailId);
                        trip.Trail = trail;
                    }

                    _repository.Update(savedTrip);
                    _repository.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.PossibleTransportTypes = _repository.List<TransportType>();
                    ViewBag.PossibleTrails = _repository.List<Trail>();
                    PopulatePersons();
                    return View();
                }
            }
            catch
            {
                ViewBag.PossibleTransportTypes = _repository.List<TransportType>();
                ViewBag.PossibleTrails = _repository.List<Trail>();
                PopulatePersons();
                return View();
            }
        }

        private void PopulatePersons(IList<Person> peopleToOmit = null)
        {
            if (peopleToOmit == null)
                _persons = new List<Person>(_repository.List<Person>());
            else
            {
                _persons = new List<Person>(_repository.List<Person>()).Except(peopleToOmit).ToList();
            }

            ViewData["persons"] = _persons;
        }

        [GridAction]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Trip trip = _repository.GetById<Trip>(id);
            _repository.Delete(trip);
            _repository.SaveChanges();
           return RedirectToAction("Index");
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

        [AcceptVerbs(HttpVerbs.Post)]
        public string AddPersonToTrip(string id, string personId, List<string> personIds)
        {
            if (string.IsNullOrEmpty(id))
                return "Invalid id. Please press back or enable JavaScript.";

            int pId = Convert.ToInt32(personId);
            int tripId = Convert.ToInt32(id);

            Person personToAdd = _repository.GetById<Person>(pId);

            Trip trip = _repository.Get<Trip>(t => t.Id.Equals(tripId),
                                              null, // order by
                                              new Expression<Func<Trip, object>>[]
                                                  {
                                                      (t => t.Persons)
                                                  }) // include
                .FirstOrDefault();

            if (trip == null) return "Trip not found";

            if (trip.Persons.Where(p => p.Id == pId).Count() == 0)
            {
                trip.Persons.Add(personToAdd);
                _repository.Update(trip);
                _repository.SaveChanges();
            }
            return string.Format("Successfully added {0}.", personToAdd.FirstName);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public string RemovePersonFromTrip(string id, string personId, List<string> personIds)
        {
            if (string.IsNullOrEmpty(id))
                return "Invalid id. Please press back or enable JavaScript."; //Add to cart logic            

            int pId = Convert.ToInt32(personId);
            int tripId = Convert.ToInt32(id);

            Person personToRemove = _repository.GetById<Person>(pId);

            Trip trip = _repository.Get<Trip>(t => t.Id.Equals(tripId),
                                              null, // order by
                                              new Expression<Func<Trip, object>>[]
                                                  {
                                                      (t => t.Persons)
                                                  }) // include
                .FirstOrDefault();

            if (trip == null) return "Trip not found";

            if (trip.Persons.Where(p => p.Id == pId).Count() > 0)
            {
                trip.Persons.Remove(personToRemove);
                _repository.Update(trip);
                _repository.SaveChanges();
            }
            
            return string.Format("Successfully removed {0}.", personToRemove.FirstName);
        }

    
    }
}
