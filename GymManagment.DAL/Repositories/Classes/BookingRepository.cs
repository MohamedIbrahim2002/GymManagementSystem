using GymManagment.DAL.Data.DbContexts;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagment.DAL.Repositories.Classes
{
    public class BookingRepository : GenaricRepository<Booking> , IBookingRepository
    {
        private readonly GymDbContext _dbContext;

        public BookingRepository(GymDbContext dbContext): base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public void Add(Booking entity)
        {

        }

        public Task<bool> AnyAsync(Expression<Func<Booking, bool>> predicate, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(Expression<Func<Booking, bool>>? condition = null, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(Booking entity)
        {
            throw new NotImplementedException();
        }

        public Task<Booking?> FirstOrDefaultAsync(Expression<Func<Booking, bool>> predicate, bool tracking = false, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Booking>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Booking?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public   Task<List<Booking>> GetBySessionId(int sessionId, CancellationToken ct = default)
        {
          return  _dbContext.Bookings
              .AsNoTracking()
              .Include(b => b.Member)
              .Where(b => b.SessionId == sessionId)
              .ToListAsync(ct);
        }

        public void Update(Booking entity)
        {
            throw new NotImplementedException();
        }
    }
}
