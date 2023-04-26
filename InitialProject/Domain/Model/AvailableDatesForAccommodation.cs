using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class AvailableDatesForAccommodation
    {
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }

        public AvailableDatesForAccommodation(DateTime arrival, DateTime departure)
        {
            Arrival = arrival;
            Departure = departure;
            Departure = Departure.AddHours(23);
            Departure = Departure.AddMinutes(59);
        }
        public AvailableDatesForAccommodation()
        {
        }

        public override string ToString()
        {
            return DateOnly.FromDateTime(Arrival).ToString() + " - " + DateOnly.FromDateTime(Departure).ToString();
        }
    }
}
