using Models.Common.Interfaces;
using ViewModels.Shared;
using Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Pagination;
using System.Linq.Expressions;
using ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Repositories.Common
{
    public class SeverityLevelService<CreateViewModel, UpdateViewModel, DetailViewModel>
        : BaseService<SeverityLevel, CreateViewModel, UpdateViewModel, DetailViewModel>,
          ISeverityLevelService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ModelStateDictionary _modelState;
        private readonly ApplicationDbContext _db;

        public SeverityLevelService(
            ApplicationDbContext db,
            ILogger<SeverityLevelService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger,
            IMapper mapper,
            IRepositoryResponse response,
            IActionContextAccessor actionContext
        ) : base(db, logger, mapper, response)
        {
            _modelState = actionContext.ActionContext.ModelState;
            _db = db;
        }

        public override async Task<Expression<Func<SeverityLevel, bool>>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as SeverityLevelSearchViewModel;

            return x =>
                        (
                            string.IsNullOrEmpty(searchFilters.Search.value) ||
                            x.Name.ToLower().Contains(searchFilters.Search.value.ToLower()) ||
                            x.Description.ToLower().Contains(searchFilters.Search.value.ToLower()) ||
                            x.Color.ToLower().Contains(searchFilters.Search.value.ToLower())
                        )
                        &&
                        (string.IsNullOrEmpty(searchFilters.Name) || x.Name.ToLower().Contains(searchFilters.Name.ToLower()))
                        &&
                        (string.IsNullOrEmpty(searchFilters.Description) || x.Description.ToLower().Contains(searchFilters.Description.ToLower()))
                        &&
                        (string.IsNullOrEmpty(searchFilters.Color) || x.Color.ToLower().Contains(searchFilters.Color.ToLower()));
        }
    }
}
