using GymManagment.BLL.Services.Interfaces;
using GymManagment.BLL.ViewModel.Analitic;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagment.BLL.Services.Classes
{
    public class AnalaticService : IAnalaticServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalaticService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<AnalaticViewModel> GetAnalaticDataAsync(CancellationToken ct = default)
        {
            // Sessions
            var upcomingSessions = await _unitOfWork.GetRepository<Session>().CountAsync(s=>s.StartDate > DateTime.UtcNow);
            var completedSessions = await _unitOfWork.GetRepository<Session>().CountAsync(s=>s.StartDate < DateTime.UtcNow);
            var ongoningSessions = await _unitOfWork.GetRepository<Session>().CountAsync(s=> s.EndDate > DateTime.UtcNow && s.StartDate<DateTime.Now);

            // Members

            var totalMembers = await _unitOfWork.GetRepository<Member>().CountAsync(ct: ct);
            

            var totalTrainers = await _unitOfWork.GetRepository<Trainer>().CountAsync(ct: ct);
        
        
            var activeMembers = await _unitOfWork.GetRepository<Membership>().CountAsync(x=>x.EndDate > DateTime.Now , ct);


            return new AnalaticViewModel
            {
                TotalMembers = totalMembers,
                ActiveMembers = activeMembers,
                CompletedSessions = completedSessions,
                TotalTrainers = totalTrainers,
                OngoingSessions = ongoningSessions,
                UpcomingSessions = upcomingSessions
            };



        }
    }
}
