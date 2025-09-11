using AutoMapper;

using Azure;

using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;

using DataLibrary;

using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;

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
using System.Text.Json;
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

        public IncidentService(ApplicationDbContext db, ILogger<IncidentService> logger)
        {
            _db = db;
            _logger = logger;
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
                        Text = it.Name,
                        Group = new SelectListGroup()
                        {
                            Name = it.Color
                        }
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
                var latLong = await GetLatLngFromAddress(viewModel.incidentiLocation.Address);

                // Generate IncidentID once
                var totalIncidentCount = await _db.Incidents.IgnoreQueryFilters().CountAsync();
                var incidentId = $"INC-{(totalIncidentCount + 1):D4}";

                // Save file if available
                var imageUrl = viewModel.incidentSupportingInfoViewModel?.File != null && viewModel.incidentSupportingInfoViewModel?.File.Count > 0
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

                    EventTypeIds = viewModel.incidentDetails?.EventTypeIds,
                    IsOtherEvent = viewModel.incidentDetails.IsOtherEvent,
                    OtherEventDetail = viewModel.incidentDetails?.OtherEventDetail,

                    EvacuationRequiredId = viewModel.incidentEnvironmentalViewModel?.EvacuationRequiredID,
                    HissingPresentId = viewModel.incidentEnvironmentalViewModel?.HissingSoundPresentID,
                    VisibleDamagePresentId = viewModel.incidentEnvironmentalViewModel?.VisibleDamageID,
                    PeopleInjuredId = viewModel.incidentEnvironmentalViewModel?.PeopleInjuredID,
                    GasPresentId = viewModel.incidentEnvironmentalViewModel?.GasodorpresentID,

                    Landmark = viewModel.incidentiLocation?.Landmark,
                    LocationAddress = viewModel.incidentiLocation?.Address,
                    ServiceAccount = viewModel.incidentiLocation?.ServiceAccount,
                    AssetIds = viewModel.incidentiLocation?.AssetIDs,
                    IsSameCallerAddress = viewModel.incidentiLocation.IsSameCallerAddress,

                    ImageUrl = viewModel.incidentSupportingInfoViewModel?.ImageUrl,
                    SupportInfoNotes = viewModel.incidentSupportingInfoViewModel?.Notes,

                    Lat = latLong?.Lat ?? 0,
                    Lng = latLong?.Lng ?? 0,
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

        public async Task<string> UpdateIncident(IncidentViewModel viewModel)
        {
            try
            {
                var incident = await _db.Incidents.FirstOrDefaultAsync(p => p.Id == viewModel.Id);

                // If no incident, save as new
                if (incident == null)
                {
                    return await SaveIncident(viewModel);
                }

                var latLong = await GetLatLngFromAddress(viewModel.incidentiLocation.Address);

                // Save file if available
                var file = viewModel.incidentSupportingInfoViewModel?.File;
                var imageUrl = (file?.Count > 0) ? await SaveAttachments(file) : null;

                if (!string.IsNullOrWhiteSpace(imageUrl))
                {
                    viewModel.incidentSupportingInfoViewModel!.ImageUrl = imageUrl;
                }
                else
                {
                    viewModel.incidentSupportingInfoViewModel!.ImageUrl ??= incident.ImageUrl;
                }

                // Update entity from ViewModel
                incident.SeverityLevelId = viewModel.severityLevelId;
                incident.DescriptionIssue = viewModel.DescriptionIssue;

                var caller = viewModel.incidentCellerInformation;
                incident.CallerAddress = caller?.CallerAddress;
                incident.CallerPhoneNumber = caller?.CallerPhoneNumber;
                incident.CallerName = caller?.CallerName;
                incident.CallTime = caller?.CallTime ?? incident.CallTime;
                incident.RelationshipId = caller?.RelationshipId;

                var details = viewModel.incidentDetails;
                incident.EventTypeIds = details?.EventTypeIds;
                incident.IsOtherEvent = details?.IsOtherEvent ?? false;
                incident.OtherEventDetail = details?.OtherEventDetail;

                var env = viewModel.incidentEnvironmentalViewModel;
                incident.EvacuationRequiredId = env?.EvacuationRequiredID;
                incident.HissingPresentId = env?.HissingSoundPresentID;
                incident.VisibleDamagePresentId = env?.VisibleDamageID;
                incident.PeopleInjuredId = env?.PeopleInjuredID;
                incident.GasPresentId = env?.GasodorpresentID;

                var loc = viewModel.incidentiLocation;
                incident.Landmark = loc?.Landmark;
                incident.LocationAddress = loc?.Address;
                incident.ServiceAccount = loc?.ServiceAccount;
                incident.AssetIds = loc?.AssetIDs;
                incident.IsSameCallerAddress = loc?.IsSameCallerAddress ?? false;

                var support = viewModel.incidentSupportingInfoViewModel;
                incident.ImageUrl = support?.ImageUrl ?? incident.ImageUrl;
                incident.SupportInfoNotes = support?.Notes;

                incident.Lat = latLong?.Lat ?? 0;
                incident.Lng = latLong?.Lng ?? 0;

                // Save within transaction
                await using var transaction = await _db.Database.BeginTransactionAsync();
                try
                {
                    await _db.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }

                return incident.IncidentID;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating incident.");
                return string.Empty;
            }
        }

        public async Task<List<IncidentGridViewModel>> GetIncidentList(FilterRequest request)
        {

            List<IncidentGridViewModel> incidentGridViews = new();
            try
            {
                var query = _db.Incidents
                             .Include(p => p.StatusLegend)
                             .Include(p => p.Relationship)
                             //.Include(p => p.EventType)
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

                    if (!string.IsNullOrWhiteSpace(request.description))
                    {
                        query = query.Where(p => p.DescriptionIssue.Contains(request.description));
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
                        EventTypeId = await GetEventTypes(item.EventTypeIds ?? string.Empty),
                        GasESIndicator = GetIndicator(item.GasPresentId),
                        Id = item.Id,
                        Intersection = item.Landmark ?? string.Empty,
                        LocationAddress = item.LocationAddress ?? string.Empty,
                        Severity = item.SeverityLevel.Name,
                        SeverityId = item.SeverityLevelId,
                        StatusLegend = item.StatusLegend.Name,
                        StatusLegendColor = item.StatusLegend.Color,
                        StatusLegendId = item.StatusLegendId,
                        RelationShipName = item.Relationship.Name,
                        RelationShipId = item.RelationshipId,
                    });
                }
                return incidentGridViews;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetIncidentList.");
                return new List<IncidentGridViewModel>();
            }
        }

        //public async Task<string?> ChangeIncidentStatus(long incidentId, string statusText)
        //{
        //    await using var transaction = await _db.Database.BeginTransactionAsync();

        //    try
        //    {
        //        var incident = await _db.Incidents.FirstOrDefaultAsync(p => p.Id == incidentId);

        //        if (incident == null)
        //        {
        //            await transaction.RollbackAsync();
        //            return null; // or string.Empty if you want
        //        }

        //        if (Enum.TryParse<StatusLegendEnum>(statusText, true, out var status))
        //        {
        //            incident.StatusLegendId = (long)status;

        //            await _db.SaveChangesAsync();
        //            await transaction.CommitAsync();
        //        }
        //        else
        //        {
        //            await transaction.RollbackAsync();
        //            return null;
        //        }

        //        return incident.IncidentID;
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        _logger.LogError(ex, "Error ChangeIncidentStatus.");
        //        return null; // or string.Empty
        //    }
        //}
        public async Task<string?> ChangeIncidentStatus(long incidentId, string statusText)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var incident = await _db.Incidents.FirstOrDefaultAsync(p => p.Id == incidentId);

                if (incident == null)
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                // find matching status from StatusLegends table
                var statusLegend = await _db.StatusLegends
                    .FirstOrDefaultAsync(s => s.Name.ToLower() == statusText.ToLower());

                if (statusLegend == null)
                {
                    await transaction.RollbackAsync();
                    return null; // no such status in DB
                }

                // update incident status
                incident.StatusLegendId = statusLegend.Id;
                incident.UpdatedOn = DateTime.UtcNow;

                // add history entry
                var history = new IncidentHistory
                {
                    IncidentId = incident.Id,
                    StatusLegendId = statusLegend.Id,
                    Description = $"Status changed to {statusLegend.Name}",
                    IsDeleted = false,
                    ActiveStatus = Enums.ActiveStatus.Active,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = 1,  // replace with current user id
                    UpdatedOn = DateTime.UtcNow,
                    UpdatedBy = 1   // replace with current user id
                };

                _db.IncidentHistories.Add(history);

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return incident.IncidentID;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error ChangeIncidentStatus.");
                return null;
            }
        }

        public async Task<IncidentViewModel> GetById(long incidentId)
        {
            var incidentViewModel = new IncidentViewModel();

            try
            {
                incidentViewModel = await GetIncidentDropDown();

                var incident = await _db.Incidents.FirstOrDefaultAsync(p => p.Id == incidentId);

                if (incident == null)
                {
                    return new IncidentViewModel();
                }

                incidentViewModel.Id = incident?.Id;

                incidentViewModel.DescriptionIssue = incident?.DescriptionIssue;
                incidentViewModel.severityLevelId = incident?.SeverityLevelId;
                incidentViewModel.incidentiLocation.Address = incident?.LocationAddress;
                incidentViewModel.incidentiLocation.AssetIDs = incident?.AssetIds;
                incidentViewModel.incidentiLocation.Landmark = incident?.Landmark;
                incidentViewModel.incidentiLocation.ServiceAccount = incident?.ServiceAccount;
                incidentViewModel.incidentiLocation.IsSameCallerAddress = incident.IsSameCallerAddress;

                incidentViewModel.incidentDetails.EventTypeIds = incident?.EventTypeIds;
                incidentViewModel.incidentDetails.OtherEventDetail = incident?.OtherEventDetail;
                incidentViewModel.incidentDetails.IsOtherEvent = incident.IsOtherEvent;

                incidentViewModel.incidentCellerInformation.CallerPhoneNumber = incident.CallerPhoneNumber;
                incidentViewModel.incidentCellerInformation.CallerAddress = incident.CallerAddress;
                incidentViewModel.incidentCellerInformation.CallerName = incident.CallerName;
                incidentViewModel.incidentCellerInformation.CallTime = incident.CallTime;
                incidentViewModel.incidentCellerInformation.RelationshipId = incident.RelationshipId;

                incidentViewModel.incidentEnvironmentalViewModel.PeopleInjuredID = incident.PeopleInjuredId;
                incidentViewModel.incidentEnvironmentalViewModel.HissingSoundPresentID = incident.HissingPresentId;
                incidentViewModel.incidentEnvironmentalViewModel.EvacuationRequiredID = incident.EvacuationRequiredId;
                incidentViewModel.incidentEnvironmentalViewModel.VisibleDamageID = incident.VisibleDamagePresentId;
                incidentViewModel.incidentEnvironmentalViewModel.GasodorpresentID = incident.GasPresentId;

                incidentViewModel.incidentSupportingInfoViewModel.ImageUrl = incident.ImageUrl;
                incidentViewModel.incidentSupportingInfoViewModel.Notes = incident.SupportInfoNotes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetById.");
                return new IncidentViewModel();
            }

            return incidentViewModel;
        }

        public async Task<IncidentViewModel> GetIncidentDetailsById(long id)
        {
            try
            {
                var incident = await _db.Incidents
                    .Include(i => i.StatusLegend)
                    .Include(i => i.SeverityLevel)
                    .Include(i => i.Relationship)
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (incident == null)
                    return new IncidentViewModel();

                var viewModel = new IncidentViewModel
                {
                    Id = incident.Id,
                    DescriptionIssue = incident.DescriptionIssue,
                    severityLevelId = incident.SeverityLevelId,


                    incidentDetails = new IncidentDetailsViewModel
                    {
                        EventTypeIds = incident.EventTypeIds,
                        IsOtherEvent = incident.IsOtherEvent,
                        OtherEventDetail = incident.OtherEventDetail ?? string.Empty,
                        EventTypes = new List<SelectListItem>()
                    },

                    incidentDetailByIdViewModel = new IncidentDetailByIdViewModel()
                    {
                        SeverityName = incident.SeverityLevel?.Name ?? string.Empty,
                        SeverityColor = incident.SeverityLevel?.Color ?? string.Empty,
                        StatusLegendName = incident.StatusLegend?.Name ?? string.Empty,
                        StatusLegendColor = incident.StatusLegend?.Color ?? string.Empty,
                        IncidentNumber = incident.IncidentID,
                        CreatedOn = incident.CreatedOn,
                        UpdatedOn = incident.UpdatedOn,
                        CreatedOnDate = GetDate(Convert.ToString(incident.CreatedOn)),
                        CreatedOnTime = GetTime(Convert.ToString(incident.CreatedOn)),
                    },

                    incidentCellerInformation = new IncidentCellerInformationViewModel
                    {
                        CallerName = incident.CallerName,
                        CallerPhoneNumber = incident.CallerPhoneNumber,
                        CallerAddress = incident.CallerAddress,
                        CallTime = incident.CallTime,
                        RelationshipId = incident.RelationshipId,
                        RelationshipName = incident.Relationship?.Name ?? string.Empty,
                        CallDateInFormat = GetDate(Convert.ToString(incident.CallTime)),
                        CallTimeInFormat = GetTime(Convert.ToString(incident.CallTime)),
                    },

                    incidentiLocation = new IncidentiLocationViewModel
                    {
                        Address = incident.LocationAddress,
                        Landmark = incident.Landmark,
                        ServiceAccount = incident.ServiceAccount,
                        AssetIDs = incident.AssetIds,
                        IsSameCallerAddress = incident.IsSameCallerAddress,
                        AssetsIncidentList = new List<SelectListItem>()
                    },

                    incidentEnvironmentalViewModel = new IncidentEnvironmentalViewModel
                    {
                        GasodorpresentID = incident.GasPresentId,
                        HissingSoundPresentID = incident.HissingPresentId,
                        VisibleDamageID = incident.VisibleDamagePresentId,
                        PeopleInjuredID = incident.PeopleInjuredId,
                        EvacuationRequiredID = incident.EvacuationRequiredId,

                        GasOdorText = GetIndicator(incident.GasPresentId),
                        HissingSoundText = GetIndicator(incident.HissingPresentId),
                        VisibleDamageText = GetIndicator(incident.VisibleDamagePresentId),
                        PeopleInjuredText = GetIndicator(incident.PeopleInjuredId),
                        EvacuationRequiredText = GetIndicator(incident.EvacuationRequiredId)
                    },

                    incidentSupportingInfoViewModel = new IncidentSupportingInfoViewModel
                    {
                        Notes = incident.SupportInfoNotes,
                        ImageUrl = incident.ImageUrl, // keep original value

                        // ✅ split comma-separated image URLs
                        ImageUrls = !string.IsNullOrEmpty(incident.ImageUrl)
                         ? incident.ImageUrl.Split(",", StringSplitOptions.RemoveEmptyEntries)
                           .Select(img => img.Trim())
                           .ToList()
                            : new List<string>()
                    }
                };

                // ✅ Resolve EventType names
                if (!string.IsNullOrWhiteSpace(incident.EventTypeIds))
                {
                    var ids = incident.EventTypeIds.Split(',').Select(long.Parse).ToList();
                    viewModel.incidentDetails.EventTypeNames = await _db.EventTypes
                        .Where(et => ids.Contains(et.Id))
                        .Select(et => et.Name)
                        .ToListAsync();
                }

                // ✅ Resolve Asset names
                if (!string.IsNullOrWhiteSpace(incident.AssetIds))
                {
                    var ids = incident.AssetIds.Split(',').Select(long.Parse).ToList();
                    viewModel.incidentiLocation.AssetNames = await _db.AssetIncidents
                        .Where(a => ids.Contains(a.Id))
                        .Select(a => a.Name)
                        .ToListAsync();
                }

                return viewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetIncidentById.");
                return new IncidentViewModel();
            }
        }

        public async Task<GeocodeResult?> GetLatLngFromAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return new GeocodeResult
                {
                    Lat = 0,
                    Lng = 0
                };

            try
            {

                using var client = new HttpClient();
                string url = $"https://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/findAddressCandidates" +
                             $"?f=json&SingleLine={Uri.EscapeDataString(address)}";

                var response = await client.GetStringAsync(url);

                using var doc = JsonDocument.Parse(response);
                var candidates = doc.RootElement.GetProperty("candidates");

                if (candidates.GetArrayLength() > 0)
                {
                    var location = candidates[0].GetProperty("location");
                    return new GeocodeResult
                    {
                        Lat = location.GetProperty("y").GetDouble(),
                        Lng = location.GetProperty("x").GetDouble()
                    };
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetLatLngFromAddress.");
                return new GeocodeResult
                {
                    Lat = 0,
                    Lng = 0
                };
            }
            return new GeocodeResult
            {
                Lat = 0,
                Lng = 0
            };
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
                2 => "N/A",
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
        private async Task<string> SaveAttachments(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return string.Empty;
            }

            var fileList = new List<string>();
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Storage", "uploads", "incidents");

            // Ensure directory exists
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            foreach (var fileItem in files)
            {
                if (fileItem.Length <= 0) continue; // skip empty files

                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(fileItem.FileName)}";
                var filePath = Path.Combine(uploadsPath, fileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await fileItem.CopyToAsync(stream);
                }

                // Save relative path (for serving in browser)
                var relativePath = $"/Storage/uploads/incidents/{fileName}";
                fileList.Add(relativePath);

                _logger.LogInformation("Saved attachment: {FileName} at {Path}", fileName, relativePath);
            }

            // Return comma-separated list of paths
            return string.Join(",", fileList);
        }

        #endregion
    }
}
