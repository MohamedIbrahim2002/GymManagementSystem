using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagment.BLL.ViewModel.Trainer
{
    public class TrainerViewModel
    {
        public int id { get; set; }
        public string Name { get; set; } 
        public string Email { get; set; }
        public string phone { get; set; }
        public string Specialization { get; set; }

        // trainer details viewmodel

        public DateOnly? DateOfBirth { get; set; } 
        public string? address { get; set; }


    }
}
