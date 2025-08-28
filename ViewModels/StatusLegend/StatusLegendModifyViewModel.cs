using System.ComponentModel.DataAnnotations;
using Models.Common.Interfaces;
using ViewModels.Shared;
using Enums;
using ViewModels;

namespace ViewModels
{
    public class StatusLegendModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier
    {
        [Display(Name = "Status Legend", Prompt = "Status Legend")]
        public string Name { get; set; }

        [Display(Name = "Color", Prompt = "Color")]
        public string Color { get; set; }
    }
}
