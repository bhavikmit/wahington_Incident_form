using Microsoft.AspNetCore.Mvc;

using Repositories.Common;

using ViewModels.Incident;

namespace Web.Controllers
{
    public class ValidationController : Controller
    {
        private readonly IncidentValidationService _iIncidentValidationService;
        public ValidationController(IncidentValidationService iIncidentValidationService)
        {
            _iIncidentValidationService = iIncidentValidationService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(long id)
        {
            ValidationWorkflowViewModel validationWorkflow = new();
            validationWorkflow.Id = id;
            return View(validationWorkflow);
        }

        [HttpGet]
        public async Task<IActionResult> GetValidationsList()
        {
            ValidationWorkflowViewModel validationWorkflow = new();

            var pendingValidations = await _iIncidentValidationService.GetValidationPendingList();
            validationWorkflow.IVCount.PendingValidationCount = pendingValidations.Count;
            validationWorkflow.IVPendingList = pendingValidations;

            var recentlyAddedValidations = await _iIncidentValidationService.GetRecentlyValidationList();
            validationWorkflow.IVCount.TodayValidationCount = recentlyAddedValidations.Count;
            validationWorkflow.IVRecentlyList = recentlyAddedValidations;

            validationWorkflow.IVCount.HighSeverityCount = await _iIncidentValidationService.GetHighPriorityIncidentCount();

            return PartialView("_IncidentValidationDashboard", validationWorkflow);
        }

        [HttpGet]
        public async Task<IActionResult> GetValidationsDetail(long id)
        {
            ValidationWorkflowViewModel validationWorkflow = new();

            var incidentValidationDtl = await _iIncidentValidationService.GetIncidentValidationDetail(id);
            validationWorkflow.IVDetails = incidentValidationDtl;

            var incidentValidationAlarm = await _iIncidentValidationService.GetIncidentValidationAlarm(id);
            validationWorkflow.IVValidation = incidentValidationAlarm;


            var incidentResponseTeams = await _iIncidentValidationService.GetIncidentValidationResponseTeam();
            validationWorkflow.IVResponseTeamList = incidentResponseTeams;

            return PartialView("_IncidentValidationDetail", validationWorkflow);
        }
    }
}
