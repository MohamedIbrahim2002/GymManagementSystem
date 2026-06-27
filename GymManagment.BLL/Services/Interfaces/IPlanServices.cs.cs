using GymManagment.BLL.ViewModel.Member;
using GymManagment.BLL.ViewModel.Plan;


namespace GymManagment.BLL.Services.Interfaces
{
    public interface IPlanServices
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default);

        Task<PlanViewModel?> GetPlanByIdAsync(int id, CancellationToken ct = default);
        Task<UpdateToPlanViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken ct = default);
        Task<bool> ToggleActivationAsync(int planId, CancellationToken ct = default);
        Task<bool> UpdateToPlanAsync(int id ,UpdateToPlanViewModel model, CancellationToken ct = default);
    }
}
