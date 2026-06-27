using GymManagment.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagment.DAL.Repositories.Interfaces
{
    // new parameter less constuctor cannot create object , abstract class from baseentity , gym user
    public interface IGenaricRepository <TEntity> where TEntity : BaseEntity , new()
    {
        Task <IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default);
        Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        void Add  (TEntity entity);
        void Update (TEntity entity);
        void Delete(TEntity entity);
        Task<bool> AnyAsync (Expression<Func<TEntity, bool>> predicate , CancellationToken ct = default);
         Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = false, CancellationToken ct = default);

        Task<int> CountAsync(Expression<Func<TEntity, bool>>? condition = null , CancellationToken ct = default);
    
    }
}
