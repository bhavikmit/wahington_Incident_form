using AutoMapper;

using Azure;

using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;

using DataLibrary;

using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;

using Enums;

using Helpers.Extensions;
using Helpers.File;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Models;
using Models.Common.Interfaces;

using Pagination;

using Repositories.Shared.UserInfoServices.Interface;

using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

using ViewModels;
using ViewModels.Incident;
using ViewModels.Shared;

namespace Repositories.Common
{
    public class IncidentValidationService : IIncidentValidationService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<IncidentValidationService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IncidentValidationService(ApplicationDbContext db, ILogger<IncidentValidationService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<IncidentValidationPendingViewModel>> GetValidationPendingList()
        {
            List<IncidentValidationPendingViewModel> incidentValidationPendings = new();
            try
            {
                var query = _db.Incidents.Where(p => !p.IsDeleted && p.StatusLegendId != (int)StatusLegendEnum.Validated)
                             .Include(p => p.SeverityLevel)
                             .AsQueryable();

                var incidentsList = await query.ToListAsync();


                foreach (var item in incidentsList)
                {
                    incidentValidationPendings.Add(new IncidentValidationPendingViewModel()
                    {
                        EventType = await GetEventTypes(item.EventTypeIds ?? string.Empty),
                        Id = item.Id,
                        Severity = item.SeverityLevel.Name,
                        SeverityColor = item.SeverityLevel.Color,
                        Description = item?.DescriptionIssue,
                        IncidentId = item.IncidentID,
                        IncidentLocation = item.LocationAddress,
                        IncidentDate = GetDate(Convert.ToString(item.CallTime))
                    });
                }
                return incidentValidationPendings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetValidationPendingList.");
                return new List<IncidentValidationPendingViewModel>();
            }
        }

        public async Task<List<RecentlyIncidentValidationViewModel>> GetRecentlyValidationList()
        {
            List<RecentlyIncidentValidationViewModel> incidentValidationPendings = new();
            try
            {
                var query = _db.Incidents.Where(p => !p.IsDeleted && p.StatusLegendId == (int)StatusLegendEnum.Validated
                             && p.UpdatedOn == DateTime.Today)
                             .Include(p => p.SeverityLevel)
                             .AsQueryable();

                var incidentsList = await query.ToListAsync();

                foreach (var item in incidentsList)
                {
                    incidentValidationPendings.Add(new RecentlyIncidentValidationViewModel()
                    {
                        EventType = await GetEventTypes(item.EventTypeIds ?? string.Empty),
                        Id = item.Id,
                        Status = item.StatusLegend.Name,
                        StatusColor = item.StatusLegend.Color,
                        IncidentId = item.IncidentID,
                        IncidentDate = GetDate(Convert.ToString(item.CallTime))
                    });
                }
                return incidentValidationPendings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetRecentlyValidationList.");
                return new List<RecentlyIncidentValidationViewModel>();
            }
        }

        public async Task<long> GetHighPriorityIncidentCount()
        {
            try
            {
                return await _db.Incidents
                    .Where(p => !p.IsDeleted
                                && p.StatusLegendId != (int)StatusLegendEnum.Validated
                                && p.SeverityLevel.Name == SeverityEnum.High.ToString())
                    .LongCountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetHighPriorityIncidentCount.");
                return 0;
            }
        }

        public async Task<IncidentValidationDetailViewModel> GetIncidentValidationDetail(long id)
        {
            try
            {
                var incident = await _db.Incidents
                    .Include(p => p.SeverityLevel)
                    .Include(p => p.StatusLegend)
                    .FirstOrDefaultAsync(p => !p.IsDeleted && p.Id == id);

                if (incident == null)
                {
                    return new IncidentValidationDetailViewModel();
                }

                // Run async calls in parallel
                var eventTypesTask = await GetEventTypes(incident.EventTypeIds ?? string.Empty);
                var assetsTask = await GetAssets(incident.AssetIds ?? string.Empty);


                return new IncidentValidationDetailViewModel
                {
                    Id = incident.Id,
                    CallerAddress = incident.CallerAddress,
                    CallerContact = incident.CallerPhoneNumber,
                    CallerDateTime = GetDate(incident.CallTime.ToString()),
                    CallerName = incident.CallerName,
                    EventType = eventTypesTask,
                    IncidentId = incident.IncidentID,
                    IncidentLocation = incident.LocationAddress,
                    NearestIntersection = incident.Landmark,
                    AffectedAssets = assetsTask,
                    Lat = incident.Lat,
                    Long = incident.Lng,
                    IncidentStatus = incident.StatusLegend?.Name,
                    IncidentStatusColor = incident.StatusLegend?.Color,
                    Severity = incident.SeverityLevel?.Name,
                    SeverityColor = incident.SeverityLevel?.Color,
                    DescriptionIssue = incident.DescriptionIssue,
                    EvacuationRequired = GetIndicator(incident.EvacuationRequiredId),
                    GasPresent = GetIndicator(incident.GasPresentId),
                    HissingPresent = GetIndicator(incident.HissingPresentId),
                    PeopleInjured = GetIndicator(incident.PeopleInjuredId),
                    VisibleDamagePresent = GetIndicator(incident.VisibleDamagePresentId),
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetIncidentValidationDetail.");
                return new IncidentValidationDetailViewModel();
            }
        }

        public async Task<IncidentValidationViewModel> GetIncidentValidationAlarm(long id)
        {
            try
            {
                var incidentTask = await _db.Incidents
                    .Include(p => p.SeverityLevel)
                    .FirstOrDefaultAsync(p => !p.IsDeleted && p.Id == id);

                //var severityLevelsTask = await _db.SeverityLevels
                //    .Where(it => !it.IsDeleted)
                //    .OrderBy(it => it.Name)
                //    .Select(it => new SelectListItem
                //    {
                //        Value = it.Id.ToString(),
                //        Text = !string.IsNullOrWhiteSpace(it.Description)
                //               ? it.Name + " - " + it.Description
                //               : it.Name
                //    })
                //    .ToListAsync();

                var severityLevelsTask = await _db.SeverityLevels
                                   .Where(it => !it.IsDeleted)
                                   .OrderBy(it => it.Name == "High" ? 1 :
                                                  it.Name == "Moderate" ? 2 :
                                                  it.Name == "Low" ? 3 : 4)
                                   .Select(it => new SelectListItem
                                   {
                                       Value = it.Id.ToString(),
                                       Text = !string.IsNullOrWhiteSpace(it.Description)
                                              ? it.Name + " (" + it.Description + ")"
                                              : it.Name
                                   })
                                   .ToListAsync();

                if (incidentTask == null)
                {
                    return new IncidentValidationViewModel { severityLevels = severityLevelsTask };
                }

                return new IncidentValidationViewModel
                {
                    Id = incidentTask.Id,
                    IncidentId = incidentTask.IncidentID,
                    IncidentLocation = incidentTask.LocationAddress,
                    severityLevels = severityLevelsTask,
                    severityLevel = incidentTask.SeverityLevel?.Name,
                    Lat = incidentTask.Lat,
                    Long = incidentTask.Lng
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetIncidentValidationAlarm.");
                return new IncidentValidationViewModel();
            }
        }

        public async Task<List<IncidentResponseTeamViewModel>> GetIncidentValidationResponseTeam()
        {
            List<IncidentResponseTeamViewModel> incidentResponseTeams = new();

            try
            {
                var responseTeams = await _db.IncidentTeams.Where(p => !p.IsDeleted).ToListAsync();

                foreach (var item in responseTeams)
                {
                    incidentResponseTeams.Add(new IncidentResponseTeamViewModel()
                    {
                        ReponseTeamId = item.Id,
                        Name = item.Name,
                        Contact = item.Contact,
                        Specializations = item.Specializations
                    });
                }

                return incidentResponseTeams;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetIncidentValidationResponseTeam.");
                return new List<IncidentResponseTeamViewModel>();
            }
        }

        public async Task<List<IncidentPolicyViewModel>> GetIncidentValidationPolicy()
        {
            List<IncidentPolicyViewModel> incidentPolicies = new();

            try
            {
                var assignTeams = await _db.AssignResponseTeams.Where(p => !p.IsDeleted).ToListAsync();

                var policies = await _db.Policies.Where(p => !p.IsDeleted).ToListAsync();

                foreach (var item in policies)
                {
                    incidentPolicies.Add(new IncidentPolicyViewModel()
                    {
                        PolicyId = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        assignTeams = assignTeams.Select(p => new SelectListItem()
                        {
                            Text = p.Name,
                            Value = p.Id.ToString()
                        }).ToList()
                    });
                }

                return incidentPolicies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetIncidentValidationPolicy.");
                return new List<IncidentPolicyViewModel>();
            }
        }

        public async Task<long> SavePolicy(PolicyModifyViewModel request)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                // Map ViewModel → Entity
                var policy = new Policy
                {
                    Name = request.Name,
                    Description = request.Description
                };

                // Save
                await _db.Policies.AddAsync(policy);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return policy.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error SavePolicy.");
                return 0;
            }
        }

        public async Task<List<SelectListItem>> GetTeamsList()
        {
            List<SelectListItem> assignResponseTeams = new();
            try
            {
                var assignResponses = await _db.IncidentTeams.Where(p => !p.IsDeleted)
                             .ToListAsync();
                assignResponseTeams = assignResponses.Select(p => new SelectListItem()
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                }).ToList();

                return assignResponseTeams;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetTeamsList.");
                return new List<SelectListItem>();
            }
        }

