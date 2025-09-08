using System.ComponentModel.DataAnnotations;
using Models.Common.Interfaces;
using ViewModels.Shared;
using Enums;
using ViewModels;

namespace ViewModels
{
    public class IncidentTeamModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier
    {

        [Display(Name = "Team", Prompt = "Team name")]
        public string Name { get; set; }
    }
}
