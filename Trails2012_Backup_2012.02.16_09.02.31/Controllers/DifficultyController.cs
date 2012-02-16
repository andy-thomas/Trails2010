using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using Trails2012.DataAccess;
using Trails2012.Domain;

namespace Trails2012.Controllers
{
    public class DifficultyController : Controller
    {
        private readonly IRepository _repository;

        [ImportingConstructor]
        public DifficultyController(IRepository repository)
        {
            _repository = repository;
        }

        public ActionResult DifficultyGrid()
        {
            return PartialView("_SelectAjaxEditing", new List<Difficulty>(_repository.List<Difficulty>()));
        }

        [GridAction]
        public ActionResult SelectAjaxEditing()
        {
            List<Difficulty> difficulties = new List<Difficulty>(_repository.List<Difficulty>());
            var gridModel = new GridModel(difficulties);
            return PartialView(gridModel);
        }

        [HttpPost]
        [GridAction]
        public ActionResult EditAjaxEditing(Difficulty difficulty)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(difficulty);
                _repository.SaveChanges();
            }
            var difficulties = new List<Difficulty>(_repository.List<Difficulty>());
            return PartialView("_SelectAjaxEditing", new GridModel(difficulties));
        }

        [GridAction]
        [HttpPost]
        public ActionResult CreateAjaxEditing(Difficulty difficulty)
        {
            if (ModelState.IsValid)
            {
                _repository.Insert(difficulty);
                _repository.SaveChanges();
            }
            var difficulties = new List<Difficulty>(_repository.List<Difficulty>());
            return PartialView("_SelectAjaxEditing", new GridModel(difficulties));
        }

        [GridAction]
        [HttpPost]
        public ActionResult DeleteAjaxEditing(int id)
        {
            Difficulty difficulty = _repository.GetById<Difficulty>(id);
            _repository.Delete(difficulty);
            _repository.SaveChanges();
            var difficulties = new List<Difficulty>(_repository.List<Difficulty>());
            return PartialView("_SelectAjaxEditing", new GridModel(difficulties));
        }

    }
}
