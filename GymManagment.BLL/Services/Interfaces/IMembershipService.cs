using GymManagment.BLL.Common;
using GymManagment.BLL.ViewModel.Membership;


namespace GymManagment.BLL.Services.Interfaces
{
    public interface IMembershipService
    {
        Task<IEnumerable<MembershipViewModel>> GetAllMembershipsAsync(CancellationToken ct = default);
        Task<Result>CreateMembershipAsync (CreateMembershipViewModel model,CancellationToken ct = default);
        Task<IEnumerable<MemberSelectListViewModel>> GetMemberForDropDownList(CancellationToken ct = default);
        Task<IEnumerable<PlanSelectListViewModel>> GetPlanForDropDownList(CancellationToken ct = default);
        Task<Result>DeleteActiveMembership(int memberId,CancellationToken ct = default);


    }
}
