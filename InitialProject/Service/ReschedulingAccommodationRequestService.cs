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
    public class ReschedulingAccommodationRequestService
    {
        private ReschedulingAccommodationRequestRepository requestRepository;
        private List<ReschedulingAccommodationRequest> requests;
        public ReschedulingAccommodationRequestService()
        {
            requestRepository = new ReschedulingAccommodationRequestRepository();
            requests = new List<ReschedulingAccommodationRequest>(requestRepository.GetAll());
            SetReservations();
        }

        public List<ReschedulingAccommodationRequest> GetAll()
        {
            return requests;
        }

        public void Add(ReschedulingAccommodationRequest request)
        {
            requestRepository.Add(request);
        }

        public ReschedulingAccommodationRequest GetById(int id)
        {
            return requestRepository.GetById(id);
        }

        public List<ReschedulingAccommodationRequest>GetPendingRequests()
        {
            List<ReschedulingAccommodationRequest> pendingRequests = new List<ReschedulingAccommodationRequest>();

            foreach(ReschedulingAccommodationRequest request in requests)
            {
                if(request.state==State.Pending)
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

            foreach(ReschedulingAccommodationRequest request in requests)
            {
                AccommodationReservation reservation = allReservations.Find(n => n.Id == request.Reservation.Id);
                if(reservation != null)
                {
                    request.Reservation = reservation;
                    SetOldReservationDates(request, reservation);
                }
            }
        }

        private void SetOldReservationDates(ReschedulingAccommodationRequest request, AccommodationReservation reservation)
        {
            request.OldArrivalDate = reservation.Arrival;
            request.OldDepartureDate = reservation.Departure;
        }

        public ReschedulingAccommodationRequest ChangeState(ReschedulingAccommodationRequest request, State newState)
        {
            ReschedulingAccommodationRequest updatedRequest = requests.Find(n => n.Id == request.Id);
            updatedRequest.state=newState;
            return requestRepository.Update(updatedRequest);
        }

    }
}
