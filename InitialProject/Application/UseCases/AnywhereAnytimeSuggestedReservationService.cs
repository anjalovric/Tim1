using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Service;

namespace InitialProject.APPLICATION.UseCases
{
    public class AnywhereAnytimeSuggestedReservationService
    {
        AccommodationReservationService accommodationReservationService;
        public AnywhereAnytimeSuggestedReservationService()
        {
            accommodationReservationService = new AccommodationReservationService();
        }
    }
}
