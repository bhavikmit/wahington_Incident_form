using Centangle.Common.ResponseHelpers.Models;

using Models.Common.Interfaces;

using Pagination;

using Repositories.Interfaces;

using ViewModels;
using ViewModels.Incident;
using ViewModels.Shared;

namespace Repositories.Common
{
    public interface IIncidentService
    {
        Task<IncidentViewModel> GetIncidentDropDown();
        Task<string> SaveIncident(IncidentViewModel incidentViewModel);
        Task<List<IncidentGridViewModel>> GetIncidentList(FilterRequest request);
       // Task<string?> ChangeIncidentStatus(long incidenetID, string status);
        Task<IncidentViewModel> GetById(long incidentId);
        Task<IncidentViewModel> GetIncidentDetailsById(long incidentId);

        Task<string?> ChangeIncidentStatus(long incidentId, string statusId);
        
        Task<string> UpdateIncident(IncidentViewModel viewModel);
    }
}
