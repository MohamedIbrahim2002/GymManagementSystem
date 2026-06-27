using GymManagment.BLL.Common;
using GymManagment.BLL.Services.Attachment;
using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.ViewModel.Member;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GymManagment.PL.Controllers
{
    [Authorize(Roles ="SuperAdmin") ]

    public class MembersController : Controller
    {
        private readonly IMemberServices _memberServices;
        private readonly IAttachmenServices _attachmenServices;

        public MembersController(IMemberServices memberServices , IAttachmenServices attachmenServices)
        {
            _memberServices = memberServices;
            _attachmenServices = attachmenServices;
        }

        // Get Member Photo
        [HttpGet]
        public async Task <IActionResult> GetPhoto(int id)
        {
            var member = await  _memberServices.GetMemberDetailsByIdAsync(id);
            if (member == null || string.IsNullOrWhiteSpace(member.Photo)) 
                return NotFound();
           var rseult =  _attachmenServices.GetFile(member.Photo, "MembersPhoto");

            if (rseult == null) return NotFound();

            return File(rseult.Value.stream ,rseult.Value.contentType );

        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await _memberServices.GetAllMembersAsync(ct);
            return View(members);

        }
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create), model);

            var result = await _memberServices.CreateMemberAsync(model, ct);
            if (result)
                TempData["SuccessMessage"] = "Member created successfully";
            else
                TempData["ErrorMessage"] = "Failed to create member";

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            //Get member details by id
            var member = await _memberServices.GetMemberDetailsByIdAsync(id, ct);
            //check if member null  return index with error message
            if (member == null)
            {
                // return index with error message
                TempData["ErrorMessage"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }
            //member exist return view with member details
            return View(member);
        }

        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            // Get health record details by member id
            var record = await _memberServices.GetMemberHealthRecordDetailsAsync(id, ct);
            // check if health record null  return index with error message
            if (record == null)
            {
                // return index with error message
                TempData["ErrorMessage"] = "Health record not found";
                return RedirectToAction(nameof(Index));
            }
            // health record exist return view with health record details
            return View(record);
        }

        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct)
        {
            var member = await _memberServices.GetMemberToUpdateAsync(id, ct);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> EditMember([FromRoute] int id, MemberToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _memberServices.UpdateMemberAsync(id, model, ct);
            if (result)
                TempData["SuccessMessage"] = "Member updated successfully";
            else
                TempData["ErrorMessage"] = "Failed to update member";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
           var member = await _memberServices.GetMemberDetailsByIdAsync(id, ct);
            if (member == null)

            {
                TempData["ErrorMessage"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id, CancellationToken ct)
        {
            var result = await _memberServices.RemoveMemberAsync(id, ct);
            if (result)
                TempData["SuccessMessage"] = "Member deleted successfully";
            else
                TempData["ErrorMessage"] = "Failed to delete member";

            return RedirectToAction(nameof(Index));

        }  
    }
}

