using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Web;
using System.Web.Helpers;
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
            ViewBag.IsReadOnly = true;
            return View(_repository.GetById<Trail>(id));
        }

        //
        // GET: /Trail/Create

        public ActionResult Create()
        {
            Trail newTrail = new Trail();
			ViewBag.PossibleLocations = _repository.List<Location>();
            ViewBag.PossibleTrailTypes = _repository.List<TrailType>();
            ViewBag.PossibleDifficulties = _repository.List<Difficulty>();
            return View(newTrail);
        } 

        //
        // POST: /Trail/Create

        // Andy - set ValidateInput(false) so that the Notes editor can set formatting tags without causing the 
        // "A potentially dangerous Request.Form value was detected from the client" error.
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Trail trail, HttpPostedFileBase file)
        {
            if (ModelState.IsValid) 
            {

                BinaryReader binaryReader = new BinaryReader(file.InputStream); 
                byte[] byteArray = binaryReader.ReadBytes(file.ContentLength);
                trail.Image = byteArray;

                // see Comment 1
                if (trail.DifficultyId.HasValue)
                {
                    Difficulty difficulty = _repository.GetById<Difficulty>(trail.DifficultyId.Value);
                    trail.Difficulty = difficulty;
                }
                Location location = _repository.GetById<Location>(trail.LocationId);
                trail.Location = location;
                TrailType trailType = _repository.GetById<TrailType>(trail.TrailTypeId);
                trail.TrailType = trailType;


                _repository.Insert(trail);
                _repository.SaveChanges();
                return RedirectToAction("Index");
            }

            //=================================
            var image = WebImage.GetImageFromRequest();

            if (image != null)
            {
                if (image.Width > 500)
                {
                    image.Resize(500, ((500 * image.Height) / image.Width));
                }
             //newImage.MimeType = image.ContentType;
           //     file.ContentType
 

                var binaryReader = new BinaryReader(file.InputStream);
                //newImage.Data = 
                byte[] byteArray =  binaryReader.ReadBytes(file.ContentLength);
                binaryReader.Close();
 
                //var filename = Path.GetFileName(image.FileName);
                //image.Save(Path.Combine("../Uploads/Images", filename));
                //filename = Path.Combine("~/Uploads/Images", filename);
                //string ImageUrl = Url.Content(filename);
                //string ImageAltText = image.FileName.Substring(0, image.FileName.Length - 4);

            }
            //=================================

            ViewBag.PossibleLocations = _repository.List<Location>();
            ViewBag.PossibleTrailTypes = _repository.List<TrailType>();
            ViewBag.PossibleDifficulties = _repository.List<Difficulty>();
            return View();
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
        // Andy - set ValidateInput(false) so that the Notes editor can set formatting tags without causing the 
        // "A potentially dangerous Request.Form value was detected from the client" error.
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Trail trail, HttpPostedFileBase file)
        {
            // Get the trail data back from the database and only then populate it from the model, 
            // because the form does not contain every piece of information about the entity;
            // specifically, it is missing the Image
            Trail savedTrail = _repository.GetById<Trail>(trail.Id);
            if (ModelState.IsValid)
            {
                UpdateModel(savedTrail);

                if (file != null)
                {
                    BinaryReader binaryReader = new BinaryReader(file.InputStream);
                    byte[] byteArray = binaryReader.ReadBytes(file.ContentLength);
                    savedTrail.Image = byteArray;
                }

                // see Comment 1
                if (trail.DifficultyId.HasValue)
                {
                    Difficulty difficulty = _repository.GetById<Difficulty>(trail.DifficultyId.Value);
                    trail.Difficulty = difficulty;
                }
                else
                    trail.Difficulty = null;
                if (trail.Location == null || trail.Location.Id != trail.LocationId)
                {
                    Location location = _repository.GetById<Location>(trail.LocationId);
                    trail.Location = location;
                }
                if (trail.TrailType == null || trail.TrailType.Id != trail.TrailTypeId)
                {
                    TrailType trailType = _repository.GetById<TrailType>(trail.TrailTypeId);
                    trail.TrailType = trailType;
                }


                _repository.Update(savedTrail);
                _repository.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PossibleLocations = _repository.List<Location>();
            ViewBag.PossibleTrailTypes = _repository.List<TrailType>();
            ViewBag.PossibleDifficulties = _repository.List<Difficulty>();
            return View();
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

        public ActionResult ShowImage(int trailId)
        {
            Trail trail = _repository.GetById<Trail>(trailId);
            if (trail == null || trail.Image == null) return null;
            return File(trail.Image, "image/jpg");
        }


    }
}

// _Comment 1_
// Andy - The foreign keys (LocationId, etc) have been updated from the form.
// Assign the objects - this step is only necessary if using the current NHib reprository.
// (The current EF repository is mapped to work with the foreign keys such as LocationId directly, 
// but NHib works through the related objects)