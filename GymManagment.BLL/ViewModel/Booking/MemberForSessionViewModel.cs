using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagment.BLL.ViewModel.Booking
{
    public class MemberForSessionViewModel
    {
        public int MemberId { get; set; }
        public int SessionId { get; set; }
        public string MemberName { get; set; }
        public bool IsAttended { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
