using Microsoft.AspNetCore.Mvc;

namespace VehicleAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
