
using System.ComponentModel.DataAnnotations.Schema;
namespace GymManagment.DAL.Data.Models
{
    public class Membership : BaseEntity
    {
        public int MemberId { get; set; }
        public Member Member { get; set; } = default!;
        public int PlanId { get; set; }
        public Plan Plan { get; set; } = default!;
        // startDate = created at
        public DateTime EndDate { get; set; }
        [NotMapped]
        public string Status => EndDate > DateTime.Now ? "Active" : "Expired";
        [NotMapped]
        public bool IsActive => EndDate > DateTime.Now;

    }
}
