using AutoMapper;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using Enums;
using Microsoft.AspNetCore.Identity;
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
    public class UserManagementService<CreateViewModel, UpdateViewModel, DetailViewModel>
 : BaseService<IncidentUser, CreateViewModel, UpdateViewModel, DetailViewModel>,
   IUserManagementService<CreateViewModel, UpdateViewModel, DetailViewModel>
 where DetailViewModel : class, IBaseCrudViewModel, new()
 where CreateViewModel : class, IBaseCrudViewModel, new()
 where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ModelStateDictionary _modelState;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<UserManagementService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;

        public UserManagementService(
            ApplicationDbContext db,
            ILogger<UserManagementService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger,
            IMapper mapper,
            IRepositoryResponse response,
            IActionContextAccessor actionContext)
            : base(db, logger, mapper, response)
        {
            _modelState = actionContext.ActionContext.ModelState;
            _db = db;
            _logger = logger;
        }
        public async Task<List<UserManagementModifyViewModel.RoleViewModel>> GetAllRoles()
        {
            try
            {
                var roles = await _db.Roles
                    .Where(t => !t.IsDeleted)
                    .Select(t => new UserManagementModifyViewModel.RoleViewModel
                    {
                        RoleId = t.Id,
                        RoleName = t.Name
                    })
                    .ToListAsync();

                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetAllRoles.");
                return new List<UserManagementModifyViewModel.RoleViewModel>();
            }
        }
        public async Task<List<UserManagementModifyViewModel>> GetAllUserManagement()
        {
            var list = new List<UserManagementModifyViewModel>();
            try
            {
                var users = await _db.Users.ToListAsync();

                foreach (var t in users)
                {
                    //fetch team name using TeamId
                    var roleName = await _db.UserRoles
                        .Where(ur => ur.UserId == t.Id)
                        .Join(_db.Roles,
                              ur => ur.RoleId,
                              r => r.Id,
                              (ur, r) => r.Name)
                        .FirstOrDefaultAsync();

                    list.Add(new UserManagementModifyViewModel
                    {
                        Id = t.Id,
                        FirstName = t.FirstName,
                        Email = t.Email,
                        RoleName = roleName,
                        Department = t.Department,
                        PhoneNumber = t.PhoneNumber,
                        Status = t.ActiveStatus.ToString(),
                        LastLogin = DateTime.UtcNow.ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetAllUsers.");
                return new List<UserManagementModifyViewModel>();
            }

            return list;
        }

        public async Task<long> SaveUserManagement(UserManagementModifyViewModel viewModel)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var now = DateTime.UtcNow;

                var user = new ApplicationUser
                {
                    FirstName = viewModel.FirstName,
                    Email=viewModel.Email,
                    UserName = viewModel.Email,
                    NormalizedUserName =viewModel.Email.ToUpper(),
                    NormalizedEmail=viewModel.Email.ToUpper(),
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    EmailConfirmed = true,
                    ActiveStatus = viewModel.ActiveStatus,
                    Department = viewModel.Department,
                };

                var hasher = new PasswordHasher<ApplicationUser>();
                user.PasswordHash = hasher.HashPassword(user, "123456");


                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                var userRole = new IdentityUserRole<long>
                {
                    UserId = user.Id,        
                    RoleId = viewModel.RoleId
                };

                _db.UserRoles.Add(userRole);
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

        public async Task<long> UpdateUserManagement(UserManagementModifyViewModel viewModel)
        {
            try
            {
                var user = await _db.Users
                    .FirstOrDefaultAsync(t => t.Id == viewModel.Id);

                if (user == null)
                {
                    // if not found, create new
                    return await SaveUserManagement(viewModel);
                }

                await using var transaction = await _db.Database.BeginTransactionAsync();

                //user.TeamId = viewModel.TeamId;
                //user.FirstName = viewModel.FirstName;
                //user.LastName = viewModel.LastName;
                //user.Telephone = viewModel.Telephone;
                //user.Email = viewModel.Email;
                //user.PinHash = viewModel.PinHash;
                //user.UpdatedOn = DateTime.UtcNow;
                //// set UpdatedBy if available

                //try
                //{
                //    await _db.SaveChangesAsync();
                //    await transaction.CommitAsync();
                //}
                //catch
                //{
                //    await transaction.RollbackAsync();
                //    throw;
                //}

                return user.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error UpdateIncidentTeam.");
                return 0;
            }
        }

        public async Task<UserManagementModifyViewModel> GetUserManagementById(long id)
        {
            try
            {
                var user = await _db.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (user == null)
                    return new UserManagementModifyViewModel();

                //return new UserManagementModifyViewModel
                //{
                //    Id = user.Id,
                //    TeamId = user.TeamId,
                //    FirstName = user.FirstName,
                //    LastName = user.LastName,
                //    Telephone = user.Telephone,
                //    Email = user.Email,
                //    PinHash = user.PinHash,
                //    VerifyPIN = user.PinHash
                //};
                return new UserManagementModifyViewModel();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetUserManagementById.");
                return new UserManagementModifyViewModel();
            }
        }

        public async Task<long> DeleteUserManagement(long id)
        {
            try
            {
                var user = await _db.Users
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (user == null)
                    return 0;

                await using var transaction = await _db.Database.BeginTransactionAsync();

                user.IsDeleted = true;
                user.ActiveStatus = 0;
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
                _logger.LogError(ex, "Error DeleteUserManagement.");
                return 0;
            }
        }
    }
}
