using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Enums;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Models.Common.Interfaces;
using ViewModels;
using ViewModels.Shared;

namespace Repositories.Common
{
    public class UsersinService<CreateViewModel, UpdateViewModel, DetailViewModel>
     : BaseService<IncidentUser, CreateViewModel, UpdateViewModel, DetailViewModel>,
       IUsersinService<CreateViewModel, UpdateViewModel, DetailViewModel>
     where DetailViewModel : class, IBaseCrudViewModel, new()
     where CreateViewModel : class, IBaseCrudViewModel, new()
     where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ModelStateDictionary _modelState;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<UsersinService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;

        public UsersinService(
            ApplicationDbContext db,
            ILogger<UsersinService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger,
            IMapper mapper,
            IRepositoryResponse response,
            IActionContextAccessor actionContext)
            : base(db, logger, mapper, response)
        {
            _modelState = actionContext.ActionContext.ModelState;
            _db = db;
            _logger = logger;
        }
        public async Task<List<UserModifyViewModel.TeamViewModel>> GetAllTeams()
        {
            try
            {
                var teams = await _db.IncidentTeams
                    .Where(t => !t.IsDeleted)
                    .Select(t => new UserModifyViewModel.TeamViewModel
                    {
                        TeamId = t.Id,
                        TeamName = t.Name
                    })
                    .ToListAsync();

                return teams;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetAllTeams.");
                return new List<UserModifyViewModel.TeamViewModel>();
            }
        }
        public async Task<List<UserModifyViewModel>> GetAllUsers()
        {
            var list = new List<UserModifyViewModel>();
            try
            {
                var users = await _db.IncidentUsers
                    .Where(t => !t.IsDeleted)
                    .ToListAsync();

                foreach (var t in users)
                {
                    // fetch team name using TeamId
                    var teamName = await _db.IncidentTeams
                        .Where(x => x.Id == t.TeamId)
                        .Select(x => x.Name)
                        .FirstOrDefaultAsync();

                    list.Add(new UserModifyViewModel
                    {
                        Id = t.Id,
                        TeamId = t.TeamId,
                        TeamName = teamName,
                        FirstName = t.FirstName,
                        LastName = t.LastName,
                        Telephone = t.Telephone,
                        Email = t.Email,
                        PinHash = t.PinHash
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetAllUsers.");
                return new List<UserModifyViewModel>();
            }

            return list;
        }

        public async Task<long> SaveUser(UserModifyViewModel viewModel)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var now = DateTime.UtcNow;
                var userId = 0L; // replace with actual user id from auth context if available

                var user = new IncidentUser
                {
                    TeamId = viewModel.TeamId,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Telephone = viewModel.Telephone,
                    Email = viewModel.Email,
                    PinHash = viewModel.PinHash,
                    IsDeleted = false,
                    ActiveStatus = ActiveStatus.Active,
                    CreatedOn = now,
                    CreatedBy = userId,
                    UpdatedOn = now,
                    UpdatedBy = userId
                };

                await _db.IncidentUsers.AddAsync(user);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return user.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error SaveUser.");
                return 0;
            }
        }

        public async Task<long> UpdateUser(UserModifyViewModel viewModel)
        {
            try
            {
                var user = await _db.IncidentUsers
                    .FirstOrDefaultAsync(t => t.Id == viewModel.Id);

                if (user == null)
                {
                    // if not found, create new
                    return await SaveUser(viewModel);
                }

                await using var transaction = await _db.Database.BeginTransactionAsync();

                user.TeamId = viewModel.TeamId;
                user.FirstName = viewModel.FirstName;
                user.LastName = viewModel.LastName;
                user.Telephone = viewModel.Telephone;
                user.Email = viewModel.Email;
                user.PinHash = viewModel.PinHash;
                user.UpdatedOn = DateTime.UtcNow;
                // set UpdatedBy if available

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

                return user.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error UpdateIncidentTeam.");
                return 0;
            }
        }

        public async Task<UserModifyViewModel> GetUserById(long id)
        {
            try
            {
                var user = await _db.IncidentUsers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (user == null)
                    return new UserModifyViewModel();

                return new UserModifyViewModel
                {
                    Id = user.Id,
                    TeamId = user.TeamId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Telephone = user.Telephone,
                    Email = user.Email,
                    PinHash = user.PinHash,
                    VerifyPIN = user.PinHash
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetUserById.");
                return new UserModifyViewModel();
            }
        }

        public async Task<long> DeleteUser(long id)
        {
            try
            {
                var user = await _db.IncidentUsers
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (user == null)
                    return 0;

                await using var transaction = await _db.Database.BeginTransactionAsync();

                user.IsDeleted = true;
                user.UpdatedOn = DateTime.UtcNow;
                // set UpdatedBy if available

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

                return user.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error DeleteIncidentTeam.");
                return 0;
            }
        }
    }
}
