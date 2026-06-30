using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.ViewModel.Plan;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Repositories.Classes;
using GymManagment.DAL.Repositories.Interfaces;

namespace GymManagment.BLL.Services.Classes
{
    public class PlanService : IPlanServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);
            if (!plans.Any()) return [];
            var planViewModel = plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                DurationInDays =p.DurationDays
            });
            return planViewModel;
        }

        public async Task<PlanViewModel> GetPlanByIdAsync(int id, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
            if (plan == null) return null;
            return new PlanViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                Price = plan.Price,
                DurationInDays = plan.DurationDays
            };
        }

        public async Task<UpdateToPlanViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken ct = default)
        {
            var plan =await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);
            if (plan == null || !plan.IsActive) return null;
            if(await HasActiveMemberShipsAsync(planId, ct)) 
                return null;
            else 
                return new UpdateToPlanViewModel
            {
                PlanName = plan.Name,
                Description = plan.Description,
                Price = plan.Price,
                DurationInDays = plan.DurationDays
            };
        }

        public async Task<bool> ToggleActivationAsync(int planId, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);
            if (plan == null) return false;
            if (!plan.IsActive && await HasActiveMemberShipsAsync(planId, ct))
                return false;
            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.Now;
            _unitOfWork.GetRepository<Plan>().Update(plan);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        public async Task<bool> UpdateToPlanAsync(int id, UpdateToPlanViewModel model, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);

            if (plan == null) return false;
            if(await HasActiveMemberShipsAsync(id, ct)) return false;

            plan.UpdatedAt = DateTime.Now;
            plan.Description = model.Description;
            plan.Price = model.Price;
            plan.DurationDays = model.DurationInDays;

            _unitOfWork.GetRepository<Plan>().Update(plan);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        private async Task<bool> HasActiveMemberShipsAsync (int planId, CancellationToken ct )
        {
            return await _unitOfWork.GetRepository<Membership >().AnyAsync(m => m.PlanId == planId && m.EndDate > DateTime.Now, ct);
        }
    }
}
