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

        public List<SelectListItem> statusLegends { get; set; } = new();
        public List<SelectListItem> severityLevels { get; set; } = new();
        public long? severityLevelId { get; set; } = default!;
        public string DescriptionIssue { get; set; } = default!;
    }

    public class IncidentCellerInformationViewModel
    {
        public string CallerName { get; set; } = default!;
        public string CallerPhoneNumber { get; set; } = default!;
        public string CallerAddress { get; set; } = default!;
        public List<SelectListItem> Relationships { get; set; } = new();
        public long? RelationshipId { get; set; } = default!;
        public DateTime? CallTime { get; set; } = default!;
    }
    public class IncidentiLocationViewModel
    {
        public string Address { get; set; } = default!;
        public string Landmark { get; set; } = default!;
        public string ServiceAccount { get; set; } = default!;
        public string AssetIDs { get; set; } = default!;
        public List<SelectListItem> AssetsIncidentList { get; set; } = new();
    }
    public class IncidentDetailsViewModel
    {
        public long? EventTypeId { get; set; } = default!;
        public List<SelectListItem> EventTypes { get; set; } = new();
    }
    public class IncidentEnvironmentalViewModel
    {
        public long? GasodorpresentID { get; set; } = default!;
        public long? HissingSoundPresentID { get; set; } = default!;
        public long? VisibleDamageID { get; set; } = default!;
        public long? PeopleInjuredID { get; set; } = default!;
        public long? EvacuationRequiredID { get; set; } = default!;
        
    }
    public class IncidentSupportingInfoViewModel
    {
        public IFormFile File { get; set; }
        public string Notes { get; set; } = default!;
        public string? ImageUrl { get; set; } = default!;
    }
}
