using AutoMapper;
using GymManagment.BLL.Common;
using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.ViewModel.Session;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Data.Models.Enums;
using GymManagment.DAL.Repositories.Interfaces;
namespace GymManagment.BLL.Services.Classes
{
    public class SessionServices : ISessionServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionServices(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> CreatesessionAsync(CreateSessionViewModel model, CancellationToken ct = default )
        {
            if (model.EndDate <= model.StartDate) return Result.Validation("EndDate Must Be After StartDate");
            if( model.StartDate <= DateTime.Now) return  Result.Validation("StartDate Must Be In The Future");

            if (model.Capacity < 1 || model.Capacity > 25) return Result.Validation("Capacity mMst Be Between 1 and 25");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if(trainer is null) return Result.NotFound("Trainer Not Found");

            var category =await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId);
            if (category is null) return Result.NotFound("Category Not Found");

            var IsValid = Enum.TryParse<Specialty>(category.CategoryName, true ,out var categorySpeciality);
            if(!IsValid || trainer.specialty != categorySpeciality) return Result.Validation("CategoryName Must Be Specialaty in Trainer");

            var session = _mapper.Map<CreateSessionViewModel, Session>(model);

                _unitOfWork.GetRepository<Session>().Add(session);
            var result = await _unitOfWork.SaveChangesAsync();

            return result > 0 ? Result.Ok() : Result.Fail("Fail To Create");

        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync( CancellationToken ct)
        {
            var sessionRepo = _unitOfWork.SessionRepository;

            var sessions = await sessionRepo.GetAllSessionsWithTrainerAndCategoryAsync(ct);
            if (sessions == null || !sessions.Any() ) return null;

            var mappedSessions = sessions.Select(s => new SessionViewModel()
            {
                Id = s.Id,
                TrainerName = s.Trainer.Name,
                Capacity = s.Capacity,
                CategoryName = s.Category.CategoryName,
                Description = s.Description,
                EndDate = s.EndDate,
                StartDate = s.StartDate,
               
            });

            foreach (var item in mappedSessions)
            {
                item.AvailableSlots = item.Capacity - await sessionRepo.GetCountAvailableSlotsAsync(item.Id, ct);

            }
            return mappedSessions;
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetRepository<Category>().GetAllAsync(ct:ct);
            return _mapper.Map<IEnumerable< CategorySelectViewModel>>(result);
            
        }

        public async Task< Result<SessionViewModel>> GetSessionByIdAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetSessionByIdWithTrainerAndCategory(sessionId, ct);

            if (session == null) 
                return Result<SessionViewModel>.NotFound(default);
          else
            {

                var mapSession =  _mapper.Map<Session , SessionViewModel>(session);
                mapSession.AvailableSlots = mapSession.Capacity - await _unitOfWork.SessionRepository.GetCountAvailableSlotsAsync(sessionId,ct);
                return Result<SessionViewModel>.OK(mapSession);
            }


        }


        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct:ct);
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(result);
        }

        public async Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId, ct);
            if (session == null) return Result<UpdateSessionViewModel>.NotFound("session not found");

            if (session.StartDate <= DateTime.Now)
                return Result<UpdateSessionViewModel>.Fail("Session can not updated already started");

            var bookingCount = await _unitOfWork.SessionRepository.GetCountAvailableSlotsAsync(sessionId, ct);
            if (bookingCount > 0)
                return Result<UpdateSessionViewModel>.Fail("Cannot update session that has booked");

            var mappedSession = _mapper.Map<Session, UpdateSessionViewModel>(session);
            return Result<UpdateSessionViewModel>.OK(mappedSession);

        }

        public async Task<Result> UpdateSessionAsync(int sessionId, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId, ct);
            if (session == null) return Result.NotFound("CanNot Updated");


            if (session.StartDate <= DateTime.Now)
                return Result.Fail(" Cannot Edit That has already started");

            if (model.EndDate <= session.StartDate)
                return Result.Validation("EndDate Must Be After StartDate");

            var bookingCount = await _unitOfWork.SessionRepository.GetCountAvailableSlotsAsync(sessionId, ct);
            if (bookingCount > 0)
                return Result.Fail("Cannot update session that has booked");


            if (model.StartDate <= DateTime.Now)
                return Result.Validation("StartDate must be in the future ");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if (trainer is null) return Result.NotFound("Trainer Not Found");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(session.CategoryId);

            var IsValid = Enum.TryParse<Specialty>(category?.CategoryName, true, out var categorySpeciality);

            if (!IsValid || trainer.specialty != categorySpeciality) return Result.Validation("CategoryName Must Be Specialaty in Trainer");

            _mapper.Map(model, session);
            session.UpdatedAt = DateTime.Now;
            _unitOfWork.SessionRepository.Update(session);

            var res = await _unitOfWork.SaveChangesAsync();

            return res > 0 ?  Result.Ok() : Result.Fail("Cannot Updated");


        }

        public async Task<Result> RemoveSessionAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId,ct);

            if (session is null) return Result.NotFound("session NotFound");
            if (session.EndDate >= DateTime.Now)
                return Result.Fail("Cannot Delete session has not ended");

            var bookCount = await _unitOfWork.SessionRepository.GetCountAvailableSlotsAsync(sessionId, ct);
            if(bookCount > 0) return Result.Fail("Cannot Delete Session Has Bookings");

            _unitOfWork.SessionRepository.Delete(session);

            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Cannot Delete Session");


        }
    }
}
