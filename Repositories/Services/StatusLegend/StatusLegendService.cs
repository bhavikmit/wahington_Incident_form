using AutoMapper;

using Centangle.Common.ResponseHelpers.Models;

using DataLibrary;

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Models;
using Models.Common.Interfaces;

using Pagination;

using System.Linq.Expressions;

using ViewModels;
using ViewModels.Shared;

namespace Repositories.Common
{
    public class StatusLegendService<CreateViewModel, UpdateViewModel, DetailViewModel>
        : BaseService<StatusLegend, CreateViewModel, UpdateViewModel, DetailViewModel>,
          IStatusLegendService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ModelStateDictionary _modelState;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<StatusLegendService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;

        public StatusLegendService(
            ApplicationDbContext db,
            ILogger<StatusLegendService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger,
            IMapper mapper,
            IRepositoryResponse response,
            IActionContextAccessor actionContext)
            : base(db, logger, mapper, response)
        {
            _modelState = actionContext.ActionContext.ModelState;
            _db = db;
            _logger = logger;
        }

        //public override async Task<Expression<Func<StatusLegend, bool>>> SetQueryFilter(IBaseSearchModel filters)
        //{
        //    var searchFilters = filters as StatusLegendSearchViewModel;

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

        public async Task<List<StatusLegendModifyViewModel>> GetAllStatusLegends()
        {
            List<StatusLegendModifyViewModel> statusLegend = new();
            try
            {
                var StatusLegendList = await _db.StatusLegends.Where(p => !p.IsDeleted).ToListAsync();

                foreach (var StatusLegend in StatusLegendList)
                {
                    statusLegend.Add(new StatusLegendModifyViewModel()
                    {
                        Id = StatusLegend.Id,
                        Name = StatusLegend.Name,
                        Color = StatusLegend.Color,
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetAllStatusLegend.");
                return new List<StatusLegendModifyViewModel>()!;
            }
            return statusLegend;
        }
        public async Task<long> SaveStatusLegend(StatusLegendModifyViewModel viewModel)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                // Map ViewModel → Entity
                var StatusLegend = new StatusLegend
                {
                    Name = viewModel.Name,
                    Color = viewModel.Color
                };

                // Save
                await _db.StatusLegends.AddAsync(StatusLegend);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return StatusLegend.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error SaveStatusLegend.");
                return 0;
            }
        }
        public async Task<long> UpdateStatusLegend(StatusLegendModifyViewModel viewModel)
        {
            try
            {

                var StatusLegend = await _db.StatusLegends.Where(p => p.Id == viewModel.Id).FirstOrDefaultAsync();

                if (StatusLegend == null)
                {
                    await SaveStatusLegend(viewModel);
                }

                // Save within transaction
                await using var transaction = await _db.Database.BeginTransactionAsync();

                StatusLegend.Name = viewModel.Name;
                StatusLegend.Color = viewModel.Color;
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

                return StatusLegend.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error UpdateStatusLegend.");
                return 0;
            }
        }
        public async Task<StatusLegendModifyViewModel> GetStatusLegendById(long Id)
        {
            var StatusLegendView = new StatusLegendModifyViewModel();

            try
            {
                var StatusLegend = await _db.StatusLegends.FirstOrDefaultAsync(p => p.Id == Id);

                if (StatusLegend == null)
                {
                    return new StatusLegendModifyViewModel();
                }

                StatusLegendView.Name = StatusLegend.Name;
                StatusLegendView.Color = StatusLegend.Color;
                StatusLegendView.Id = StatusLegend.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetStatusLegendById.");
                return new StatusLegendModifyViewModel();
            }

            return StatusLegendView;
        }
        public async Task<long> DeleteStatusLegend(long Id)
        {
            try
            {

                var StatusLegend = await _db.StatusLegends.Where(p => p.Id == Id).FirstOrDefaultAsync();

                if (StatusLegend == null)
                {
                    return 0;
                }

                // Save within transaction
                await using var transaction = await _db.Database.BeginTransactionAsync();

                StatusLegend.IsDeleted = true;

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

                return StatusLegend.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error DeleteStatusLegend.");
                return 0;
            }
        }
    }
}
