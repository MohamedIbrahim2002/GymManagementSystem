using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagment.BLL.ViewModel.Plan
{
    public class UpdateToPlanViewModel
    {
        public string PlanName { get; set; } = default!;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = default!;
        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [Range(1, 365, ErrorMessage = "Duration must be between 1 and 365 days ")]
        public int DurationInDays { get; set; }
    }
}
