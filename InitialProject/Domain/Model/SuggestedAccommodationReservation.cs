using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;

namespace InitialProject.Domain.Model
{
    public class SuggestedAccommodationReservation
    {
        public Accommodation Accommodation { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }

        public SuggestedAccommodationReservation(Accommodation accommodation, DateTime arrival, DateTime departure)
        {
            this.Accommodation = accommodation;
            Arrival = arrival;
            Departure = departure;
        }

    }
}
