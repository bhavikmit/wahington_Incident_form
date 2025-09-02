using AutoMapper;

using Azure;

using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;

using DataLibrary;

using DocumentFormat.OpenXml.InkML;
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
using ViewModels.Dashboard;
using ViewModels.Incident;
using ViewModels.Shared;

namespace Repositories.Common
{
    public class IncidentDashboardService : IIncidentDashboardService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<IncidentDashboardService> _logger;

        public IncidentDashboardService(ApplicationDbContext db, ILogger<IncidentDashboardService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<DashboardViewModel> GetIncidentDashboardReport()
        {
            try
            {
                var incidents = _db.Incidents.Where(i => !i.IsDeleted);

                var severityData = await incidents
                    .GroupBy(i => i.SeverityLevelId)
                    .Select(g => new
                    {
                        Id = g.Key,
                        Count = g.Count()
                    })
                    .Join(_db.SeverityLevels,
                          g => g.Id,
                          s => s.Id,
                          (g, s) => new IncidentDashboardSeverityReportViewModel
                          {
                              color = s.Color,
                              name = s.Name,
                              count = g.Count
                          })
                    .ToListAsync();

                var statusData = await incidents
                    .GroupBy(i => i.StatusLegendId)
                    .Select(g => new
                    {
                        Id = g.Key,
                        Count = g.Count()
                    })
                    .Join(_db.StatusLegends,
                          g => g.Id,
                          s => s.Id,
                          (g, s) => new IncidentDashboardStatusReportViewModel
                          {
                              color = s.Color,
                              name = s.Name,
                              count = g.Count
                          })
                    .ToListAsync();

                var totalIncidentCount = await incidents.CountAsync();

                var totalSeverity = severityData.Sum(x => x.count);
                var totalStatus = statusData.Sum(x => x.count);

                foreach (var s in severityData)
                {
                    s.SeverityPercentage = totalSeverity == 0
                        ? 0
                        : Math.Round((decimal)s.count / totalSeverity * 100, 2);
                }

                foreach (var s in statusData)
                {
                    s.StatusPercentage = totalStatus == 0
                        ? 0
                        : Math.Round((decimal)s.count / totalStatus * 100, 2);
                }

                return new DashboardViewModel
                {
                    IncidentDashboard = new IncidentDashboardViewModel
                    {
                        SeverityLabels = severityData.Select(s => s.name).ToList(),
                        SeverityCounts = severityData.Select(s => s.count).ToList(),
                        SeverityColors = severityData.Select(s => s.color).ToList(),
                        StatusLabels = statusData.Select(s => s.name).ToList(),
                        StatusCounts = statusData.Select(s => s.count).ToList(),
                        StatusColors = statusData.Select(s => s.color).ToList(),
                        ListIncidentDashboardSeverityReportViewModel = severityData,
                        ListIncidentDashboardStatusReport = statusData,
                        TotalIncidentCount = totalIncidentCount,
                        TotalSeverityCount = totalSeverity,
                        TotalStatusLegendCount = totalStatus,
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetIncidentDashboardReport.");
                return new DashboardViewModel();
            }
        }


        //public async Task<DashboardViewModel> GetIncidentDashboardReport()
        //{
        //    try
        //    {
        //        // Run severity + status queries in parallel
        //        var severityTask = await _db.Incidents
        //            .Where(i => !i.IsDeleted)
        //            .GroupBy(i => i.SeverityLevelId)
        //            .Select(g => new
        //            {
        //                Id = g.Key,
        //                Count = g.Count()
        //            })
        //            .Join(_db.SeverityLevels,
        //                  g => g.Id,
        //                  s => s.Id,
        //                  (g, s) => new
        //                  {
        //                      Id = g.Id,
        //                      Name = s.Name,
        //                      Color = s.Color,
        //                      Count = g.Count
        //                  })
        //            .ToListAsync();

        //        var totalIncidentCount = await _db.Incidents
        //            .Where(i => !i.IsDeleted).CountAsync();

        //        var statusTask = await _db.Incidents
        //            .Where(i => !i.IsDeleted)
        //            .GroupBy(i => i.StatusLegendId)
        //            .Select(g => new
        //            {
        //                Id = g.Key,
        //                Count = g.Count()
        //            })
        //            .Join(_db.StatusLegends,
        //                  g => g.Id,
        //                  s => s.Id,
        //                  (g, s) => new
        //                  {
        //                      Id = g.Id,
        //                      Name = s.Name,
        //                      Color = s.Color,
        //                      Count = g.Count
        //                  })
        //            .ToListAsync();


        //        var severityData = severityTask;
        //        var statusData = statusTask;

        //        var totalStatus = statusData.Sum(x => x.Count);
        //        var totalSeverity = severityData.Sum(x => x.Count);

        //        List<IncidentDashboardStatusReportViewModel> incidentDashboardStatusReportViewModels = new();
        //        foreach (var item in statusData)
        //        {
        //            incidentDashboardStatusReportViewModels.Add(new IncidentDashboardStatusReportViewModel()
        //            {
        //                color = item.Color,
        //                name = item.Name,
        //                count = item.Count,
        //                StatusPercentage = Math.Round((decimal)item.Count / totalStatus * 100, 2),
        //            });
        //        }

        //        List<IncidentDashboardSeverityReportViewModel> incidentDashboardSeverityReportViewModel = new();
        //        foreach (var item in severityData)
        //        {
        //            incidentDashboardSeverityReportViewModel.Add(new IncidentDashboardSeverityReportViewModel()
        //            {
        //                color = item.Color,
        //                name = item.Name,
        //                count = item.Count,
        //                SeverityPercentage = Math.Round((decimal)item.Count / totalSeverity * 100, 2),
        //            });
        //        }

        //        // Build view model
        //        var dashboard = new IncidentDashboardViewModel
        //        {
        //            SeverityLabels = severityData.Select(s => s.Name).ToList(),
        //            SeverityCounts = severityData.Select(s => s.Count).ToList(),
        //            SeverityColors = severityData.Select(s => s.Color).ToList(),

        //            StatusLabels = statusData.Select(s => s.Name).ToList(),
        //            StatusCounts = statusData.Select(s => s.Count).ToList(),
        //            StatusColors = statusData.Select(s => s.Color).ToList(),
        //            ListIncidentDashboardSeverityReportViewModel = incidentDashboardSeverityReportViewModel,
        //            ListIncidentDashboardStatusReport = incidentDashboardStatusReportViewModels,
        //            TotalIncidentCount = totalIncidentCount,
        //            TotalSeverityCount = totalSeverity,
        //            TotalStatusLegendCount = totalStatus,
        //        };

        //        return new DashboardViewModel()
        //        {
        //            IncidentDashboard = dashboard
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error GetIncidentDashboardReport.");
        //        return new DashboardViewModel();
        //    }
        //}
    }
}
