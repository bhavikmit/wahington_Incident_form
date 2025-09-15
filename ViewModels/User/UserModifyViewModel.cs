using Models.Common.Interfaces;
using System.ComponentModel;
using ViewModels.Shared;

namespace ViewModels
{
    public class UserModifyViewModel : BaseUpdateVM, IBaseCrudViewModel, IIdentitifier
    {
        public long TeamId { get; set; }
        public long Id { get; set; }
        //public string Name { get => string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName) ? string.IsNullOrEmpty(Username) ? "-" : $"{Username}" : $"{FirstName} {LastName}"; set { } }

        [DisplayName("FirstName")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Telephone")]
        public string Telephone { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("PinHash")]
        public string PinHash { get; set; }
        public string VerifyPIN { get; set; }
        public string TeamName { get; set; }

        public class TeamViewModel
        {
            public long TeamId { get; set; }
            public string TeamName { get; set; }
        }
        public List<TeamViewModel> Teams { get; set; } = new List<TeamViewModel>();
    }
}
