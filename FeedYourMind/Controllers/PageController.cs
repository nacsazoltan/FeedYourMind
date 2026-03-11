using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using OkosDobozWeb.Services;

namespace OkosDobozWeb.Controllers;

public sealed class PageController : Controller
{
    private readonly IPageRepository _pageRepository;

    public PageController(IPageRepository pageRepository)
    {
        _pageRepository = pageRepository;
    }

    [HttpGet("/page/{page}")]
    public async Task<IActionResult> Index(string page, CancellationToken cancellationToken)
    {
        var culture = CultureInfo.CurrentUICulture.Name;
        var pageItem = await _pageRepository.GetPageAsync(culture, page, cancellationToken);
        if (pageItem is null)
        {
            return NotFound();
        }

        return View(pageItem);
    }
}
