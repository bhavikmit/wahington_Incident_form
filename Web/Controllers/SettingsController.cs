using Microsoft.AspNetCore.Mvc;

using Repositories.Common;

using ViewModels;
using ViewModels.Incident;

namespace Web.Controllers
{
    public class SettingsController : Controller
    {
        #region Init Service
        private readonly IRelationshipService<RelationshipModifyViewModel, RelationshipModifyViewModel, RelationshipDetailViewModel> _iRelationshipService;
        private readonly IEventTypeService<EventTypeModifyViewModel, EventTypeModifyViewModel, EventTypeDetailViewModel> _iEventTypeService;
        private readonly ISeverityLevelService<SeverityLevelModifyViewModel, SeverityLevelModifyViewModel, SeverityLevelDetailViewModel> _iSeverityLevelService;
        private readonly IStatusLegendService<StatusLegendModifyViewModel, StatusLegendModifyViewModel, StatusLegendDetailViewModel> _iStatusLegendService;
        private readonly IAssetIdService<AssetIdModifyViewModel, AssetIdModifyViewModel, AssetIdDetailViewModel> _iAssetIdService;
        private readonly IAssetTypeService<AssetTypeModifyViewModel, AssetTypeModifyViewModel, AssetTypeDetailViewModel> _iAssetTypeService;
        private readonly IIncidentTeamService<IncidentTeamModifyViewModel, IncidentTeamModifyViewModel, IncidentTeamDetailViewModel> _iIncidentTeamService;
        private readonly IPolicyService<PolicyModifyViewModel, PolicyModifyViewModel, PolicyDetailViewModel> _iPolicyService;
        #endregion

        #region Ctor
        public SettingsController(IRelationshipService<RelationshipModifyViewModel, RelationshipModifyViewModel, RelationshipDetailViewModel> iRelationshipService, IEventTypeService<EventTypeModifyViewModel, EventTypeModifyViewModel, EventTypeDetailViewModel> iEventTypeService, ISeverityLevelService<SeverityLevelModifyViewModel, SeverityLevelModifyViewModel, SeverityLevelDetailViewModel> iSeverityLevelService, IStatusLegendService<StatusLegendModifyViewModel, StatusLegendModifyViewModel, StatusLegendDetailViewModel> iStatusLegendService, IAssetIdService<AssetIdModifyViewModel, AssetIdModifyViewModel, AssetIdDetailViewModel> iAssetIdService,
            IAssetTypeService<AssetTypeModifyViewModel, AssetTypeModifyViewModel, AssetTypeDetailViewModel> iAssetTypeService, IIncidentTeamService<IncidentTeamModifyViewModel, IncidentTeamModifyViewModel, IncidentTeamDetailViewModel> iIncidentTeamService, IPolicyService<PolicyModifyViewModel, PolicyModifyViewModel, PolicyDetailViewModel> iPolicyService)
        {
            _iRelationshipService = iRelationshipService;
            _iEventTypeService = iEventTypeService;
            _iSeverityLevelService = iSeverityLevelService;
            _iStatusLegendService = iStatusLegendService;
            _iAssetIdService = iAssetIdService;
            _iAssetTypeService = iAssetTypeService;
            _iIncidentTeamService = iIncidentTeamService;
            _iPolicyService = iPolicyService;
        }
        #endregion

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

        #region StatusLegend

