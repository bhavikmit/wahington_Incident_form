using Microsoft.AspNetCore.Mvc;

using Models;

using Newtonsoft.Json;

using Repositories.Common;

using ViewModels;
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

            var incidentPolicy = await _iIncidentValidationService.GetIncidentValidationPolicy();
            validationWorkflow.IVPolicyList = incidentPolicy;

            return PartialView("_IncidentValidationDetail", validationWorkflow);
        }

        [HttpPost]
        public async Task<JsonResult> SavePolicy([FromBody] PolicyModifyViewModel request)
        {
            long policyId = await _iIncidentValidationService.SavePolicy(request);
            var teamsList = await _iIncidentValidationService.GetTeamsList();
            request.Id = policyId;

            return new JsonResult(new
            {
                Success = true,
                AssignTeams = teamsList,
                Request = request,
                Message = "Policy saved successfully"
            });
        }

        [HttpPost]
        public async Task<IActionResult> SaveIncidentValidation([FromForm] IncidentSubmitViewModel request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            try
            {

                foreach (var comm in request.listSubmitCommunicationVM)
                {
                    if (comm.Files != null && comm.Files.Count > 0)
                    {
                        foreach (var file in comm.Files)
                        {
                            var filePath = Path.Combine("Uploads", file.FileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(request.listPolicyVM))
                    request.listSubmitPolicyVM = JsonConvert.DeserializeObject<List<IncidentSubmitPolicyViewModel>>(request.listPolicyVM);

                // Deserialize metadata
                if (!string.IsNullOrEmpty(request.listCommunicationVM))
                {
                    request.listSubmitCommunicationVM =
                        JsonConvert.DeserializeObject<List<IncidentSubmitCommunicationViewModel>>(request.listCommunicationVM);
                }

                // Files will be bound automatically to request.listSubmitCommunicationVM[i].Files
                foreach (var comm in request.listSubmitCommunicationVM)
                {
                    if (comm.Files != null)
                    {
                        foreach (var file in comm.Files)
                        {
                            // process file (save, etc.)
                        }
                    }
                }

                var successMsg = $"Incident validation saved successfully!";

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
