
using GymManagment.DAL.Data.Models;

namespace GymManagment.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IGenaricRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity , new();
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        public IBookingRepository  BookingRepository { get; }
        public ISessionRepository SessionRepository { get; }
        public IMembershipRepository MembershipRepository { get; }
    }
}
