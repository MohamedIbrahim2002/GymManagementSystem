using GymManagment.DAL.Data.DbContexts;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymManagment.DAL.Repositories.Classes
{
    public class MembershipRepository :GenaricRepository<Membership> , IMembershipRepository
    {
        private readonly GymDbContext _dbContext;

        public MembershipRepository(GymDbContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Membership>> GetMembershipWithPlansAndMembers(Expression<Func<Membership, bool>>? filter = null,CancellationToken ct = default)
        {
            var   query = _dbContext.Memberships
                                        .Include(m => m.Member)
                                        .Include(m => m.Plan)
                                        .AsNoTracking();

            if(filter != null)
               query= query.Where(filter);

            return await query.ToListAsync(ct);


        }
    }
}
