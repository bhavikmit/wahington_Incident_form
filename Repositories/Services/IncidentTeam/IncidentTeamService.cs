using AutoMapper;

using Centangle.Common.ResponseHelpers.Models;

using DataLibrary;

using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Vml.Office;

using Enums;

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Models;
using Models.Common.Interfaces;

using Pagination;

using Repositories.Common;

using System.Linq.Expressions;

using ViewModels;
using ViewModels.Incident;
using ViewModels.Shared;

namespace Repositories.Services
{
    public class IncidentTeamService<CreateViewModel, UpdateViewModel, DetailViewModel>
        : BaseService<IncidentTeam, CreateViewModel, UpdateViewModel, DetailViewModel>,
          IIncidentTeamService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ModelStateDictionary _modelState;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<IncidentTeamService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        public IncidentTeamService(
            ApplicationDbContext db,
            ILogger<IncidentTeamService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger,
            IMapper mapper,
            IRepositoryResponse response,
            IActionContextAccessor actionContext)
            : base(db, logger, mapper, response)
        {
            _modelState = actionContext.ActionContext.ModelState;
            _db = db;
            _logger = logger;
        }

        //public override async Task<Expression<Func<IncidentTeam, bool>>> SetQueryFilter(IBaseSearchModel filters)
        //{
        //    var searchFilters = filters as IncidentTeamSearchViewModel;

        //    return x =>
        //                (
        //                    (
        //                        string.IsNullOrEmpty(searchFilters.Search.value)
        //                        ||
        //                        x.Name.ToLower().Contains(searchFilters.Search.value.ToLower())
        //                    )
        //                )
        //                &&
        //                (string.IsNullOrEmpty(searchFilters.Name) || x.Name.ToLower().Contains(searchFilters.Name.ToLower()))
        //                ;
        //}

        //public async Task<List<IncidentTeamModifyViewModel>> GetAllIncidentTeams()
        //{
        //    List<IncidentTeamModifyViewModel> IncidentTeams = new();
        //    try
        //    {

        //        var reslations = await _db.IncidentTeams.Where(p => !p.IsDeleted).ToListAsync();

        //        foreach (var relation in reslations)
        //        {
        //            IncidentTeams.Add(new IncidentTeamModifyViewModel()
        //            {
        //                Id = relation.Id,
        //                Name = relation.Name
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error GetAllIncidentTeams.");
        //        return new List<IncidentTeamModifyViewModel>()!;
        //    }
        //    return IncidentTeams;
        //}
        //public async Task<long> SaveRelation(IncidentTeamModifyViewModel viewModel)
        //{
        //    await using var transaction = await _db.Database.BeginTransactionAsync();
        //    try
        //    {
        //        // Map ViewModel → Entity
        //        var IncidentTeam = new IncidentTeam
        //        {
        //            Name = viewModel.Name
        //        };

        //        // Save
        //        await _db.IncidentTeams.AddAsync(IncidentTeam);
        //        await _db.SaveChangesAsync();
        //        await transaction.CommitAsync();

        //        return IncidentTeam.Id;
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        _logger.LogError(ex, "Error SaveRelation.");
        //        return 0;
        //    }
        //}
        //public async Task<long> UpdateRelation(IncidentTeamModifyViewModel viewModel)
        //{
        //    try
        //    {

        //        var relation = await _db.IncidentTeams.Where(p => p.Id == viewModel.Id).FirstOrDefaultAsync();

        //        if (relation == null)
        //        {
        //            await SaveRelation(viewModel);
        //        }

        //        // Save within transaction
        //        await using var transaction = await _db.Database.BeginTransactionAsync();

        //        relation.Name = viewModel.Name;

        //        try
        //        {
        //            await _db.SaveChangesAsync();
        //            await transaction.CommitAsync();
        //        }
        //        catch
        //        {
        //            await transaction.RollbackAsync();
        //            throw;
        //        }

        //        return relation.Id;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error SaveRelation.");
        //        return 0;
        //    }
        //}
        //public async Task<IncidentTeamModifyViewModel> GetRelationById(long Id)
        //{
        //    var IncidentTeamView = new IncidentTeamModifyViewModel();

        //    try
        //    {
        //        var IncidentTeam = await _db.IncidentTeams.FirstOrDefaultAsync(p => p.Id == Id);

        //        if (IncidentTeam == null)
        //        {
        //            return new IncidentTeamModifyViewModel();
        //        }

        //        IncidentTeamView.Name = IncidentTeam.Name;
        //        IncidentTeamView.Id = IncidentTeam.Id;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error GetById.");
        //        return new IncidentTeamModifyViewModel();
        //    }

        //    return IncidentTeamView;
        //}
        //public async Task<long> DeleteRelation(long Id)
        //{
        //    try
        //    {

        //        var relation = await _db.IncidentTeams.Where(p => p.Id == Id).FirstOrDefaultAsync();

        //        if (relation == null)
        //        {
        //            return 0;
        //        }

        //        // Save within transaction
        //        await using var transaction = await _db.Database.BeginTransactionAsync();

        //        relation.IsDeleted = true;

        //        try
        //        {
        //            await _db.SaveChangesAsync();
        //            await transaction.CommitAsync();
        //        }
        //        catch
        //        {
        //            await transaction.RollbackAsync();
        //            throw;
        //        }

        //        return relation.Id;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error DeleteRelation.");
        //        return 0;
        //    }
        //}
    }
}
