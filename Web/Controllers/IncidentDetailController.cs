using Microsoft.AspNetCore.Mvc;

using Repositories.Common;

namespace Web.Controllers
{
    public class IncidentDetailController : Controller
    {
        private readonly IIncidentService _iIncidentService;

        public IncidentDetailController(IIncidentService incidentService)
        {
            _iIncidentService = incidentService;
        }
        public async Task<IActionResult> Index(long id)
        {
            var model = await _iIncidentService.GetIncidentDetailsById(id);
            model.ListIncidentLocationMapViewModel = await _iIncidentService.GetIncidentMapDetailsbyId(id);

            return View(model);
        }
    }
}
