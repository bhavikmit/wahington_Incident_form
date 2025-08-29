using Microsoft.AspNetCore.Mvc;

using Repositories.Common;

using System.Threading.Tasks;

using ViewModels;
using ViewModels.Incident;

namespace Web.Controllers
{

    public class IncidentsController : Controller
    {
        private readonly IIncidentService _iIncidentService;
        public IncidentsController(IIncidentService incidentService)
        {
            _iIncidentService = incidentService;
        }

        public async Task<ActionResult> Index()
        {
            var incidentViewModel = await _iIncidentService.GetIncidentDropDown();
            //incidentViewModel.incidentGridViewModel = await _iIncidentService.GetIncidentList();
            return View(incidentViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveIncident([FromForm] IncidentViewModel incidentViewModel)
        {
            if (incidentViewModel == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            try
            {
                var incidentId = await _iIncidentService.SaveIncident(incidentViewModel);

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

        [HttpGet]
        public async Task<PartialViewResult> GetIncidentList()
        {
            var incidentViewModel = new IncidentViewModel();
            incidentViewModel.incidentGridViewModel = await _iIncidentService.GetIncidentList();
            return PartialView("_IncidentGrid", incidentViewModel ?? new IncidentViewModel());
        }
    }
}