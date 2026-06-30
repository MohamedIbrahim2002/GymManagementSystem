using AutoMapper;
using GymManagment.BLL.Common;
using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.ViewModel.Membership;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Repositories.Interfaces;

namespace GymManagment.BLL.Services.Classes
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MembershipService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<MembershipViewModel>> GetAllMembershipsAsync(CancellationToken ct = default)
        {
            var memberships = await _unitOfWork.MembershipRepository
                .GetMembershipWithPlansAndMembers(m => m.EndDate > DateTime.Now, ct);

            if (memberships == null)
                return Enumerable.Empty<MembershipViewModel>();

            return _mapper.Map<IEnumerable<MembershipViewModel>>(memberships)
                   ?? Enumerable.Empty<MembershipViewModel>();
        }
        public async Task<Result> CreateMembershipAsync(CreateMembershipViewModel model, CancellationToken ct = default)
        {
            // check member is exist
            var memberExist = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Id == model.MemberId , ct);
            if (!memberExist)
                return Result.NotFound("Member is Not Found");
            // check plan is exist
            var planExist = await _unitOfWork.GetRepository<Plan>().AnyAsync(m => m.Id == model.PlanId , ct);
            if (!planExist)
                return Result.NotFound("Member is Not Found");
            // check member has active membership
            var hasActiveMembership = await _unitOfWork.MembershipRepository.AnyAsync(m=>m.MemberId==model.MemberId && m.EndDate > DateTime.Now, ct);
            if (hasActiveMembership)
                return Result.Fail("Member Already has an active membership");

            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(model.PlanId,ct);
            if (!plan.IsActive)
                return Result.Fail("Plan Is Not active");

            var membership = _mapper.Map<Membership>(model);
            membership.EndDate=(model.StartDate?? DateTime.UtcNow).AddDays(plan.DurationDays);


             _unitOfWork.MembershipRepository.Add(membership);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.NotFound("Fail To create membership");
        }

        public async Task<IEnumerable<MemberSelectListViewModel>> GetMemberForDropDownList(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct:ct);
            return _mapper.Map <IEnumerable<MemberSelectListViewModel>>(members);

        }

        public async Task<IEnumerable<PlanSelectListViewModel>> GetPlanForDropDownList(CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct:ct);
            return _mapper.Map<IEnumerable<PlanSelectListViewModel>>(plans);
        }
        public async Task<Result> DeleteActiveMembership(int memberId, CancellationToken ct = default)
        {
         var activeMembership= await _unitOfWork.MembershipRepository.FirstOrDefaultAsync(m=>m.MemberId == memberId&&m.EndDate>DateTime.Now, true);


            if (activeMembership is null)
                return Result.Fail("Active Membership not found" , ResultKind.NotFound);
        
            _unitOfWork.MembershipRepository.Delete(activeMembership);
           var result =   await _unitOfWork.SaveChangesAsync(ct);
            return result > 0? Result.Ok() : Result.Fail("fail to delete active membership");
        
        }

    }
}
