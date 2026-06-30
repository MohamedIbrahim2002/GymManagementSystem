using GymManagment.DAL.Data.DbContexts;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Repositories.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagment.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _context;
        // to store repositories 
        private readonly Dictionary<string, object> _repositories = [];
        public UnitOfWork(GymDbContext context , ISessionRepository sessionRepository ,IMembershipRepository membershipRepository) 
        {
            _context = context;
            SessionRepository = sessionRepository;
            MembershipRepository = membershipRepository;
        }

        public ISessionRepository SessionRepository { get; }
        public IMembershipRepository MembershipRepository { get; }

        public IBookingRepository BookingRepository  { get; }

        public IGenaricRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            // check type of entity ??? plan , membership , member , trainer 
            var typeName = typeof(TEntity).Name;
            // if Exist return repository
            if (_repositories.TryGetValue(typeName, out object? repository))
            {
                // cast object to IGenaricRepository<TEntity> and return it
                return (IGenaricRepository<TEntity>)repository;
            }
            // if not exist create new repository - store and return it
            var newRepository = new GenaricRepository<TEntity>(_context);
            _repositories[typeName] = newRepository;
            return newRepository;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
            =>await _context.SaveChangesAsync(ct);


    }
}
