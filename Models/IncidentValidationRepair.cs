using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models.Shared;

namespace Models
{
    public class IncidentValidationRepair : BaseDBModel
    {
        [Required]
        [ForeignKey("Incident")]
        public long IncidentId { get; set; }

        [Required]
        [ForeignKey("IncidentValidation")]
        public long IncidentValidationId { get; set; }
        public string? SourceOfLeak { get; set; }
        public string? SourceOfLeakStatus { get; set; }
        public string? PreventFurtherOutage { get; set; }
        public string? PreventFurtherOutageStatus { get; set; }
        public string? VacuumTruckFitting { get; set; }
        public string? VacuumTruckFittingStatus { get; set; }

    }
}
