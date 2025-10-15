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
            AssestmentFilterRequest request = new()
            {
                IncidentId = id
            };

            var model = await _iIncidentService.GetIncidentDetailsById(id);
            model.ListIncidentLocationMapViewModel = await _iIncidentService.GetIncidentMapDetailsbyId(id);
            model.listIncidentMapChats = await _iIncidentService.GetIncidentMapChatChat(id);
            model.IncidentAssessmentDetails = await _iIncidentService.GetAssessmentDetails(request);

            #region Personnel
            var companies = await _iIncidentService.GetAllCompanies();
            ViewBag.Companies = new SelectList(companies, "CompanyId", "CompanyName");
            var roles = await _iIncidentService.GetAllIncidentRoles();
            ViewBag.Roles = new SelectList(roles, "IncidentRoleId", "IncidentRoleName");
            #endregion

            return View(model);
        }


        #region Map
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
        #endregion

        #region Assestment
        [HttpPost]
        public async Task<PartialViewResult> GetAssessmentDetails([FromBody] AssestmentFilterRequest request)
        {
            var model = await _iIncidentService.GetAssessmentDetails(request);
            return PartialView("_IncidentAssessmentDetailsPartial", model);
        }

        [HttpGet]
        public async Task<PartialViewResult> EditAssessmentDetails(long id, long mainstepId, long substepId)
        {
            var model = await _iIncidentService.EditAssessmentDetails(id, mainstepId, substepId);
            return PartialView("_UpdateAssestmentPartial", model);
        }

        [HttpGet]
        public async Task<PartialViewResult> ViewAssessmentDetails(long id, long mainstepId, long substepId)
        {
            var model = await _iIncidentService.ViewAssessmentDetails(id, mainstepId, substepId);
            return PartialView("_ViewAssestmentPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAssessment(IncidentAssessmentEditViewModel model, List<IFormFile> Files)
        {
            try
            {
                var fileUrls = new List<string>();

                if (Files != null && Files.Count > 0)
                {
                    var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Storage", "uploads", "Assessment");

                    if (!Directory.Exists(uploadsPath))
                        Directory.CreateDirectory(uploadsPath);

                    foreach (var file in Files)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                            var filePath = Path.Combine(uploadsPath, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            // Relative URL for use in browser
                            var relativeUrl = $"/Storage/uploads/Assessment/{fileName}";
                            fileUrls.Add(relativeUrl);
                        }
                    }
                    model.ImageUrl = string.Join(",", fileUrls);
                }

                long id = await _iIncidentService.UpdateAssessment(model);

                if (id > 0)
                {
                    AssestmentFilterRequest request = new()
                    {
                        IncidentId = (long)model.IncidentId
                    };
                    var details = await _iIncidentService.GetAssessmentDetails(request);
                    return Json(new { success = true, files = fileUrls, asssetDetails = details });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { success = false, message = "Failed to Update Assessment." });
                }
            }
            catch (Exception ex)
            {
                // log ex here if needed
                return Json(new { success = false, message = "Save failed. " + ex.Message });
            }
        }
        #endregion


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

        [HttpPost]
        public async Task<JsonResult> SaveValidationNote([FromBody] SaveValidationNoteRequest request)
        {
            if (request == null || request.IncidentId <= 0 || string.IsNullOrWhiteSpace(request.Notes))
                return Json(new { success = false, message = "Invalid request." });

            try
            {
                var id = await _iIncidentService.SaveValidationNoteAsync(request);
                if (id > 0)
                    return Json(new { success = true, id, message = "Saved" });

                return Json(new { success = false, message = "Save failed." });
            }
            catch (Exception ex)
            {
                // log if you have logger
                return Json(new { success = false, message = "Error saving note." });
            }
        }

        [HttpPost]
        //public async Task<IActionResult> SavePostDetails([FromForm] IncidentViewModel incidentViewModel)
        public async Task<IActionResult> SavePostDetails([FromForm] IncidentViewPostViewModel incidentViewPostViewModel)
        {
            //if (incidentViewModel == null)
            //    return BadRequest(new { success = false, message = "Invalid request data." });

            try
            {
                //var incidentId = string.Empty;
                //if (incidentViewModel.Id > 0)
                //{
                //    incidentId = await _iIncidentService.UpdateIncident(incidentViewModel);
                //}
                //else
                //{
                //    incidentId = await _iIncidentService.SaveIncident(incidentViewModel);
                //}
                //if (string.IsNullOrWhiteSpace(incidentId))
                //    return StatusCode(StatusCodes.Status500InternalServerError,
                //        new { success = false, message = "Failed to save incident." });

                //var successMsg = $"Incident {incidentId} saved successfully!";

                //return Ok(new { success = true, data = successMsg });
                return Ok(new { success = true, data = "stromg " });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurred." });
            }
        }

    }
}
