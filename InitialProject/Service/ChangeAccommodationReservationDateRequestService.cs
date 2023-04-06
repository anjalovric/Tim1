using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private List<ChangeAccommodationReservationDateRequest> requests;
        public ChangeAccommodationReservationDateRequestService()
        {
            requestRepository = new ChangeAccommodationReservationDateRequestRepository();
            requests = new List<ChangeAccommodationReservationDateRequest>(requestRepository.GetAll());
            SetReservations();
        }

        public List<ChangeAccommodationReservationDateRequest> GetAll()
        {
            return requests;
        }



        public void Add(ChangeAccommodationReservationDateRequest request)
        {
            requestRepository.Add(request);
        }

        public ChangeAccommodationReservationDateRequest GetById(int id)
        {
            return requestRepository.GetById(id);
        }

        public List<ChangeAccommodationReservationDateRequest>GetPendingRequests()
        {
            List<ChangeAccommodationReservationDateRequest> pendingRequests = new List<ChangeAccommodationReservationDateRequest>();

            foreach(ChangeAccommodationReservationDateRequest request in requests)
            {
                if(request.state==ChangeAccommodationReservationDateRequest.State.Pending)
                {
                    pendingRequests.Add(request);
                }
            }

            return pendingRequests;
        }

        private void SetReservations()
        {
            AccommodationReservationService accommodationReservationService = new AccommodationReservationService();
            List<AccommodationReservation> allReservations = accommodationReservationService.GetAll();

            foreach(ChangeAccommodationReservationDateRequest request in requests)
            {
                AccommodationReservation reservation = allReservations.Find(n => n.Id == request.Reservation.Id);
                if(reservation != null)
                {
                    request.Reservation = reservation;
                }
            }
        }

    }
}
