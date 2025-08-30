using AutoMapper;

using Azure;

using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;

using DataLibrary;

using Enums;

using Helpers.Extensions;
using Helpers.File;

using Microsoft.AspNetCore.Http;
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
using System.Threading.Tasks;

using ViewModels;
using ViewModels.Incident;
using ViewModels.Shared;

namespace Repositories.Common
{
    public class IncidentService : IIncidentService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<IncidentService> _logger;
        private readonly IMapper _mapper;
        private readonly IFileHelper _fileHelper;

        public IncidentService(ApplicationDbContext db, ILogger<IncidentService> logger, IMapper mapper, IFileHelper fileHelper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _fileHelper = fileHelper;
        }

        public async Task<IncidentViewModel> GetIncidentDropDown()
        {
            try
            {
                IncidentViewModel incidentViewModel = new();

                var statusLegends = await _db.StatusLegends
                    .Where(it => !it.IsDeleted)
                    .OrderBy(it => it.Name)
                    .Select(it => new SelectListItem
                    {
                        Value = it.Id.ToString(),
                        Text = it.Name
                    })
                    .ToListAsync();

                var severityLevels = await _db.SeverityLevels
                    .Where(it => !it.IsDeleted)
                    .OrderBy(it => it.Name)
                    .Select(it => new SelectListItem
                    {
                        Value = it.Id.ToString(),
                        Text = !string.IsNullOrWhiteSpace(it.Description) ? it.Name + " (" + it.Description + ")" : it.Name
                    })
                    .ToListAsync();

                var relationships = await _db.Relationships
                   .Where(it => !it.IsDeleted)
                   .OrderBy(it => it.Name)
                   .Select(it => new SelectListItem
                   {
                       Value = it.Id.ToString(),
                       Text = it.Name
                   })
                   .ToListAsync();

                var assetIncidents = await _db.AssetIncidents
                  .Where(it => !it.IsDeleted)
                  .OrderBy(it => it.Name)
                  .Select(it => new SelectListItem
                  {
                      Value = it.Id.ToString(),
                      Text = it.Name
                  })
                  .ToListAsync();

                var eventTypes = await _db.EventTypes
                 .Where(it => !it.IsDeleted)
                 .OrderBy(it => it.Name)
                 .Select(it => new SelectListItem
                 {
                     Value = it.Id.ToString(),
                     Text = !string.IsNullOrWhiteSpace(it.Description) ? it.Name + " (" + it.Description + ")" : it.Name
                 })
                 .ToListAsync();


                incidentViewModel.severityLevels = severityLevels;
                incidentViewModel.statusLegends = statusLegends;
                incidentViewModel.incidentCellerInformation.Relationships = relationships;
                incidentViewModel.incidentiLocation.AssetsIncidentList = assetIncidents;
                incidentViewModel.incidentDetails.EventTypes = eventTypes;

                return incidentViewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetIncidentDropDown.");
                return new IncidentViewModel()!;
            }
        }

        public async Task<string> SaveIncident(IncidentViewModel viewModel)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                // Generate IncidentID once
                var totalIncidentCount = await _db.Incidents.IgnoreQueryFilters().CountAsync();
                var incidentId = $"INC-{(totalIncidentCount + 1):D4}";

                // Save file if available
                var imageUrl = viewModel.incidentSupportingInfoViewModel?.File != null
                    ? await SaveAttachments(viewModel.incidentSupportingInfoViewModel.File)
                    : null;

                if (viewModel.incidentSupportingInfoViewModel != null)
                    viewModel.incidentSupportingInfoViewModel.ImageUrl = imageUrl;

                // Map ViewModel → Entity
                var incident = new Incident
                {
                    IncidentID = incidentId,
                    StatusLegendId = (int)StatusLegendEnum.Submitted,
                    SeverityLevelId = viewModel.severityLevelId,
                    DescriptionIssue = viewModel.DescriptionIssue,

                    CallerAddress = viewModel.incidentCellerInformation?.CallerAddress,
                    CallerPhoneNumber = viewModel.incidentCellerInformation?.CallerPhoneNumber,
                    CallerName = viewModel.incidentCellerInformation?.CallerName,
                    CallTime = viewModel.incidentCellerInformation?.CallTime ?? DateTime.Now,
                    RelationshipId = viewModel.incidentCellerInformation?.RelationshipId,

                    EventTypeId = viewModel.incidentDetails?.EventTypeId ?? 0,

                    EvacuationRequiredId = viewModel.incidentEnvironmentalViewModel?.EvacuationRequiredID,
                    HissingPresentId = viewModel.incidentEnvironmentalViewModel?.HissingSoundPresentID,
                    VisibleDamagePresentId = viewModel.incidentEnvironmentalViewModel?.VisibleDamageID,
                    PeopleInjuredId = viewModel.incidentEnvironmentalViewModel?.PeopleInjuredID,
                    GasPresentId = viewModel.incidentEnvironmentalViewModel?.GasodorpresentID,

                    Landmark = viewModel.incidentiLocation?.Landmark,
                    LocationAddress = viewModel.incidentiLocation?.Address,
                    ServiceAccount = viewModel.incidentiLocation?.ServiceAccount,
                    AssetIds = viewModel.incidentiLocation?.AssetIDs,
                    ImageUrl = viewModel.incidentSupportingInfoViewModel?.ImageUrl,
                    SupportInfoNotes = viewModel.incidentSupportingInfoViewModel?.Notes
                };

