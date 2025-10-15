using Microsoft.AspNetCore.Mvc;
using Repositories.Common;
using ViewModels.Incident;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace Web.Controllers
{
    public class IncidentDetailController : Controller
    {
        private readonly IIncidentService _iIncidentService;

        public object JsonRequestBehavior { get; private set; }

        public IncidentDetailController(IIncidentService incidentService)
        {
            _iIncidentService = incidentService;
        }
        public async Task<IActionResult> Index(long id)
        {
            var model = await _iIncidentService.GetIncidentDetailsById(id);
            model.ListIncidentLocationMapViewModel = await _iIncidentService.GetIncidentMapDetailsbyId(id);
            model.listIncidentMapChats = await _iIncidentService.GetIncidentMapChatChat(id);

            #region Personnel
            var companies = await _iIncidentService.GetAllCompanies();
            ViewBag.Companies = new SelectList(companies, "CompanyId", "CompanyName");
            var roles = await _iIncidentService.GetAllIncidentRoles();
            ViewBag.Roles = new SelectList(roles, "IncidentRoleId", "IncidentRoleName");
            #endregion

            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> AddMapChat([FromBody] IncidentMapChatRequest request)
        {
            if (request == null)
            {
                return Json(new { success = false, message = "Invalid request." });
            }

            try
            {
                long id = await _iIncidentService.AddMapChat(request);
                if (id > 0)
                {
                    return Json(new { success = true, message = "Success" });
                }
                return Json(new { success = false, message = "Failed to delete location." });
            }
            catch (Exception ex)
            {
                // you already have _logger in controller? If not, use try/catch and return generic
                return Json(new { success = false, message = "Error delete location." });
            }
        }

        #region Personnel
        [HttpPost]
        public async Task<IActionResult> UpdateTimeIn(long id, DateTime timeIn)
        {
            try
            {
                var Id = await _iIncidentService.UpdateTimeIn(id, timeIn);

                if (Id == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { success = false, message = "Failed to Update TimeIn." });

                var successMsg = "Update TimeIn successfully!";
                return Ok(new { success = true, data = successMsg });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurred." });
            }
        }
        [HttpGet]
        public IActionResult FilterByRole(long incidentId, long roleId, long companyid, string onsite)
        {
            try
            {
                var response = _iIncidentService.GetFilterByRole(incidentId, roleId, companyid, onsite);
                // Simply return Ok() with JSON data
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurred." });
            }
        }
        [HttpGet]
        public IActionResult GetSupervisors(long companyId, long userId)
        {
            try
            {
                var response = _iIncidentService.GetSupervisors(companyId, userId);
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurred." });
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSupervisor(long personnelId, long supervisorId)
        {
            try
            {
                var Id = await _iIncidentService.UpdateSupervisor(personnelId, supervisorId);

                if (Id == 0)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { success = false, message = "Failed to Update Supervisor." });

                var successMsg = "Update Supervisor successfully!";
                return Ok(new { success = true, data = successMsg });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurred." });
            }
        }
        #endregion


    }
}
