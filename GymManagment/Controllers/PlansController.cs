using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.ViewModel.Plan;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Repositories.Classes;
using GymManagment.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagment.Controllers
{
    [Authorize]

    public class PlansController : Controller
    {

        private readonly IPlanServices _PlanServices;

        public PlansController(IPlanServices planServices)
        {
            _PlanServices = planServices;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        => View(await _PlanServices.GetAllPlansAsync(ct: ct));

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var plan = await _PlanServices.GetPlanByIdAsync(id, ct);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan not found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);


        }

        [HttpGet]

        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var plan = await _PlanServices.GetPlanToUpdateAsync(id, ct);

            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan not found or has active memberships";
                return RedirectToAction(nameof(Index));

            }
            return View(plan);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateToPlanViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _PlanServices.UpdateToPlanAsync(id, model, ct);
            if (result)
                TempData["SuccessMessage"] = "Plan updated successfully";
            else
                TempData["ErrorMessage"] = "Failed to update plan";
            return RedirectToAction(nameof(Index));


        }

        [HttpPost]

        public async Task<IActionResult> Activate(int id, CancellationToken ct)
        {
            var result = await _PlanServices.ToggleActivationAsync(id, ct);
            if (result)
                TempData["SuccessMessage"] = "Plan activation toggled successfully";
            else
                TempData["ErrorMessage"] = "Failed to toggle plan activation";
            return RedirectToAction(nameof(Index));
        }
    }
}
