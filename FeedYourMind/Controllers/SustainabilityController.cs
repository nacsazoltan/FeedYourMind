using Microsoft.AspNetCore.Mvc;
using OkosDobozWeb.Models;
using OkosDobozWeb.Services;
using System.Globalization;

namespace OkosDobozWeb.Controllers
{
    public class SustainabilityController : Controller
    {
        private readonly ContentService _contentService;

        public SustainabilityController(ContentService contentService)
        {
            _contentService = contentService;
        }

        public IActionResult Index(string category = "environment")
        {
            var culture = CultureInfo.CurrentUICulture.Name;

            var viewModel = new HomeViewModel
            {
                CurrentTopic = category,
                FilteredTexts = _contentService.GetSustainabilityTexts(culture, category),
                FilteredVideos = _contentService.GetSustainabilityVideos(culture, category),
                FilteredExercises = _contentService.GetSustainabilityExercises(culture, category)
            };

            ViewData["CurrentCategory"] = category;
            return View("Index", viewModel);
        }
    }
}
