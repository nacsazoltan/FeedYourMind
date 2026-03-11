using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace OkosDobozWeb.Controllers
{
    public class LandingController : Controller
    {
        public IActionResult Index(string category = "healthy")
        {
            ViewData["CurrentCategory"] = category;
            return View();
        }
    }
}