        public async Task<long> SaveIncidentValidation(IncidentSubmitViewModel request)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                // 1. Save main IncidentValidation
                var incidentValidation = new IncidentValidation
                {
                    IncidentId = request.Id,
                    IsMarkFalseAlarm = false,
                    ValidationNotes = request.ValidationNotes,
                    AssignResponseTeams = request.AssignResponseTeams,
                    ConfirmedSeverityLevelId = request.ConfirmedSeverityLevelId,
                    DiscoveryPerimeterId = request.DiscoveryPerimeterId,
                };

                await _db.IncidentValidations.AddAsync(incidentValidation);
                await _db.SaveChangesAsync();

                // 2. Policies
                var policies = request.listSubmitPolicyVM.Select(item => new IncidentValidationPolicy
                {
                    IncidentId = request.Id,
                    IncidentValidationId = incidentValidation.Id,
                    PolicyId = item.PolicyId,
                    Status = item.Status,
                    TeamIds = item.Teams != null && item.Teams.Any()
                                ? string.Join(",", item.Teams)
                                : string.Empty
                }).ToList();

                if (policies.Any())
                    await _db.IncidentValidationPolicies.AddRangeAsync(policies);

                // 3. Communication history
                var uploadRoot = Path.Combine(Directory.GetCurrentDirectory(), "Storage", "uploads", "Communication");
                if (!Directory.Exists(uploadRoot))
                    Directory.CreateDirectory(uploadRoot);

