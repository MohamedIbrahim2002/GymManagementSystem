using GymManagment.BLL.Common;
using GymManagment.BLL.ViewModel.Session;
using GymManagment.DAL.Data.Models;

namespace GymManagment.BLL.Services.Interfaces
{
    public interface ISessionServices
    {
       Task<IEnumerable<SessionViewModel?>> GetAllSessionsAsync( CancellationToken ct = default);
        Task<Result> CreatesessionAsync(CreateSessionViewModel session , CancellationToken ct = default);
        Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default);
        Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync( CancellationToken ct = default);

        Task<Result<SessionViewModel>> GetSessionByIdAsync(int sessionId , CancellationToken ct = default);

        Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int sessionId, CancellationToken ct = default);

        Task<Result> UpdateSessionAsync(int sessionId, UpdateSessionViewModel session, CancellationToken ct = default);

        Task<Result> RemoveSessionAsync (int sessionId , CancellationToken ct = default);
    }
}
