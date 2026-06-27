using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace GymManagment.DAL.Data.Models
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
