using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;

using Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewModels.Dashboard;

namespace ViewModels.Incident
{
    public class IncidentViewModel
    {
        public IncidentCellerInformationViewModel incidentCellerInformation { get; set; } = new();
        public IncidentiLocationViewModel incidentiLocation { get; set; } = new();
        public IncidentDetailsViewModel incidentDetails { get; set; } = new();
        public IncidentEnvironmentalViewModel incidentEnvironmentalViewModel { get; set; } = new();
        public IncidentSupportingInfoViewModel incidentSupportingInfoViewModel { get; set; } = new();
        public IncidentDetailByIdViewModel incidentDetailByIdViewModel { get; set; } = new();
        public List<IncidentGridViewModel> incidentGridViewModel { get; set; } = new();
        public IncidentValidationsDetailsViewModel incidentValidationsDetailsViewModel { get; set; } = new();
        public List<WorkStepViewModel> workSteps { get; set; } = new();
        public List<List<WorkStepViewModel>> workStepsByPolicy { get; set; } = new();

        public List<SelectListItem> statusLegends { get; set; } = new();
        public List<SelectListItem> severityLevels { get; set; } = new();
        public List<SelectListItem> progressLevels { get; set; } = new();
        public long? severityLevelId { get; set; } = default!;
        public long? Id { get; set; } = default!;
        public string DescriptionIssue { get; set; } = default!;

        public List<IncidentLocationMapViewModel> ListIncidentLocationMapViewModel { get; set; } = new();
        public AdditionalLocationViewModel additionalLocation { get; set; } = new();
        public List<AdditionalLocationViewModel> additionalLocations { get; set; } = new();

        public IncidentValidationAssignedRolesViewModel incidentValidationAssignedRolesViewModel { get; set; } = new();
        public IncidentValidationGatesViewModel incidentValidationGatesViewModel { get; set; } = new();
        public List<IncidentValidationLocationViewModel> IncidentValidationLocations { get; set; } = new();

        public List<IncidentMapChat> listIncidentMapChats { get; set; } = new();
    }

    public class IncidentCellerInformationViewModel
    {
        public string CallerName { get; set; } = default!;
        public string CallerPhoneNumber { get; set; } = default!;
        public string CallerAddress { get; set; } = default!;
        public List<SelectListItem> Relationships { get; set; } = new();
        public long? RelationshipId { get; set; } = default!;
        public DateTime CallTime { get; set; } = default!;
        public string RelationshipName { get; set; } = string.Empty;
        public string CallDateInFormat { get; set; } = string.Empty;
        public string CallTimeInFormat { get; set; } = string.Empty;
    }
    public class IncidentiLocationViewModel
    {
        public string Address { get; set; } = default!;
        public bool IsSameCallerAddress { get; set; } = default!;
        public string Landmark { get; set; } = default!;
        public string? ServiceAccount { get; set; } = default!;
        public string AssetIDs { get; set; } = default!;
        public List<SelectListItem> AssetsIncidentList { get; set; } = new();
        public List<string> AssetNames { get; set; } = new();
    }
    public class IncidentDetailsViewModel
    {
        public string EventTypeIds { get; set; } = default!;
        public bool IsOtherEvent { get; set; } = default!;
        public string OtherEventDetail { get; set; } = default!;
        public List<SelectListItem> EventTypes { get; set; } = new();
        public List<string> EventTypeNames { get; set; } = new();
    }
    public class IncidentEnvironmentalViewModel
    {
        public long? GasodorpresentID { get; set; } = default!;
        public long? WaterPresentID { get; set; } = default!;
        public long? HissingSoundPresentID { get; set; } = default!;
        public long? VisibleDamageID { get; set; } = default!;
        public long? PeopleInjuredID { get; set; } = default!;
        public long? EvacuationRequiredID { get; set; } = default!;
        public long? EmergencyResponseNotifiedID { get; set; } = default!;
        // ✅ Friendly
        public string GasOdorText { get; set; } = string.Empty;
        public string WaterPresentText { get; set; } = string.Empty;
        public string HissingSoundText { get; set; } = string.Empty;
        public string VisibleDamageText { get; set; } = string.Empty;
        public string PeopleInjuredText { get; set; } = string.Empty;
        public string EvacuationRequiredText { get; set; } = string.Empty;
        public string EmergencyResponseNotifiedText { get; set; } = string.Empty;
    }
    public class IncidentSupportingInfoViewModel
    {
        public List<IFormFile>? File { get; set; }
        public string Notes { get; set; } = default!;
        public string? ImageUrl { get; set; } = default!;
        public List<string> ImageUrls { get; set; } = new();
    }

    public class IncidentDetailByIdViewModel
    {
        public string SeverityName { get; set; } = string.Empty;
        public string StatusLegendName { get; set; } = string.Empty;
        public string StatusLegendColor { get; set; } = string.Empty;
        public string SeverityColor { get; set; } = string.Empty;
        public string IncidentNumber { get; set; } = string.Empty;
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string CreatedOnDate { get; set; } = string.Empty;
        public string CreatedOnTime { get; set; } = string.Empty;
    }

    public class IncidentGridViewModel
    {
        public long Id { get; set; }
        public long? StatusLegendId { get; set; }
        public string StatusLegend { get; set; }
        public string StatusLegendColor { get; set; }
        public string CallDate { get; set; }
        public string CallTime { get; set; }
        public string LocationAddress { get; set; }
        public string Intersection { get; set; }
        public string AssetId { get; set; }
        public string EventType { get; set; }
        public string EventTypeId { get; set; }
        public long? SeverityId { get; set; }
        public string Severity { get; set; }

