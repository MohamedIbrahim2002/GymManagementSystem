using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
namespace GymManagment.DAL.Data.Models
{

    [Table("HelthRecords")]
 
    public class HealthRecord: BaseEntity
    {
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public string? Note { get; set; }
        public string? BloodType { get; set; }
        // lastUpdated = updated at
        public int MemberId { get; set; }
        public Member Member { get; set; }
    } 
}
