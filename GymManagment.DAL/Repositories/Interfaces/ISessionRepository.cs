using GymManagment.DAL.Data.Models;
using System.Linq.Expressions;

namespace GymManagment.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenaricRepository<Session>
    {
        // additional methods to get sessions with related data
        Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct = default);
        Task<int> GetCountAvailableSlotsAsync(  int sessionId,CancellationToken ct = default);
        Task<Session?> GetSessionByIdWithTrainerAndCategory(int sessionId, CancellationToken ct = default);

    }
}
