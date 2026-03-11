using Microsoft.AspNetCore.Mvc;
using OkosDobozWeb.Models;
using OkosDobozWeb.Services;
using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace OkosDobozWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ContentService _contentService;

        public HomeController(ContentService contentService)
        {
            _contentService = contentService;
        }

        public IActionResult Index(string topic = "basic")
        {
            var culture = CultureInfo.CurrentUICulture.Name;

            var viewModel = new HomeViewModel
            {
                CurrentTopic = topic,
                FilteredTexts = _contentService.GetTexts(culture, topic),
                FilteredVideos = _contentService.GetVideos(culture, topic),
                FilteredExercises = _contentService.GetExercises(culture, topic)
            };

            return View(viewModel);
        }

        // This action sets the language cookie
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl = "/")
        {
            if (string.IsNullOrEmpty(culture) || (culture != "hu" && culture != "en" && culture != "cs"))
            {
                culture = "hu"; // Default to Hungarian if invalid
            }

            var cookieName = CookieRequestCultureProvider.DefaultCookieName;
            var cookieValue = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture));

            Response.Cookies.Append(
                cookieName,
                cookieValue,
                new CookieOptions 
                { 
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                    Path = "/"
                }
            );

            // Return to the previous page, or home if returnUrl is empty
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}