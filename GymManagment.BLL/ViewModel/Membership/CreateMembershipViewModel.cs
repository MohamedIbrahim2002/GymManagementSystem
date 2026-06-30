using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagment.BLL.ViewModel.Membership
{
    public class CreateMembershipViewModel
    {
        public DateTime? StartDate { get; set; }

        public int PlanId { get; set; }
        public int MemberId { get; set; }


    }
}
