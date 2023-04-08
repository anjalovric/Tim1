using System.Collections.Generic;
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

        public List<ReschedulingAccommodationRequest> GetApprovedRequests()
        {
            List<ReschedulingAccommodationRequest> approvedRequests = new List<ReschedulingAccommodationRequest>();

            foreach (ReschedulingAccommodationRequest request in requests)
            {
                if (request.state == State.Approved)
                {
                    approvedRequests.Add(request);
                }
            }

            return approvedRequests;
        }

        public List<ReschedulingAccommodationRequest> GetDeclinedRequests()
        {
            List<ReschedulingAccommodationRequest> declinedRequests = new List<ReschedulingAccommodationRequest>();

            foreach (ReschedulingAccommodationRequest request in requests)
            {
                if (request.state == State.Declined)
                {
                    declinedRequests.Add(request);
                }
            }

            return declinedRequests;
        }

        public void Add(ReschedulingAccommodationRequest request)
        {
            requestRepository.Add(request);
        }

        public ReschedulingAccommodationRequest GetById(int id)
        {
            return requestRepository.GetById(id);
        }

        public List<ReschedulingAccommodationRequest>GetPendingRequestsIfNotCancelled()
        {
            List<ReschedulingAccommodationRequest> pendingRequests = new List<ReschedulingAccommodationRequest>();

            foreach(ReschedulingAccommodationRequest request in requests)
            {
                if(request.state==State.Pending && !IsReservationCancelled(request.Reservation))
                {
                    pendingRequests.Add(request);
                }
            }

            return pendingRequests;
        }

        public List<ReschedulingAccommodationRequest> GetPendingRequests()
        {
            List<ReschedulingAccommodationRequest> pendingRequests = new List<ReschedulingAccommodationRequest>();

            foreach (ReschedulingAccommodationRequest request in requests)
            {
                if (request.state == State.Pending)
                {
                    pendingRequests.Add(request);
                }
            }

            return pendingRequests;
        }

        private void SetReservations()
        {
            AccommodationReservationService accommodationReservationService = new AccommodationReservationService();
            CancelAccommodationReservationService cancelAccommodationReservationService = new CancelAccommodationReservationService();
            List<AccommodationReservation> allReservations = accommodationReservationService.GetAll();
            List<AccommodationReservation> allCancelledReservations = cancelAccommodationReservationService.GetAll();

            foreach (ReschedulingAccommodationRequest request in requests)
            {
                AccommodationReservation reservation = allReservations.Find(n => n.Id == request.Reservation.Id);
                if(reservation == null)
                {
                    AccommodationReservation cancelledReservation = allCancelledReservations.Find(n => n.Id == request.Reservation.Id);
                    request.Reservation = cancelledReservation;
                    SetOldReservationDates(request, cancelledReservation);
                }
                else
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

        private bool IsReservationCancelled(AccommodationReservation reservation)
        {
            AccommodationReservationService accommodationReservationService = new AccommodationReservationService();
            return accommodationReservationService.IsCancelled(reservation);
        }
    }
}
