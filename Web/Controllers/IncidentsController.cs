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
            return View(incidentViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> SaveIncident([FromForm] IncidentViewModel incidentViewModel)
        {
            return Ok(new { success = true });

            //var incidentViewModel = await _iIncidentService.GetIncidentDropDown();
            //return View(incidentViewModel);
        }
    }
}