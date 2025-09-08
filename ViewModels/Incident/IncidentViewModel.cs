using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<SelectListItem> statusLegends { get; set; } = new();
        public List<SelectListItem> severityLevels { get; set; } = new();
        public long? severityLevelId { get; set; } = default!;
        public long? Id { get; set; } = default!;
        public string DescriptionIssue { get; set; } = default!;
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
        public long? HissingSoundPresentID { get; set; } = default!;
        public long? VisibleDamageID { get; set; } = default!;
        public long? PeopleInjuredID { get; set; } = default!;
        public long? EvacuationRequiredID { get; set; } = default!;
        // ✅ Friendly
        public string GasOdorText { get; set; } = string.Empty;
        public string HissingSoundText { get; set; } = string.Empty;
        public string VisibleDamageText { get; set; } = string.Empty;
        public string PeopleInjuredText { get; set; } = string.Empty;
        public string EvacuationRequiredText { get; set; } = string.Empty;
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
}
