using Microsoft.AspNetCore.Mvc;

using Repositories.Common;

using ViewModels;
using ViewModels.Incident;

namespace Web.Controllers
{
    public class SettingsController : Controller
    {
        IRelationshipService<RelationshipModifyViewModel, RelationshipModifyViewModel, RelationshipDetailViewModel> _iRelationshipService;
        IEventTypeService<EventTypeModifyViewModel, EventTypeModifyViewModel, EventTypeDetailViewModel> _iEventTypeService;
        ISeverityLevelService<SeverityLevelModifyViewModel, SeverityLevelModifyViewModel, SeverityLevelDetailViewModel> _iSeverityLevelService;
        public SettingsController(IRelationshipService<RelationshipModifyViewModel, RelationshipModifyViewModel, RelationshipDetailViewModel> iRelationshipService, IEventTypeService<EventTypeModifyViewModel, EventTypeModifyViewModel, EventTypeDetailViewModel> iEventTypeService, ISeverityLevelService<SeverityLevelModifyViewModel, SeverityLevelModifyViewModel, SeverityLevelDetailViewModel> iSeverityLevelService)
        {
            _iRelationshipService = iRelationshipService;
            _iEventTypeService = iEventTypeService;
            _iSeverityLevelService = iSeverityLevelService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Source
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
        #endregion

        #region EventType
        [HttpGet]
        public async Task<IActionResult> GetAllEventTypes()
        {
            var model = await _iEventTypeService.GetAllEventTypes();
            return PartialView("~/Views/Settings/EventType/_ListEventType.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEventType()
        {
            var model = new EventTypeModifyViewModel();
            return PartialView("~/Views/Settings/EventType/_AddEventType.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetEventTypeById(long id)
        {
            var model = await _iEventTypeService.GetEventTypeById(id);
            return PartialView("~/Views/Settings/EventType/_AddEventType.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEventType([FromForm] EventTypeModifyViewModel eventType)
        {
            if (eventType == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            try
            {
                long Id = 0;
                if (eventType.Id > 0)
                {
                    Id = await _iEventTypeService.UpdateEventType(eventType);
                }
                else
                {
                    Id = await _iEventTypeService.SaveEventType(eventType);
                }
                if (Id == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { success = false, message = "Failed to save event type." });

                var successMsg = $"Event type saved successfully!";

                return Ok(new { success = true, data = successMsg });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteEventTypeById(long id)
        {
            if (id == 0)
                return BadRequest(new { success = false, message = "Invalid request data." });

            try
            {
                long Id = 0;
                if (id > 0)
                {
                    Id = await _iEventTypeService.DeleteEventType(id);
                }
                if (Id == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { success = false, message = "Failed to delete relation." });

                var successMsg = $"Event Type deleted successfully!";

                return Ok(new { success = true, data = successMsg });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurred." });
            }
        }
        #endregion

        #region SeverityLevel
        [HttpGet]
        public async Task<IActionResult> GetAllSeverity()
        {
            var model = await _iSeverityLevelService.GetAllSeverityLevels();
            return PartialView("~/Views/Settings/SeverityLevel/_ListSeverityLevel.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> AddSeverity()
        {
            var model = new SeverityLevelModifyViewModel();
            return PartialView("~/Views/Settings/SeverityLevel/_AddSeverityLevel.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetSeverityById(long id)
        {
            var model = await _iSeverityLevelService.GetSeverityLevelById(id);
            return PartialView("~/Views/Settings/SeverityLevel/_AddSeverityLevel.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveSeverity([FromForm] SeverityLevelModifyViewModel severityLevel)
        {
            if (severityLevel == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            try
            {
                long Id = 0;
                if (severityLevel.Id > 0)
                {
                    Id = await _iSeverityLevelService.UpdateSeverityLevel(severityLevel);
                }
                else
                {
                    Id = await _iSeverityLevelService.SaveSeverityLevel(severityLevel);
                }
                if (Id == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { success = false, message = "Failed to save severity level." });

                var successMsg = $"Severity level saved successfully!";

                return Ok(new { success = true, data = successMsg });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteSeverityById(long id)
        {
            if (id == 0)
                return BadRequest(new { success = false, message = "Invalid request data." });

            try
            {
                long Id = 0;
                if (id > 0)
                {
                    Id = await _iSeverityLevelService.DeleteSeverityLevel(id);
                }
                if (Id == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { success = false, message = "Failed to delete severity level." });

                var successMsg = $"Severity level deleted successfully!";

                return Ok(new { success = true, data = successMsg });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurred." });
            }
        }
        #endregion
    }
}
