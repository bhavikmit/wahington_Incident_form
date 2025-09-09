using Microsoft.AspNetCore.Mvc;

using Repositories.Common;
using Repositories.Services.ArcGis;
using Repositories.Services.ArcGis.Interface;

using System.Threading.Tasks;

using ViewModels;
using ViewModels.Incident;

namespace Web.Controllers
{

    public class IncidentsController : Controller
    {
        private readonly IIncidentService _iIncidentService;
        private readonly IArcGisGeocodingService _iArcGisGeocodingService;

        public IncidentsController(IIncidentService incidentService, IArcGisGeocodingService iArcGisGeocodingService)
        {
            _iIncidentService = incidentService;
            _iArcGisGeocodingService = iArcGisGeocodingService;
        }

        public async Task<ActionResult> Index()
        {
            var incidentViewModel = await _iIncidentService.GetIncidentDropDown();
            return View(incidentViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveIncident([FromForm] IncidentViewModel incidentViewModel)
        {
            if (incidentViewModel == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            try
            {
                var incidentId = string.Empty;
                if (incidentViewModel.Id > 0)
                {
                    incidentId = await _iIncidentService.UpdateIncident(incidentViewModel);
                }
                else
                {
                    incidentId = await _iIncidentService.SaveIncident(incidentViewModel);
                }
                if (string.IsNullOrWhiteSpace(incidentId))
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { success = false, message = "Failed to save incident." });

                var successMsg = $"Incident {incidentId} saved successfully!";

                return Ok(new { success = true, data = successMsg });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpPost]
        public async Task<PartialViewResult> GetIncidentList([FromBody] FilterRequest request)
        {
            var incidentViewModel = new IncidentViewModel
            {
                incidentGridViewModel = await _iIncidentService.GetIncidentList(request)
            };

            return PartialView("_IncidentGrid", incidentViewModel ?? new IncidentViewModel());
        }


        //[HttpPost]
        //public async Task<IActionResult> ChangeIncidentStatus([FromBody] ChangeStatusRequest request)
        //{
        //    if (request == null || request.IncidentId <= 0 || string.IsNullOrWhiteSpace(request.Status))
        //    {
        //        return BadRequest(new { success = false, message = "Invalid data." });
        //    }

        //    var result = await _iIncidentService.ChangeIncidentStatus(request.IncidentId, request.Status);

        //    if (string.IsNullOrEmpty(result))
        //    {
        //        return NotFound(new { success = false, message = "Incident not found." });
        //    }

        //    return Ok(new { success = true, data = $"Incident {result} status changed successfully." });
        //}

        [HttpGet]
        public async Task<IActionResult> AddIncident()
        {
            var incidentViewModel = await _iIncidentService.GetIncidentDropDown();
            return PartialView("_AddEditIncidentModal", incidentViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditIncident(int id)
        {
            var model = await _iIncidentService.GetById(id);
            return PartialView("_AddEditIncidentModal", model);
        }
        [HttpGet]
        public async Task<PartialViewResult> GetIncidentDetails(long id)
        {
            var model = await _iIncidentService.GetIncidentDetailsById(id);
            return PartialView("_IncidentAllDetails", model);
        }

        [HttpGet]
        public async Task<IActionResult> Suggest(string text)
        {
            var results = await _iArcGisGeocodingService.GetSuggestionsAsync(text);
            return Json(results);
        }

        [HttpGet]
        public async Task<IActionResult> Resolve(string magicKey)
        {
            var result = await _iArcGisGeocodingService.GetCoordinatesAsync(magicKey);
            if (result == null) return NotFound();
            return Json(result);
        }
    }
}