using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagment.DAL.Data.Models
{
    public class Booking : BaseEntity
    {
        public Session Session { get; set; }
        public int SessionId { get; set; }
        public Member Member { get; set; }
        public int MemberId { get; set; }
         public bool isAttended { get; set; }
    }
}
