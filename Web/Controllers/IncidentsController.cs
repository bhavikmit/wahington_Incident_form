using Microsoft.AspNetCore.Mvc;

using Repositories.Common;

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

        public ActionResult Index()
        {
            IncidentViewModel incidentViewModel = new();
            return View(incidentViewModel);
        }
    }
}