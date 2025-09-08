using AutoMapper;

using Azure;

using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;

using DataLibrary;

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

        public IncidentValidationService(ApplicationDbContext db, ILogger<IncidentValidationService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<List<IncidentValidationPendingViewModel>> GetValidationPendingList()
        {
            List<IncidentValidationPendingViewModel> incidentValidationPendings = new();
            try
            {
                var query = _db.Incidents.Where(p => !p.IsDeleted && p.StatusLegendId != (int)StatusLegendEnum.Started)
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
                var query = _db.Incidents.Where(p => !p.IsDeleted && p.StatusLegendId == (int)StatusLegendEnum.Started
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
    }
}
