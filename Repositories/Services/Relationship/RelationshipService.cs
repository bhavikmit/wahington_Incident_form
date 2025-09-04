using AutoMapper;

using Centangle.Common.ResponseHelpers.Models;

using DataLibrary;

using DocumentFormat.OpenXml.Drawing.Diagrams;

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Models;
using Models.Common.Interfaces;

using Pagination;

using System.Linq.Expressions;

using ViewModels;
using ViewModels.Incident;
using ViewModels.Shared;

namespace Repositories.Common
{
    public class RelationshipService<CreateViewModel, UpdateViewModel, DetailViewModel>
        : BaseService<Relationship, CreateViewModel, UpdateViewModel, DetailViewModel>,
          IRelationshipService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ModelStateDictionary _modelState;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<RelationshipService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        public RelationshipService(
            ApplicationDbContext db,
            ILogger<RelationshipService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger,
            IMapper mapper,
            IRepositoryResponse response,
            IActionContextAccessor actionContext)
            : base(db, logger, mapper, response)
        {
            _modelState = actionContext.ActionContext.ModelState;
            _db = db;
            _logger = logger;
        }

        public override async Task<Expression<Func<Relationship, bool>>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as RelationshipSearchViewModel;

            return x =>
                        (
                            (
                                string.IsNullOrEmpty(searchFilters.Search.value)
                                ||
                                x.Name.ToLower().Contains(searchFilters.Search.value.ToLower())
                            )
                        )
                        &&
                        (string.IsNullOrEmpty(searchFilters.Name) || x.Name.ToLower().Contains(searchFilters.Name.ToLower()))
                        ;
        }

        public async Task<List<RelationshipModifyViewModel>> GetAllRelationships()
        {
            List<RelationshipModifyViewModel> relationships = new();
            try
            {

                var reslations = await _db.Relationships.Where(p => !p.IsDeleted).ToListAsync();

                foreach (var relation in reslations)
                {
                    relationships.Add(new RelationshipModifyViewModel()
                    {
                        Id = relation.Id,
                        Name = relation.Name
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetAllRelationships.");
                return new List<RelationshipModifyViewModel>()!;
            }
            return relationships;
        }
    }
}
