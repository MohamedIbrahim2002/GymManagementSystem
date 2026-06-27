using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagment.BLL.ViewModel.AccountViewModel
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress (ErrorMessage ="Inavlid Email!")]
        public string Email { get; set; }

        [DataType (DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe{ get; set; }
        


    }
}
