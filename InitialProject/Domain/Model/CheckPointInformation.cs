using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Model
{
    public class CheckPointInformation
    {
        public CheckPoint CheckPoint { get; set; }
        public List<Guest2> guest2s { get; set; }
        public int countGuests { get;set; }
        public CheckPointInformation() {
        guest2s = new List<Guest2>();
        }

    }
}
