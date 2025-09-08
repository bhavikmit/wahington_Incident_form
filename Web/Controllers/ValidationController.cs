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
        public async Task<ActionResult> Index()
        {
            ValidationWorkflowViewModel validationWorkflow = new();
            var pendingValidations = await _iIncidentValidationService.GetValidationPendingList();
            validationWorkflow.IVCount.PendingValidationCount = pendingValidations.Count;
            validationWorkflow.IVPendingList = pendingValidations;
            return View(validationWorkflow);
        }
    }
}
