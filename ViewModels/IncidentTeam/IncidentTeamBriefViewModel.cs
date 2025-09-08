using Select2.Model;
using System.ComponentModel;

namespace ViewModels
{
    public class IncidentTeamBriefViewModel : BaseSelect2VM, ISelect2BaseVM
    {
        public IncidentTeamBriefViewModel() : base(true, "The Incident Team field is required.")
        {

        }

        public IncidentTeamBriefViewModel(bool isValidationEnabled, string errorMessage) : base(isValidationEnabled, errorMessage)
        {

        }

        [DisplayName("Incident Team")]
        public string? Name { get; set; }

        public override string? Select2Text
        {
            get
            {
                return Name;
            }
        }
    }
}