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

        public List<ReschedulingAccommodationRequest> GetApprovedRequests(Guest1 guest1)
        {
            List<ReschedulingAccommodationRequest> approvedRequests = new List<ReschedulingAccommodationRequest>();

            foreach (ReschedulingAccommodationRequest request in requests)
            {
                if (request.state == State.Approved && request.Reservation.Guest.Id == guest1.Id)
                {
                    approvedRequests.Add(request);
                }
            }

            approvedRequests.Reverse();
            return approvedRequests;
        }

        public List<ReschedulingAccommodationRequest> GetDeclinedRequests(Guest1 guest1)
        {
            List<ReschedulingAccommodationRequest> declinedRequests = new List<ReschedulingAccommodationRequest>();

            foreach (ReschedulingAccommodationRequest request in requests)
            {
                if (request.state == State.Declined && request.Reservation.Guest.Id == guest1.Id)
                {
                    declinedRequests.Add(request);
                }
            }

            declinedRequests.Reverse();
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

        public List<ReschedulingAccommodationRequest> GetPendingRequests(Guest1 guest1)
        {
            List<ReschedulingAccommodationRequest> pendingRequests = new List<ReschedulingAccommodationRequest>();

            foreach (ReschedulingAccommodationRequest request in requests)
            {
                if (request.state == State.Pending && request.Reservation.Guest.Id == guest1.Id)
                {
                    pendingRequests.Add(request);
                }
            }
            pendingRequests.Reverse();
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
                }
                else
                {
                    request.Reservation = reservation;
                }
            }
        }

        public ReschedulingAccommodationRequest ChangeState(ReschedulingAccommodationRequest request, State newState)
        {
            ReschedulingAccommodationRequest updatedRequest = requests.Find(n => n.Id == request.Id);
            updatedRequest.state=newState;
            updatedRequest.OwnerExplanationForDeclining = request.OwnerExplanationForDeclining;
            if(newState == State.Approved)
                UpdateReservationDates(request);
            return requestRepository.Update(updatedRequest);
        }

        private bool IsReservationCancelled(AccommodationReservation reservation)
        {
            AccommodationReservationService accommodationReservationService = new AccommodationReservationService();
            return accommodationReservationService.IsCancelled(reservation);
        }

        private void UpdateReservationDates(ReschedulingAccommodationRequest request)
        {
            AccommodationReservationService accommodationReservationService = new AccommodationReservationService();
            request.Reservation.Arrival = request.NewArrivalDate;
            request.Reservation.Departure = request.NewDepartureDate;
            accommodationReservationService.Update(request.Reservation);
        }
    }
}
