using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.ViewModel.Booking;
using GymManagment.BLL.ViewModel.Membership;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagment.PL.Controllers
{
    public class SessionScheduleController : Controller
    {
        private readonly IBookingService _bookingService;

        public SessionScheduleController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        public IActionResult Index(CancellationToken ct)
        {
            var sessions = _bookingService.GetAllSessionsAsync(ct);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetMembersForSession(int id, CancellationToken ct)
        {
            var members = await _bookingService.GetMemberForSession(id, ct);

            return View(members);
        }
        [HttpGet]
        public async Task<IActionResult> GetMembersForUpComingSession(int id, CancellationToken ct)
        {
            var members = await _bookingService.GetMemberForSession(id, ct);

            return View(members);
        }
        [HttpGet]
        public async Task<IActionResult> GetMembersForOngoingSession(int id, CancellationToken ct)
        {
            var members = await _bookingService.GetMemberForSession(id, ct);

            return View(members);
        }

        [HttpGet]
        public async Task<IActionResult> CreateBooking(int id, CancellationToken ct)
        {
            var members = await _bookingService.GetMemberForSession(id, ct);
            ViewBag.Members = new SelectList(members, "Id", "Name");
            ViewBag.SessionId = id;

            return View();
        }

        [HttpPost]

        public async Task <IActionResult> Create (CreateBookingViewModel model, CancellationToken ct)
        {
            var result = await _bookingService.CreateBooking(model, ct);
            TempData[result.success ? "SuccessMessage" : "ErrorMessage"] =
                result.success ? "booking Created successfully" : result.error;

            return RedirectToAction(nameof(GetMembersForUpComingSession) , new {id = model.SessionId});

        }
        [HttpPost]

        public async Task <IActionResult> Attended (int memberid ,int sessionid , CancellationToken ct)
        {
            var result = await _bookingService.MarkAttendedAsync(memberid,sessionid, ct);
            TempData[result.success ? "SuccessMessage" : "ErrorMessage"] =
                result.success ? "Marked Attended successfully" : result.error;

            return RedirectToAction(nameof(GetMembersForOngoingSession),new {id = sessionid} );

        }
        [HttpPost]

        public async Task <IActionResult> Cancel (int memberid ,int sessionid , CancellationToken ct)
        {
            var result = await _bookingService.CancelBookingAsync(memberid,sessionid, ct);
            TempData[result.success ? "SuccessMessage" : "ErrorMessage"] =
                result.success ? "Booking Canceled successfully" : result.error;

            return RedirectToAction(nameof(GetMembersForUpComingSession),new {id = sessionid} );

        }



        protected async Task<IEnumerable<MemberSelectListViewModel>> GetMemberForDropDown(int sessionId, CancellationToken ct)
        {
            var members = await _bookingService.GetMemberForDropDown(sessionId, ct);

            return members;
        }



        
    } 
}   



    

