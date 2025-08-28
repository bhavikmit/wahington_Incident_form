using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
   
    public class IncidentsController : Controller
        {

        public ActionResult Index()
        {
            return View();
        }
    }
}