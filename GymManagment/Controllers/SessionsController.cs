using GymManagment.BLL.Common;
using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.ViewModel.Session;
using GymManagment.DAL.Repositories.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagment.PL.Controllers
{
    [Authorize]

    public class SessionsController : Controller
    {
        private readonly ISessionServices _sessionServices;

        public SessionsController(ISessionServices sessionServices)
        {
            _sessionServices = sessionServices;
        }

        public async Task<ActionResult> Index()
        {
            var sessions = await _sessionServices.GetAllSessionsAsync();
            return View(sessions);

        }

        public ActionResult Details(int id)
        {
            // Implement logic to get session details by id
            return View();
        }

        [HttpGet]

        public async Task<IActionResult> Create()
        {
            // select id and display name
            await PopulateDropDownAsync();
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownAsync();
                return View(model);
            }

            var result = await _sessionServices.CreatesessionAsync(model, ct);

            if (result.success)
            {
                TempData["SuccessMessage"] = "Session Created";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.error;

            await PopulateDropDownAsync();

            return View(model);
        }
        private async Task PopulateDropDownAsync()
        {
            ViewBag.Trainers = new SelectList(await _sessionServices.GetTrainersForDropDownAsync(), "Id", "Name");
            ViewBag.Categories = new SelectList(await _sessionServices.GetCategoriesForDropDownAsync(), "Id", "CategoryName");

        }

        [HttpGet]

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var result = await _sessionServices.GetSessionByIdAsync(id, ct);
            if (result.success)
                return View(result.Value);
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }





        }



        [HttpGet]

        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await _sessionServices.GetSessionToUpdateAsync(id, ct);
            if (result.success)
            {
                ViewBag.Trainers = new SelectList(await _sessionServices.GetTrainersForDropDownAsync(), "Id", "Name");
                return View(result.Value);

            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }

        }


        [HttpPost]

        public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Trainers = new SelectList(await _sessionServices.GetTrainersForDropDownAsync(), "Id", "Name");
                return View(model);
            }


            var result = await _sessionServices.UpdateSessionAsync(id, model, ct);

            if (result.success)
            {
                TempData["SuccessMessage"] = "Session Updated";
                return RedirectToAction(nameof(Index));
            }
            else
            {

                TempData["ErrorMessage"] = result.error;
                ViewBag.Trainers = new SelectList(await _sessionServices.GetTrainersForDropDownAsync(), "Id", "Name");
                return View(model);
            }


        }

        [HttpGet]

        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _sessionServices.GetSessionByIdAsync(id, ct);
            if (result.success)
            {
                return View(result.Value);

            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }


        }

        [HttpPost]
            public async Task<IActionResult> DeleteConfirmed(int id,CancellationToken ct) 
           {
           
                var session = await _sessionServices.RemoveSessionAsync(id, ct);

            TempData[session.success? "SuccessMessage" : "ErrorMesage"] = session.success ? "SessionDeleted" : session.error;

            return RedirectToAction(nameof(Index));



           }    
       
             
        

    }
}
