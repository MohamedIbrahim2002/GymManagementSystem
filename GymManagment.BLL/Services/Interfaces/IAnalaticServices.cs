using GymManagment.BLL.ViewModel.Analitic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagment.BLL.Services.Interfaces
{
    public interface IAnalaticServices
    {
        Task <AnalaticViewModel> GetAnalaticDataAsync ( CancellationToken ct = default);
    }
}
