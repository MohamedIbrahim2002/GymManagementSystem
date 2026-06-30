using GymManagment.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagment.DAL.Repositories.Interfaces
{
    public interface IMembershipRepository : IGenaricRepository<Membership>
    {
     Task<IEnumerable<Membership>>GetMembershipWithPlansAndMembers(Expression<Func<Membership,bool>>?filter = null,CancellationToken ct = default);


    }
}
