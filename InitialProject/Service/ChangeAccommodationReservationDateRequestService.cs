using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.Service
{
    public class ChangeAccommodationReservationDateRequestService
    {
        private ChangeAccommodationReservationDateRequestRepository requestRepository;
        public ChangeAccommodationReservationDateRequestService()
        {
            requestRepository = new ChangeAccommodationReservationDateRequestRepository();
            
        }

        public List<ChangeAccommodationReservationDateRequest> GetAll()
        {
            return requestRepository.GetAll();
        }



        public void Add(ChangeAccommodationReservationDateRequest request)
        {
            requestRepository.Add(request);
        }

        public ChangeAccommodationReservationDateRequest GetById(int id)
        {
            return requestRepository.GetById(id);
        }
    }
}
