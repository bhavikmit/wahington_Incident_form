using Centangle.Common.ResponseHelpers.Models;

using DocumentFormat.OpenXml.Drawing.Spreadsheet;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.Common.Interfaces;

using Pagination;

using Repositories.Interfaces;

using ViewModels;
using ViewModels.Dashboard;
using ViewModels.Incident;
using ViewModels.Shared;

using static ViewModels.Incident.IncidentViewModel;

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
        Task<List<SelectListItem>> GetAllUsersDrop();
        Task<List<SelectListItem>> GetAllCompaniesDrop();
        Task<List<SelectListItem>> GetAllIncidentRolesDrop();
        Task<List<SelectListItem>> GetAllShiftsDrop();
        Task<List<IncidentViewModel.CompanyViewModel>> GetAllCompanies();
        Task<List<IncidentViewModel.IncidentRoleViewModel>> GetAllIncidentRoles();
        Task<IncidentValidationPersonnelsCountViewModel> UpdateTimeIn(long id, DateTime timeIn);
        Task<List<IncidentViewModel.ProgressStatusViewModel>> GetAllProgressStatus();
        //Task<long> UpdateTimeIn(long id, DateTime timeIn);
        Task<List<IncidentValidationPersonnelsViewModel>> GetFilterByRole(long incidentId, long roleId, long companyid, string onsite);
        Task<List<IncidentViewModel.UsersViewModel>> GetSupervisors(long companyId, long userId);
        Task<long> UpdateSupervisor(long personnelId, long supervisorId);
        #endregion
        Task<IncidentAssessmentDetailViewModel> GetAssessmentDetails(AssestmentFilterRequest request);
        Task<IncidentAssessmentEditViewModel> EditAssessmentDetails(long id, long mainstepId, long substepId);
        Task<long> SaveValidationNoteAsync(SaveValidationNoteRequest request);
        Task<long> UpdateAssessment(IncidentAssessmentEditViewModel request);
        Task<IncidentAssessmentReadViewModel> ViewAssessmentDetails(long id, long mainstepId, long substepId);
        Task<IncidentViewAssessmentAttachmentViewModel> ViewAssessmentAttachment(long id);
        Task<long> AddPerson(long userId, long companyId, long roleId, long shiftId, long incidentId, long incidentValidationId);
        Task<List<IncidentViewPostViewModel>> SavePostDetails(IncidentViewPostViewModel incidentViewPostViewModel);
        Task<IncidentAssessmentAddViewModel> AddAssessmentDetails();
        Task<long> SubmitAssestment(IncidentValidationAssessment request);
        Task<IncidentViewTaskListViewModel> AddIncidentTaskAsync(AddIncidentTaskRequest request);


    }
}
