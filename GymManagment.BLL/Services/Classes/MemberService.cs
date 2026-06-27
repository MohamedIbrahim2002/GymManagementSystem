using AutoMapper;
using GymManagment.BLL.Common;
using GymManagment.BLL.Services.Attachment;
using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.ViewModel.Member;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Repositories.Interfaces;

namespace GymManagment.BLL.Services.Classes
{
    public class MemberService : IMemberServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmenServices _attachmenServices;

        public MemberService(IUnitOfWork unitOfWork , IMapper mapper , IAttachmenServices attachmenServices)
        {
            _unitOfWork = unitOfWork;
           _mapper = mapper;
          _attachmenServices = attachmenServices;
        }
        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            // check if email or phone exist
            var emailExist = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email);
            var phoneExist = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone);
            if (emailExist || phoneExist) return false;
            // upload file 
          var storedPhotName =  await _attachmenServices.UploadAsync(model.PhotoFile.OpenReadStream(), model.PhotoFile.FileName , "MembersPhoto");
            if (string.IsNullOrWhiteSpace(storedPhotName)) return false;


            // add member to database if not exist && casting from viewmodel to entity
            var member =_mapper.Map<Member>(model);
            member.Photo = storedPhotName;
           _unitOfWork.GetRepository<Member>().Add(member);
            var result = await _unitOfWork.SaveChangesAsync();
            if(result>0)
                return true;
            else
            {
                // delete uploaded photo
                _attachmenServices.Delete(storedPhotName, "MembersPhoto");

                return false;
            }

        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct:ct);
            if (!members.Any()) return [];

            var membersViewmodel = _mapper.Map<IEnumerable<Member>, IEnumerable< MemberViewModel>>(members);

            return membersViewmodel;



        }

        public async Task<MemberViewModel?> GetMemberDetailsByIdAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);

            if (member == null) return null;

            // casting with AutoMapper
            var model = _mapper.Map<Member, MemberViewModel>(member);

            var activeMembership = await _unitOfWork.GetRepository<Membership>().FirstOrDefaultAsync(m => m.MemberId == memberId && m.EndDate > DateTime.Now);
            if (activeMembership != null)
            {
                var activePlan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(activeMembership.PlanId, ct);
                model.MemberShipStartDate = activeMembership.CreatedAt.ToString();
                model.MemberShipEndDate = activeMembership.EndDate.ToString();
                model.planName = activePlan?.Name;
            }

            return model;

        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordDetailsAsync(int memberId, CancellationToken ct = default)
        {
            var healthRecord = await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(hr => hr.MemberId == memberId, ct: ct);
            if (healthRecord == null) return null;

            return _mapper.Map<HealthRecord, HealthRecordViewModel>(healthRecord);

        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);

            if (member == null) return null;
            else
            
                return _mapper.Map<Member, MemberToUpdateViewModel>(member);


         }

        public async Task<bool> RemoveMemberAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if (member == null) return false;
           
            var hasfutureBooking = await _unitOfWork.GetRepository<Booking>().AnyAsync(b => b.MemberId == memberId && b.Session.StartDate > DateTime.Now, ct);
            if(hasfutureBooking) return false;
             _unitOfWork.GetRepository<Member>().Delete(member); // delete member from database logically by isdeleted property
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        public async Task<bool> UpdateMemberAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if (member == null) return false;

            var emailExist = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email && m.Id != memberId);
            var phoneExist = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone && m.Id != memberId);
            if (emailExist || phoneExist) return false;

            _mapper.Map(model, member);

              _unitOfWork.GetRepository<Member>().Update(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result> 0? true : false;


        }

    }
}
