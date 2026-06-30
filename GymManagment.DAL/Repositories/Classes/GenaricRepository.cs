using GymManagment.DAL.Data.DbContexts;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymManagment.DAL.Repositories.Classes
{
    public class GenaricRepository<TEntity> : IGenaricRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        public GenaricRepository(GymDbContext dbContext )
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

       
            //_dbcontext.set<TEntity>().Add(entity)
        public void Add(TEntity entity)=>_dbSet.Add(entity);
        public void Update(TEntity entity)=>_dbSet.Update(entity);
        public void Delete(TEntity entity) =>_dbSet.Remove(entity);
        
        

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default) => await _dbSet.FindAsync(id, ct);

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            return _dbSet.AsNoTracking().AnyAsync(predicate, ct);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _dbSet : _dbSet.AsNoTracking();
            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? condition, CancellationToken ct = default)
        {

            return condition == null? await _dbSet.AsNoTracking().CountAsync(ct) : await _dbSet.CountAsync(condition, ct);

            
        }

    

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _dbSet : _dbSet.AsNoTracking();
            return await query.ToListAsync();

        }
    }
}
