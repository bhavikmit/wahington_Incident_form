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

using System.Linq.Expressions;

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

        public async Task<string> SaveIncident(IncidentViewModel incidentViewModel)
        {
            try
            {
                var transaction = await _db.Database.BeginTransactionAsync();

                var incident = new Incident();

                if (incidentViewModel.incidentSupportingInfoViewModel.File != null)
                {
                    incidentViewModel.incidentSupportingInfoViewModel.ImageUrl = await SaveAttachments(incidentViewModel.incidentSupportingInfoViewModel.File);
                }
                var totalIncidentCount = await _db.Equipments.IgnoreQueryFilters().CountAsync();

                incident.IncidentID = "INC-" + (totalIncidentCount + 1).ToString("D4");
                incident.StatusLegendId = (int)StatusLegendEnum.Submitted;
                incident.SeverityLevelId = incidentViewModel.severityLevelId;
                incident.DescriptionIssue = incidentViewModel.DescriptionIssue;

                incident.CallerAddress = incidentViewModel.incidentCellerInformation.CallerAddress;
                incident.CallerPhoneNumber = incidentViewModel.incidentCellerInformation.CallerPhoneNumber;
                incident.CallerName = incidentViewModel.incidentCellerInformation.CallerName;
                incident.CallTime = (DateTime)incidentViewModel.incidentCellerInformation.CallTime;
                incident.RelationshipId = incidentViewModel.incidentCellerInformation.RelationshipId;

                incident.EventTypeId = incidentViewModel.incidentDetails.EventTypeId;
                

                incident.EvacuationRequiredId = incidentViewModel.incidentEnvironmentalViewModel.EvacuationRequiredID;
                incident.HissingPresentId = incidentViewModel.incidentEnvironmentalViewModel.HissingSoundPresentID;
                incident.VisibleDamagePresentId = incidentViewModel.incidentEnvironmentalViewModel.VisibleDamageID;
                incident.PeopleInjuredId = incidentViewModel.incidentEnvironmentalViewModel.PeopleInjuredID;
                incident.GasPresentId = incidentViewModel.incidentEnvironmentalViewModel.GasodorpresentID;

                incident.Landmark = incidentViewModel.incidentiLocation.Landmark;
                incident.LocationAddress = incidentViewModel.incidentiLocation.Address;
                incident.ServiceAccount = incidentViewModel.incidentiLocation.ServiceAccount;
                incident.AssetIds = incidentViewModel.incidentiLocation.AssetIDs;

                return incident.IncidentID;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error SaveIncident.");
                return "";
            }
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
    }
}
