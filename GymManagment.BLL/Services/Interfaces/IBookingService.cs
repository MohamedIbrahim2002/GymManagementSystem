using GymManagment.BLL.Common;
using GymManagment.BLL.ViewModel.Booking;
using GymManagment.BLL.ViewModel.Membership;
using GymManagment.BLL.ViewModel.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagment.BLL.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default);
        Task<IEnumerable<MemberForSessionViewModel>> GetMemberForSession(int sessionId ,CancellationToken ct = default);
        Task<IEnumerable<MemberSelectListViewModel>> GetMemberForDropDown(int sessionId ,CancellationToken ct = default);
        Task<Result> CreateBooking(CreateBookingViewModel model , CancellationToken ct = default);
        Task<Result> MarkAttendedAsync (int memberId,int sessionId , CancellationToken ct = default);
        Task<Result> CancelBookingAsync (int memberId,int sessionId , CancellationToken ct = default);


    }
}
