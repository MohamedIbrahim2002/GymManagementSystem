using GymManagment.DAL.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagment.DAL.Data.Models
{
    public class Trainer : GymUser
    {
        // hire date = created at
        public Specialty specialty { get; set; }

        public  ICollection<Session> Sessions { get; set; } = new List<Session>();

    }
}