                // Save
                await _db.Incidents.AddAsync(incident);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return incidentId;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error SaveIncident.");
                return string.Empty;
            }
        }

        public async Task<List<IncidentGridViewModel>> GetIncidentList(FilterRequest request)
        {

            List<IncidentGridViewModel> incidentGridViews = new();

            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var query = _db.Incidents
                             .Include(p => p.StatusLegend)
                             .Include(p => p.Relationship)
                             .Include(p => p.EventType)
                             .Include(p => p.SeverityLevel)
                             .AsQueryable();


                if (request != null)
                {
                    if (request.severityId > 0)
                    {
                        query = query.Where(p => p.SeverityLevelId == request.severityId);
                    }

                    if (request.statusId > 0)
                    {
                        query = query.Where(p => p.StatusLegendId == request.statusId);
                    }
                }
                var incidentsList = await query.ToListAsync();


                foreach (var item in incidentsList)
                {
                    incidentGridViews.Add(new IncidentGridViewModel()
                    {
                        CallDate = GetDate(Convert.ToString(item.CallTime)),
                        CallTime = GetTime(Convert.ToString(item.CallTime)),
                        AssetId = await GetAssets(item.AssetIds ?? string.Empty),
                        DescriptionIssue = item.DescriptionIssue ?? string.Empty,
                        EventType = item.EventType.Name,
                        EventTypeId = item.EventTypeId,
                        GasESIndicator = GetIndicator(item.GasPresentId),
                        Id = item.Id,
                        Intersection = item.Landmark ?? string.Empty,
                        LocationAddress = item.LocationAddress ?? string.Empty,
                        Severity = item.SeverityLevel.Name,
                        SeverityId = item.SeverityLevelId,
                        StatusLegend = item.StatusLegend.Name,
                        StatusLegendColor = item.StatusLegend.Color,
                        StatusLegendId = item.StatusLegendId
                    });
                }
                return incidentGridViews;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error GetIncidentList.");
                return new List<IncidentGridViewModel>();
            }
        }

        public async Task<string?> ChangeIncidentStatus(long incidentId, long statusId)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var incident = await _db.Incidents.FirstOrDefaultAsync(p => p.Id == incidentId);

                if (incident == null)
                {
                    await transaction.RollbackAsync();
                    return null; // or string.Empty if you want
                }

                incident.StatusLegendId = statusId;
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                // Assuming you want to return IncidentID (string) or Id as string
                return incident.IncidentID; // change to incident.Id.ToString() if needed
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error ChangeIncidentStatus.");
                return null; // or string.Empty
            }
        }

        #region private methods
        private bool TryParseCallTime(string callTime, out DateTime dateTime)
        {
            return DateTime.TryParse(callTime, out dateTime);
        }

        private string GetDate(string callTime)
        {
            if (TryParseCallTime(callTime, out var dt))
            {
                return dt.ToString("dd MMM, yyyy");  // Example: 29 Aug, 2025
            }
            return string.Empty;
        }

        private string GetTime(string callTime)
        {
            if (TryParseCallTime(callTime, out var dt))
            {
                return dt.ToString("HH:mm tt");      // Example: 02:53 PM
            }
            return string.Empty;
        }

        private string GetIndicator(long? value) =>
            value switch
            {
                1 => "Yes",
                0 => "No",
                2 => "Unknown",
                _ => string.Empty
            };

        private async Task<string> GetAssets(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                return string.Empty;

            var idArray = ids.Split(",", StringSplitOptions.RemoveEmptyEntries)
                             .Select(id => long.TryParse(id.Trim(), out var val) ? val : (long?)null)
                             .Where(val => val.HasValue)
                             .Select(val => val.Value)
                             .ToList();

            var assetNames = await _db.AssetIncidents
                                      .Where(a => idArray.Contains(a.Id))
                                      .Select(a => a.Name)
                                      .ToListAsync();

            return string.Join(",", assetNames);
        }
        private async Task<string> SaveAttachments(IFormFile file)
        {
            string imageUrl = string.Empty;
            if (file.Length > 0)
            {
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Storage", "uploads", "incidents");

                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Store relative URL path
                imageUrl = $"/Storage/uploads/incidents/{fileName}";

                _logger.LogInformation($"Saved attachment: {fileName} at {imageUrl}");
            }

            return imageUrl;
        }
        #endregion
    }
}
