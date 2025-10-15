using Centangle.Common.ResponseHelpers.Models;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Common.Interfaces;

using Pagination;

using Repositories.Interfaces;

using ViewModels;
using ViewModels.Dashboard;
using ViewModels.Incident;
using ViewModels.Shared;

namespace Repositories.Common
{
    public interface IIncidentService
    {
        Task<IncidentViewModel> GetIncidentDropDown();
        Task<string> SaveIncident(IncidentViewModel incidentViewModel);
        Task<List<IncidentGridViewModel>> GetIncidentList(FilterRequest request);
        Task<string?> ChangeIncidentStatus(long incidenetID, string status);
        Task<IncidentViewModel> GetById(long incidentId);
        Task<IncidentViewModel> GetIncidentDetailsById(long incidentId);
        Task<string> UpdateIncident(IncidentViewModel viewModel);
        Task<List<IncidentLocationMapViewModel>> GetIncidentMapDetailsbyId(long incidentId);
        Task<bool> SaveCommunicationMessage(SaveCommunicationRequest request);
        Task<List<AdditionalLocationViewModel>> GetAdditionalLocationsByIncidentId(long incidentId);
        Task<long> AddMapChat(IncidentMapChatRequest request);

        Task<List<IncidentMapChat>> GetIncidentMapChatChat(long incidentId);

        #region Personnel
        Task<List<IncidentViewModel.CompanyViewModel>> GetAllCompanies();
        Task<List<IncidentViewModel.IncidentRoleViewModel>> GetAllIncidentRoles();
        Task<long> UpdateTimeIn(long id, DateTime timeIn);
        Task<List<IncidentValidationPersonnelsViewModel>> GetFilterByRole(long incidentId, long roleId, long companyid, string onsite);
        Task<List<IncidentViewModel.UsersViewModel>> GetSupervisors(long companyId, long userId);
        Task<long> UpdateSupervisor(long personnelId, long supervisorId);
        #endregion
    }
}
