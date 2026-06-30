using GymManagment.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagment.DAL.Repositories.Interfaces
{
    public interface IBookingRepository:IGenaricRepository<Booking>
    {
        Task<List<Booking>>GetBySessionId (int sessionId , CancellationToken ct = default);
    }
}
