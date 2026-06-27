using GymManagment.BLL.Services.Classes;
using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.ViewModel.Member;
using GymManagment.BLL.ViewModel.Trainer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagment.PL.Controllers
{
    [Authorize(Roles ="SuperAdmin")]

    public class TrainersController : Controller
    {
        private readonly ITrainerServices _trainerServices;

        public TrainersController(ITrainerServices trainerServices)
        {
            _trainerServices = trainerServices;
        }

        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var trainers = await _trainerServices.GetAllTrainersAsync(ct);
            if (trainers == null)
            {
                TempData["ErrorMessage"] = "No trainers found";
                return NotFound();
            }
            return View(trainers);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create), model);

            var result = await _trainerServices.CreateTrainerAsync(model, ct);
            if (result)
                TempData["SuccessMessage"] = "Trainer created successfully";
            else
                TempData["ErrorMessage"] = "Failed to create trainer";

            return RedirectToAction(nameof(Index));


        }



        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var trainer = await _trainerServices.GetTrainerDetailsByIdAsync(id, ct);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);

        }

        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var trainer = await _trainerServices.GetTrainerToUpdateAsync(id, ct);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TrainerToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Edit), model);
            var result = await _trainerServices.UpdateTrainerDetailsAsync(id, model, ct);
            if (result)
                TempData["SuccessMessage"] = "Trainer updated successfully";
            else
                TempData["ErrorMessage"] = "Failed to update trainer";
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var trainer = await _trainerServices.GetTrainerDetailsByIdAsync(id, ct);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id, CancellationToken ct)
        {
            var result = await _trainerServices.RemoveTrainerAsync(id, ct);
            if (result)
                TempData["SuccessMessage"] = "Trainer deleted successfully";
            else
                TempData["ErrorMessage"] = "Failed to delete trainer";

            return RedirectToAction(nameof(Index));

        }

    }
}