                var communications = request.listSubmitCommunicationVM.Select(item =>
                {
                    var fileList = MoveFilesToPermanentStorage(item.FileMeta, uploadRoot);

                    return new IncidentValidationCommunicationHistory
                    {
                        IncidentId = request.Id,
                        IncidentValidationId = incidentValidation.Id,
                        ImageUrl = string.Join(",", fileList),
                        MessageType = item.MessageType,
                        Message = item.Message,
                        RecipientsIds = item.RecipientsIds,
                        TimeStamp = item.TimeStamp,
                        UserName = item.UserName,
                    };
                }).ToList();

                if (communications.Any())
                    await _db.IncidentValidationCommunicationHistories.AddRangeAsync(communications);

                // 4. Update Incident record
                var incident = await _db.Incidents.FirstOrDefaultAsync(p => p.Id == request.Id);
                if (incident != null)
                {
                    var userId = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var userIdParsed = !string.IsNullOrEmpty(userId) ? long.Parse(userId) : 0;

                    // ⚠️ Replace with real logged-in user
                    var statusLegend = await _db.StatusLegends.FirstOrDefaultAsync(x => x.Name == StatusLegendEnum.Validated.ToString());

                    incident.StatusLegendId = statusLegend?.Id ?? (int)StatusLegendEnum.Validated;
                    incident.UpdatedOn = DateTime.Now;
                    incident.UpdatedBy = userIdParsed;
                }

                // 5. Save everything in one go
                await _db.SaveChangesAsync();

                await transaction.CommitAsync();
                return incidentValidation.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error SaveIncidentValidation.");
                return 0;
            }
        }

        #region private event
        /// <summary>
        /// Moves files from temp folder to permanent storage.
        /// Returns list of relative paths.
        /// </summary>
        private static List<string> MoveFilesToPermanentStorage(IEnumerable<FileMeta> fileMeta, string uploadRoot)
        {
            var fileList = new List<string>();

            foreach (var file in fileMeta)
            {
                if (string.IsNullOrWhiteSpace(file.TempPath))
                    continue;

                var destinationPath = Path.Combine(uploadRoot, file.FileName);
                var relativePath = $"/Storage/uploads/Communication/{file.FileName}";

                if (!File.Exists(destinationPath))
                {
                    File.Move(file.TempPath, destinationPath);
                }

                fileList.Add(relativePath);
            }

            return fileList;
        }
        private async Task<string> GetEventTypes(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                return string.Empty;

            var idArray = ids.Split(",", StringSplitOptions.RemoveEmptyEntries)
                             .Select(id => long.TryParse(id.Trim(), out var val) ? val : (long?)null)
                             .Where(val => val.HasValue)
                             .Select(val => val.Value)
                             .ToList();

            var eventTypes = await _db.EventTypes
                                      .Where(a => idArray.Contains(a.Id))
                                      .Select(a => a.Name)
                                      .ToListAsync();

            return string.Join(",", eventTypes);
        }
        private string GetDate(string callTime)
        {
            if (TryParseCallTime(callTime, out var dt))
            {
                return dt.ToString("MMM dd, yyyy hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);
            }
            return string.Empty;
        }
        private bool TryParseCallTime(string callTime, out DateTime dateTime)
        {
            return DateTime.TryParse(callTime, out dateTime);
        }
        private async Task<List<string>> GetAssets(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                return new List<string>();

            var idArray = ids.Split(",", StringSplitOptions.RemoveEmptyEntries)
                             .Select(id => long.TryParse(id.Trim(), out var val) ? val : (long?)null)
                             .Where(val => val.HasValue)
                             .Select(val => val.Value)
                             .ToList();

            var assetNames = await _db.AssetIncidents
                                      .Where(a => idArray.Contains(a.Id))
                                      .Select(a => a.Name)
                                      .ToListAsync();

            return assetNames;
        }
        private string GetIndicator(long? value) =>
           value switch
           {
               1 => "Yes",
               0 => "No",
               2 => "N/A",
               _ => string.Empty
           };
        #endregion
    }
}