        public long? RelationShipId { get; set; }
        public string RelationShipName { get; set; }
        public string DescriptionIssue { get; set; }
        public string GasESIndicator { get; set; }
        public int AdditionalLocationCount { get; set; } = 0;
    }
    public class ChangeStatusRequest
    {
        public long IncidentId { get; set; }
        public string Status { get; set; }
    }
    public class FilterRequest
    {
        public long severityId { get; set; }
        public long statusId { get; set; }
        public string description { get; set; }
    }

    public class GeocodeResult
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class SaveCommunicationRequest
    {
        public long IncidentId { get; set; }
        public long IncidentValidationId { get; set; }
        public string Message { get; set; }
        public string ImageUrl { get; set; }
        public long MessageType { get; set; }
        public List<IFormFile> File { get; set; }
    }

    public class IncidentValidationsDetailsViewModel
    {
        public long IncidentValidationId { get; set; }
        public long ConfirmedSeverityLevelId { get; set; }
        public long DiscoveryPerimeterId { get; set; }
        public string DiscoveryPerimeterName { get; set; }
        public string AssignResponseTeams { get; set; }
        public string ValidationNotes { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedDateInFormat { get; set; }
        public string CreatedTimeInFormat { get; set; }
        public string SeverityLevelName { get; set; }
        public string SeverityLevelColor { get; set; }
        public List<IncidentValidationCommunicationHistoriesViewModel> IncidentValidationCommunicationHistoriesViewModelList { get; set; }
        public List<IncidentValidationNoteViewModel> IncidentValidationNotesList { get; set; } = new();
    }

    public class IncidentValidationCommunicationHistoriesViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string TimeStamp { get; set; } = string.Empty;
        public string ReceipientsIds { get; set; } = string.Empty;
        private string _imageUrl = string.Empty;
        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                _imageUrl = value ?? string.Empty;
                Image = string.IsNullOrWhiteSpace(_imageUrl)
                    ? string.Empty
                    : Path.GetFileName(_imageUrl);
            }
        }

        // new property that auto-extracts from ImageUrl
        public string Image { get; private set; } = string.Empty;

        public long MessageType { get; set; }
    }

    public class WorkStepViewModel
    {
        public long PolicyId { get; set; }
        public string PolicyName { get; set; } = string.Empty;
        public string TeamName { get; set; } = string.Empty;
        public long TeamId { get; set; }

        // existing comma separated string
        public string PolicySteps { get; set; } = string.Empty;

        // new property: auto-splits PolicySteps into list
        public List<string> Steps
        {
            get
            {
                if (string.IsNullOrWhiteSpace(PolicySteps))
                    return new List<string>();

                return PolicySteps
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList();
            }
        }

        public string TeamsByPolicy { get; set; } = string.Empty;
    }

    public class AdditionalLocationViewModel
    {

        public long? Id { get; set; } = default!;
        public long? IncidentId { get; set; } = default!; 
        public string LocationAddress { get; set; } = default!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? NearestIntersection { get; set; }
        public string? ServiceAccount { get; set; } = default!;
        public bool PerimeterType { get; set; }
        public bool IsPrimaryLocation { get; set; }
        public long? PerimeterTypeDigit { get; set; }
        public string AssetIDs { get; set; } = default!;
        public List<SelectListItem> AssetsIncidentList { get; set; } = new();
        public List<string> AssetNames { get; set; } = new();
    }

    public class IncidentAdditionalLocationViewModel
    {
        public long Id { get; set; } = default!;
        public long? IncidentId { get; set; } = default!;
        public string AdditionalLocation { get; set; } = default!;
        public double Lat { get; set; } = default!;
        public double Long { get; set; } = default!;
        public bool IsPrimaryLocation { get; set; } = default!;
    }

    public class IncidentLocationValidationViewModel
    {
        public long IncidentId { get; set; }
        public long IncidentValidationId { get; set; }
        public long IncidentLocationId { get; set; }
        public long SeverityLevelId { get; set; }
        public long DiscoveryPerimeterId { get; set; }
        public long ResponseTeamId { get; set; }
        public string TeamMemberIds { get; set; } = default!;
        public string LocationSpecificNotes { get; set; } = default!;
        public List<IncidentWorkStepViewModel> WorkSteps { get; set; } = new();
    }

    public class IncidentWorkStepViewModel
    {
        public string WorkStepName { get; set; } = default!;
        public string WorkStepDescription { get; set; } = default!;
        public string WorkStepSpecificPersion { get; set; } = default!;
    }

    public class IncidentValidationAssignedRolesViewModel
    {
        public long Id { get; set; }
        public long IncidentId { get; set; }
        public long IncidentValidationId { get; set; }
        public long? IncidentCommanderId { get; set; }
        public long? FieldEnvRepId { get; set; }
        public long? GEC_CoordinatorId { get; set; }
        public long? EngineeringLeadId { get; set; }

        public string? IncidentCommanderName { get; set; }
        public string? FieldEnvRepName { get; set; }
        public string? GEC_CoordinatorName { get; set; }
        public string? EngineeringLeadName { get; set; }
    }

    public class IncidentValidationGatesViewModel
    {
        public long Id { get; set; }
        public long IncidentId { get; set; }
        public long IncidentValidationId { get; set; }
        public string ContainmentAcknowledgement { get; set; } = string.Empty;
        public string Exception { get; set; } = string.Empty;
        public string IndependentInspection { get; set; } = string.Empty;
        public string Regulatory { get; set; } = string.Empty;
    }
    public class IncidentValidationNoteViewModel
    {
        public long Id { get; set; }
        public long? IncidentId { get; set; }
        public long? IncidentValidationId { get; set; }
        public string Notes { get; set; }
        public bool IsDeleted { get; set; }
        public int ActiveStatus { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
    }
}
