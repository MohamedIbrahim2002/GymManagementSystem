using AutoMapper;
using GymManagment.BLL.Common;
using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.ViewModel.Booking;
using GymManagment.BLL.ViewModel.Membership;
using GymManagment.BLL.ViewModel.Session;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagment.BLL.Services.Classes
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
        {
           var sessions = await _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct);
            var mappedSessions = _mapper.Map<IEnumerable< SessionViewModel>>(sessions);

            foreach (var session in mappedSessions)
            {
               session.AvailableSlots = session.Capacity- await _unitOfWork.SessionRepository.GetCountAvailableSlotsAsync(session.Id,ct);   
            }
            return mappedSessions;


        }

        public async Task<IEnumerable<MemberForSessionViewModel>> GetMemberForSession(int sessionId, CancellationToken ct = default)
        {
            var bookings = await _unitOfWork.BookingRepository.GetBySessionId(sessionId, ct);
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId, ct);
            return bookings.Select(booking => new MemberForSessionViewModel
            {
                MemberId = booking.MemberId,
                SessionId = booking.SessionId,
                BookingDate = booking.CreatedAt,
                MemberName = booking.Member.Name,
                IsAttended = session?.EndDate > DateTime.Now ? false : booking.isAttended


            }).ToList();
        }
        public async Task<IEnumerable<MemberSelectListViewModel>> GetMemberForDropDown(int sessionId, CancellationToken ct = default)
        {
            var bookings = await _unitOfWork.BookingRepository.GetAllAsync(false, ct);
            
             var bookedMemberIds = bookings.Select(b=>b.MemberId).ToList();
            
                var availableMember =await _unitOfWork.GetRepository<Member>().GetAllAsync(false, ct); 
        

            return _mapper.Map<IEnumerable< MemberSelectListViewModel>>(availableMember);

        }

        public async Task<Result> CreateBooking(CreateBookingViewModel model, CancellationToken ct = default)
        {

            var session =await _unitOfWork.SessionRepository.GetByIdAsync(model.SessionId, ct);
            if (session is null)

                return Result.NotFound("session not found");

            if (session.StartDate <= DateTime.Now)
                return Result.Fail("Cannot book session that has already started");


            var membership = await _unitOfWork.MembershipRepository.AnyAsync(m=>m.MemberId==model.MemberId && m.EndDate>DateTime.Now);

            if (!membership)
                return Result.Fail("You Donot have membership");


            var alreadyBooked = await _unitOfWork.BookingRepository.AnyAsync(b=>b.MemberId==model.MemberId &&b.SessionId==model.SessionId );

            if (alreadyBooked)
            return Result.Fail("You already booked this session");



            var bookedSlots = await _unitOfWork.SessionRepository.GetCountAvailableSlotsAsync(model.SessionId, ct);

            if (bookedSlots >= session.Capacity)
                return Result.Fail("Session Is Full");

            var booking = new Booking
            {
                MemberId = model.MemberId,
                SessionId = session.Id,
                isAttended = false,
                CreatedAt = DateTime.Now
            };

              _unitOfWork.BookingRepository.Add(booking);

            return await _unitOfWork.SaveChangesAsync(ct) > 0? Result.Ok() : Result.Fail("fail to book this seesion");
          

        }

        public async Task<Result> MarkAttendedAsync(int memberId, int sessionId, CancellationToken ct = default)
        {
            var booking = await _unitOfWork.BookingRepository.FirstOrDefaultAsync(b=>b.MemberId == memberId && b.SessionId == sessionId , false,ct);

            if (booking is null)
                return Result.NotFound("booking is not found");
             booking.isAttended = true;
            booking.UpdatedAt= DateTime.Now;

            _unitOfWork.BookingRepository.Update(booking);

            return await _unitOfWork.SaveChangesAsync() > 0 ? Result.Ok() : Result.Fail("Fail to mark ");
        
        
        }

        public async Task<Result> CancelBookingAsync(int memberId, int sessionId, CancellationToken ct = default)
        {

            var session = await _unitOfWork.SessionRepository.GetByIdAsync(sessionId);
            if (session is null)
                return Result.NotFound("Cannot find session");
            if (session.StartDate <= DateTime.Now)
                return Result.Fail("Cannot cancel book");


            _unitOfWork.SessionRepository.Delete(session);

            return await _unitOfWork.SaveChangesAsync() > 0 ? Result.Ok() : Result.Fail("Fail to cancelled this booking ");



        }

    }
}
