using Centangle.Common.ResponseHelpers.Models;

using Models.Common.Interfaces;

using Pagination;

using Repositories.Interfaces;

using ViewModels;
using ViewModels.Incident;
using ViewModels.Shared;

namespace Repositories.Common
{
    public interface IIncidentValidationService
    {
        Task<List<IncidentValidationPendingViewModel>> GetValidationPendingList();
        Task<List<RecentlyIncidentValidationViewModel>> GetRecentlyValidationList();
        Task<long> GetHighPriorityIncidentCount();
        Task<IncidentValidationViewModel> GetIncidentValidationAlarm(long id);
        Task<IncidentValidationDetailViewModel> GetIncidentValidationDetail(long id);
        Task<List<IncidentResponseTeamViewModel>> GetIncidentValidationResponseTeam();
        Task<List<IncidentPolicyViewModel>> GetIncidentValidationPolicy();
    }
}
