using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Repository;

namespace InitialProject.Service
{
    public class AccommodationReservationService
    {
        private AccommodationReservationRepository accommodationReservationRepository;
        public AccommodationReservationService()
        {
            accommodationReservationRepository = new AccommodationReservationRepository();
        }


    }
}
