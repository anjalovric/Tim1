using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class SuggestedReservationViewModel
    {
        public Accommodation Accommodation { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }

        public SuggestedReservationViewModel(Accommodation accommodation, DateTime arrival, DateTime departure)
        {   
            Accommodation = accommodation;
            Arrival = arrival;
            Departure = departure;

        }
    }
}