        [HttpGet]
        public async Task<IActionResult> GetAllStatusLegend()
        {
            var model = await _iStatusLegendService.GetAllStatusLegends();
            return PartialView("~/Views/Settings/StatusLegend/_ListStatusLegend.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> AddStatusLegend()
        {
            var model = new StatusLegendModifyViewModel();
            return PartialView("~/Views/Settings/StatusLegend/_AddStatusLegend.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetStatusLegendById(long id)
        {
            var model = await _iStatusLegendService.GetStatusLegendById(id);
            return PartialView("~/Views/Settings/StatusLegend/_AddStatusLegend.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveStatusLegend([FromForm] StatusLegendModifyViewModel statusLegend)
        {
            if (statusLegend == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            try
            {
                long Id = 0;
                if (statusLegend.Id > 0)
                {
                    Id = await _iStatusLegendService.UpdateStatusLegend(statusLegend);
                }
                else
                {
                    Id = await _iStatusLegendService.SaveStatusLegend(statusLegend);
                }
                if (Id == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { success = false, message = "Failed to save status legend." });

                var successMsg = $"Status legend saved successfully!";

                return Ok(new { success = true, data = successMsg });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteStatusLegendById(long id)
        {
            if (id == 0)
                return BadRequest(new { success = false, message = "Invalid request data." });

            try
            {
                long Id = 0;
                if (id > 0)
                {
                    Id = await _iStatusLegendService.DeleteStatusLegend(id);
                }
                if (Id == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { success = false, message = "Failed to delete status legend." });

                var successMsg = $"Status legend deleted successfully!";

                return Ok(new { success = true, data = successMsg });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurred." });
            }
        }
        #endregion

        #region AssetIds
        [HttpGet]
        public async Task<IActionResult> GetAllAssetIds()
        {
            var model = await _iAssetIdService.GetAllAssetIds();
            return PartialView("~/Views/Settings/AssetId/_ListAssetId.cshtml", model);
        }
        // inside SettingsController

        [HttpGet]
        public async Task<IActionResult> AddAssetId()
        {
            var model = new AssetIdModifyViewModel();
            return PartialView("~/Views/Settings/AssetId/_AddAssetId.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAssetIdById(long id)
        {
            var model = await _iAssetIdService.GetAssetIdById(id);
            return PartialView("~/Views/Settings/AssetId/_AddAssetId.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAssetId([FromForm] AssetIdModifyViewModel asset)
        {
            if (asset == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            try
            {
                long id = 0;
                if (asset.Id > 0)
                    id = await _iAssetIdService.UpdateAssetId(asset);
                else
                    id = await _iAssetIdService.SaveAssetId(asset);

                if (id == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { success = false, message = "Failed to save asset." });

                var successMsg = "Asset saved successfully!";
                return Ok(new { success = true, data = successMsg });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAssetIdById(long id)
        {
            if (id == 0)
                return BadRequest(new { success = false, message = "Invalid request data." });

            try
            {
                var deletedId = await _iAssetIdService.DeleteAssetId(id);

                if (deletedId == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { success = false, message = "Failed to delete asset." });

                var successMsg = "Asset deleted successfully!";
                return Ok(new { success = true, data = successMsg });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurred." });
            }
        }
        #endregion

        #region AssetTypes
        [HttpGet]
        public async Task<IActionResult> GetAllAssetTypes()
        {
            var model = await _iAssetTypeService.GetAllAssetTypes();
            return PartialView("~/Views/Settings/AssetType/_ListAssetType.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> AddAssetType()
        {
            var model = new AssetTypeModifyViewModel();
            return PartialView("~/Views/Settings/AssetType/_AddAssetType.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAssetTypeById(long id)
        {
            var model = await _iAssetTypeService.GetAssetTypeById(id);
            return PartialView("~/Views/Settings/AssetType/_AddAssetType.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAssetType([FromForm] AssetTypeModifyViewModel assetType)
        {
            if (assetType == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            try
            {
                long id = 0;
                if (assetType.Id > 0)
                    id = await _iAssetTypeService.UpdateAssetType(assetType);
                else
                    id = await _iAssetTypeService.SaveAssetType(assetType);

                if (id == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { success = false, message = "Failed to save asset type." });

                return Ok(new { success = true, data = "Asset type saved successfully!" });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAssetTypeById(long id)
        {
            if (id == 0)
                return BadRequest(new { success = false, message = "Invalid request data." });

            try
            {
                var deletedId = await _iAssetTypeService.DeleteAssetType(id);

                if (deletedId == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { success = false, message = "Failed to delete asset type." });

                return Ok(new { success = true, data = "Asset type deleted successfully!" });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurred." });
            }
        }
        #endregion
        #region IncidentTeams
        [HttpGet]
        public async Task<IActionResult> GetAllIncidentTeams()
        {
            var model = await _iIncidentTeamService.GetAllIncidentTeams();
            return PartialView("~/Views/Settings/IncidentTeam/_ListIncidentTeam.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> AddIncidentTeam()
        {
            var model = new IncidentTeamModifyViewModel();
            return PartialView("~/Views/Settings/IncidentTeam/_AddIncidentTeam.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetIncidentTeamById(long id)
        {
            var model = await _iIncidentTeamService.GetIncidentTeamById(id);
            return PartialView("~/Views/Settings/IncidentTeam/_AddIncidentTeam.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveIncidentTeam([FromForm] IncidentTeamModifyViewModel incidentTeam)
        {
            if (incidentTeam == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            try
            {
                long id = 0;
                if (incidentTeam.Id > 0)
                    id = await _iIncidentTeamService.UpdateIncidentTeam(incidentTeam);
                else
                    id = await _iIncidentTeamService.SaveIncidentTeam(incidentTeam);

                if (id == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { success = false, message = "Failed to save incident team." });

                var successMsg = "Incident team saved successfully!";
                return Ok(new { success = true, data = successMsg });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteIncidentTeamById(long id)
        {
            if (id == 0)
                return BadRequest(new { success = false, message = "Invalid request data." });

            try
            {
                var deletedId = await _iIncidentTeamService.DeleteIncidentTeam(id);

                if (deletedId == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { success = false, message = "Failed to delete incident team." });

                var successMsg = "Incident team deleted successfully!";
                return Ok(new { success = true, data = successMsg });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurred." });
            }
        }
        #endregion
        #region Policy
        [HttpGet]
        public async Task<IActionResult> GetAllPolicies()
        {
            var model = await _iPolicyService.GetAllPolicies();
            return PartialView("~/Views/Settings/Policy/_ListPolicy.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> AddPolicy()
        {
            var model = new PolicyModifyViewModel();
            return PartialView("~/Views/Settings/Policy/_AddPolicy.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetPolicyById(long id)
        {
            var model = await _iPolicyService.GetPolicyById(id);
            return PartialView("~/Views/Settings/Policy/_AddPolicy.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> SavePolicy([FromForm] PolicyModifyViewModel policy)
        {
            if (policy == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            try
            {
                long Id = 0;
                if (policy.Id > 0)
                {
                    Id = await _iPolicyService.UpdatePolicy(policy);
                }
                else
                {
                    Id = await _iPolicyService.SavePolicy(policy);
                }

                if (Id == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { success = false, message = "Failed to save policy." });

                var successMsg = $"Policy saved successfully!";

                return Ok(new { success = true, data = successMsg });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeletePolicyById(long id)
        {
            if (id == 0)
                return BadRequest(new { success = false, message = "Invalid request data." });

            try
            {
                long Id = 0;
                if (id > 0)
                {
                    Id = await _iPolicyService.DeletePolicy(id);
                }

                if (Id == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { success = false, message = "Failed to delete policy." });

                var successMsg = $"Policy deleted successfully!";

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

