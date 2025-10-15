using Centangle.Common.ResponseHelpers.Models;

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
        Task<long> SaveValidationNoteAsync(SaveValidationNoteRequest request);
    }
}
