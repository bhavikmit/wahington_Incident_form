using Microsoft.AspNetCore.Mvc;

using Repositories.Common;

using ViewModels;

namespace Web.Controllers
{
    public class SettingsController : Controller
    {
        IRelationshipService<RelationshipModifyViewModel, RelationshipModifyViewModel, RelationshipDetailViewModel> _iRelationshipService;
        public SettingsController(IRelationshipService<RelationshipModifyViewModel, RelationshipModifyViewModel, RelationshipDetailViewModel> iRelationshipService)
        {
            _iRelationshipService = iRelationshipService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRelationships()
        {
            var model = await _iRelationshipService.GetAllRelationships();
            return PartialView("~/Views/Settings/Source/_ListSource.cshtml", model);
        }
    }
}
