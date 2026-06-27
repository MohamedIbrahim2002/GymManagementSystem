using GymManagment.BLL.Services.Interfaces;
using GymManagment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GymManagment.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
            private readonly ILogger<HomeController> _logger;
        private readonly IAnalaticServices _analaticServices;

        public HomeController(ILogger<HomeController> logger , IAnalaticServices analaticServices )
        {
                _logger = logger;
                _analaticServices = analaticServices;
        }

            public async Task<IActionResult> Index(CancellationToken ct)
            {
                 var data = await _analaticServices.GetAnalaticDataAsync(ct);
                return View(data);
            }

            public IActionResult Privacy()
            {
                return View();
            }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

        }
}
