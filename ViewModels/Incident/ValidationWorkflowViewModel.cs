using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Incident
{

    public class BaseIncidentValidationViewModel
    {
        public long Id { get; set; }
        public string IncidentId { get; set; } = default!;
    }

    public class ValidationWorkflowViewModel : BaseIncidentValidationViewModel
    {
        public IncidentValidationCountViewModel IVCount { get; set; } = new();
        public IncidentValidationDetailViewModel IVDetails { get; set; } = new();
        public IncidentValidationViewModel IVValidation { get; set; } = new();
        public List<IncidentValidationPendingViewModel> IVPendingList { get; set; } = new();
        public List<RecentlyIncidentValidationViewModel> IVRecentlyList { get; set; } = new();
        public List<IncidentResponseTeamViewModel> IVResponseTeamList { get; set; } = new();
    }

    public class IncidentValidationCountViewModel : BaseIncidentValidationViewModel
    {
        public long PendingValidationCount { get; set; } = default!;
        public long TodayValidationCount { get; set; } = default!;
        public long HighSeverityCount { get; set; } = default!;
    }

    public class IncidentValidationPendingViewModel : BaseIncidentValidationViewModel
    {
        public string EventType { get; set; } = default!;
        public string IncidentLocation { get; set; } = default!;
        public string Severity { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string IncidentDate { get; set; } = default!;
    }
    public class RecentlyIncidentValidationViewModel : BaseIncidentValidationViewModel
    {
        public string EventType { get; set; } = default!;
        public string Status { get; set; } = default!;
        public string StatusColor { get; set; } = default!;
        public string IncidentDate { get; set; } = default!;
    }
    public class IncidentValidationDetailViewModel : BaseIncidentValidationViewModel
    {
        public string CallerName { get; set; } = default!;
        public string CallerContact { get; set; } = default!;
        public string CallerAddress { get; set; } = default!;
        public DateTime CallerDateTime { get; set; } = default!;
        public string IncidentLocation { get; set; } = default!;
        public string NearestIntersection { get; set; } = default!;
        public string EventType { get; set; } = default!;
        public string Severity { get; set; } = default!;
        public string IncidentStatus { get; set; } = default!;
        public List<string> AffectedAssets { get; set; } = default!;
        public List<SafetyAssessment> SafetyAssessments { get; set; } = default!;
    }

    public class SafetyAssessment
    {
        public string Name { get; set; } = default!;
        public string AssetStatus { get; set; } = default!;
    }

    public class IncidentValidationViewModel
    {
        public List<SelectListItem> severityLevels { get; set; } = new();
        public long? severityLevelId { get; set; } = default!;
        public decimal? DiscoveryPerimeter { get; set; } = default!;
        public string ValidationNotes { get; set; } = default!;
        public string IncidentLocation { get; set; } = default!;
    }

    public class IncidentResponseTeamViewModel
    {
        public string Name { get; set; } = default!;
        public string Tag { get; set; } = default!;
        public string Contact { get; set; } = default!;
        public List<string> Specializations { get; set; } = default!;
    }

    public class IncidentPolicyViewModel
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}
