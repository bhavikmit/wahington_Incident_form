using AutoMapper;

using Azure;

using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;

using DataLibrary;

using Helpers.Extensions;
using Helpers.File;

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
using ViewModels.Shared;

namespace Repositories.Common
{
    public class IncidentService : IIncidentService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<IncidentService> _logger;
        private readonly IMapper _mapper;

        public IncidentService(ApplicationDbContext db, ILogger<IncidentService> logger, IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<SelectListItem>> GetIncidentDropDown()
        {
            try
            {
                var statusLegends = await _db.StatusLegends
                    .Where(it => !it.IsDeleted)
                    .OrderBy(it => it.Name)
                    .Select(it => new SelectListItem
                    {
                        Value = it.Id.ToString(),
                        Text = it.Name
                    })
                    .ToListAsync();

                return statusLegends;

                //var statusLegends = await _db.
                //    .Where(it => !it.IsDeleted)
                //    .OrderBy(it => it.Name)
                //    .Select(it => new SelectListItem
                //    {
                //        Value = it.Id.ToString(),
                //        Text = it.Name
                //    })
                //    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating incident.");
                return null!;
            }
        }
    }
}
