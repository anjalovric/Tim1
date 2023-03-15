using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class AvailableDatesForAccommodationReservation
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public AvailableDatesForAccommodationReservation(DateTime start, DateTime end)        {
            Start = start;
            End = end;
        }

    }
}
