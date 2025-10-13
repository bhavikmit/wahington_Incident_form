using Models.Models.Shared;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class IncidentValidationAssessment :BaseDBModel
    {
        [ForeignKey("IncidentId")]
        public long? IncidentId { get; set; }
        public Incident Incident { get; set; }

        [ForeignKey("IncidentValidationId")]
        public long? IncidentValidationId { get; set; }
        public IncidentValidation IncidentValidation { get; set; }

        public long? IC_MCR_AssignId { get; set; }
        public long? IC_MCR_StatusId { get; set; }

        public long? IC_Notify_AssignId { get; set; }
        public long? IC_Notify_StatusId { get; set; }

        public long? IC_EstablishICP_AssignId { get; set; }
        public long? IC_EstablishICP_StatusId { get; set; }

        public long? FER_PCA_AssignId { get; set; }
        public long? FER_PCA_StatusId { get; set; }

        public long? FER_LC_AssignId { get; set; }
        public long? FER_LC_StatusId { get; set; }

        public long? EGEC_RSM_AssignId { get; set; }
        public long? EGEC_RSM_StatusId { get; set; }

        public long? EGEC_MLP_AssignId { get; set; }
        public long? EGEC_MLP_StatusId { get; set; }

        public long? EGEC_ICT_AssignId { get; set; }
        public long? EGEC_ICT_StatusId { get; set; }
    }
}
