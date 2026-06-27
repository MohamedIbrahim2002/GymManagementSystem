using GymManagment.DAL.Data.Models;

namespace GymManagment.BLL.ViewModel.Member
{
    public class HelthRecord : HealthRecord
    {
        public string BloodType { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
    }
}