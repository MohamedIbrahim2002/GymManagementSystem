using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.ViewModel.Membership;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagment.PL.Controllers
{
    [Authorize]
    public class MembershipController : Controller
    {
        private readonly IMembershipService _membershipService;

        public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }
        public async Task<IActionResult> Index()
        {
            var members =await _membershipService.GetAllMembershipsAsync();

            return View(members);
        }
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
           await PopulateDropDownAsync(ct);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMembershipViewModel model,CancellationToken ct)
        {
            if(!ModelState.IsValid)
            {
                await PopulateDropDownAsync(ct);
                return View(model);
            }

            var result = await _membershipService.CreateMembershipAsync(model,ct);
            if(result.success)
            {
                TempData["SuccessMessage"] = "Membership Created Successsfuly";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.error;
            await PopulateDropDownAsync(ct);
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id, CancellationToken ct)
        {
            var result = await _membershipService.DeleteActiveMembership(id, ct);
            if (result.success)
            
                TempData[result.success? "SuccedMessage" : "ErrorMessage" ]
                    =result.success ? "Membership cancelled successfully" :result.error;
                return RedirectToAction(nameof(Index));

           

        }




        private async Task PopulateDropDownAsync(CancellationToken ct)
        {
            ViewBag.Plans = new SelectList(await _membershipService.GetPlanForDropDownList(),"Id","Name");
            ViewBag.Members = new SelectList(await _membershipService.GetMemberForDropDownList(),"Id","Name");

        }





    }
}
