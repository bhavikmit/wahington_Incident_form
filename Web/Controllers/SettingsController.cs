using Microsoft.AspNetCore.Mvc;

using Repositories.Common;

using ViewModels;
using ViewModels.Incident;

namespace Web.Controllers
{
    public class SettingsController : Controller
    {
        IRelationshipService<RelationshipModifyViewModel, RelationshipModifyViewModel, RelationshipDetailViewModel> _iRelationshipService;
        public SettingsController(IRelationshipService<RelationshipModifyViewModel, RelationshipModifyViewModel, RelationshipDetailViewModel> iRelationshipService)
        {
            _iRelationshipService = iRelationshipService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRelationships()
        {
            var model = await _iRelationshipService.GetAllRelationships();
            return PartialView("~/Views/Settings/Source/_ListSource.cshtml", model);
        }
        
        [HttpGet]
        public async Task<IActionResult> AddRelationships()
        {
            var model = new RelationshipModifyViewModel();
            return PartialView("~/Views/Settings/Source/_AddSource.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetRelationshipById(long id)
        {
            var model = await _iRelationshipService.GetRelationById(id);
            return PartialView("~/Views/Settings/Source/_AddSource.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveRelation([FromForm] RelationshipModifyViewModel relationship)
        {
            if (relationship == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            try
            {
                long Id = 0;
                if (relationship.Id > 0)
                {
                    Id = await _iRelationshipService.UpdateRelation(relationship);
                }
                else
                {
                    Id = await _iRelationshipService.SaveRelation(relationship);
                }
                if (Id == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { success = false, message = "Failed to save relation." });

                var successMsg = $"Source saved successfully!";

                return Ok(new { success = true, data = successMsg });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRelationshipById(long id)
        {
            if (id == 0)
                return BadRequest(new { success = false, message = "Invalid request data." });

            try
            {
                long Id = 0;
                if (id > 0)
                {
                    Id = await _iRelationshipService.DeleteRelation(id);
                }
                if (Id == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { success = false, message = "Failed to delete relation." });

                var successMsg = $"Source deleted successfully!";

                return Ok(new { success = true, data = successMsg });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurred." });
            }
        }

    }
}
