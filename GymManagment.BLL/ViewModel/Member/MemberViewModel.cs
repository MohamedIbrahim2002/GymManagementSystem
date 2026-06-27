using GymManagment.DAL.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagment.BLL.ViewModel.Member
{
    public class MemberViewModel
    {
        public int Id { get; set; }
        public string? Photo { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
       public string  Gender { get; set; }

        // member details nullable because in index( GetAll) view we will not show member details
        public string? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? planName { get; set; }
        public string? MemberShipStartDate { get; set; }
        public string? MemberShipEndDate { get; set; }


    }
}
