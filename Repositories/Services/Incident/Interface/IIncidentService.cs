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
    }
}
