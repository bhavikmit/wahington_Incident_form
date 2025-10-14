using Microsoft.AspNetCore.Mvc;

using Repositories.Common;

using ViewModels.Incident;

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
            model.listIncidentMapChats = await _iIncidentService.GetIncidentMapChatChat(id);

            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> AddMapChat([FromBody] IncidentMapChatRequest request)
        {
            if (request == null)
            {
                return Json(new { success = false, message = "Invalid request." });
            }

            try
            {
                long id = await _iIncidentService.AddMapChat(request);
                if (id > 0)
                {
                    return Json(new { success = true, message = "Success" });
                }
                return Json(new { success = false, message = "Failed to delete location." });
            }
            catch (Exception ex)
            {
                // you already have _logger in controller? If not, use try/catch and return generic
                return Json(new { success = false, message = "Error delete location." });
            }
        }


    }
}
