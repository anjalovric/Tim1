using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class AvailableDatesForAccommodationReservation
    {
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }

        public AvailableDatesForAccommodationReservation(DateTime arrival, DateTime departure)
        {
            Arrival = arrival;
            Departure = departure;
        }

    }
}
