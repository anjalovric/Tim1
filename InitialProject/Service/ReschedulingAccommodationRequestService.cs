using System;
using System.Collections.Generic;
using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;

namespace InitialProject.Service
{
    public class ReschedulingAccommodationRequestService
    {
        private IReschedulingAccommodationRequestRepository requestRepository = Injector.CreateInstance<IReschedulingAccommodationRequestRepository>();
        private List<ReschedulingAccommodationRequest> requests;
        public ReschedulingAccommodationRequestService()
        {
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
        public List<ReschedulingAccommodationRequest>GetPendingRequestsIfNotCancelled()
        {
            List<ReschedulingAccommodationRequest> pendingRequests = new List<ReschedulingAccommodationRequest>();

            foreach(ReschedulingAccommodationRequest request in requests)
            {
                if(request.state==State.Pending && !IsReservationCancelled(request.Reservation) && request.Reservation.Arrival.Date > DateTime.Now.Date)
                {
                    pendingRequests.Add(request);
                }
            }
            return pendingRequests;
        }

        private void SetReservations()
        {
            AccommodationReservationService accommodationReservationService = new AccommodationReservationService();
            CancelledAccommodationReservationService cancelAccommodationReservationService = new CancelledAccommodationReservationService();
            List<AccommodationReservation> storedReservations = accommodationReservationService.GetAll();
            List<AccommodationReservation> storedCancelledReservations = cancelAccommodationReservationService.GetAll();
            foreach (ReschedulingAccommodationRequest request in requests)
                request.Reservation = SetReservationToRequest(request, storedReservations, storedCancelledReservations);
        }

        private AccommodationReservation SetReservationToRequest(ReschedulingAccommodationRequest request, List<AccommodationReservation> storedReservations, List<AccommodationReservation> storedCancelledReservations)
        {
            AccommodationReservation reservation = storedReservations.Find(n => n.Id == request.Reservation.Id);
            if (reservation == null)
            {
                AccommodationReservation cancelledReservation = storedCancelledReservations.Find(n => n.Id == request.Reservation.Id);
                request.Reservation = cancelledReservation;
            }
            else
                request.Reservation = reservation;
            return request.Reservation;
        }

        public ReschedulingAccommodationRequest ChangeState(ReschedulingAccommodationRequest request, State newState)
        {
            ReschedulingAccommodationRequest updatedRequest = requests.Find(n => n.Id == request.Id);
            updatedRequest.state = newState;
            updatedRequest.OwnerExplanationForDeclining = request.OwnerExplanationForDeclining;
            if(newState == State.Approved)
                UpdateReservationDates(request);
            return requestRepository.Update(updatedRequest);
        }

        private bool IsReservationCancelled(AccommodationReservation reservation)
        {
            CancelledAccommodationReservationService cancelledReservationService = new CancelledAccommodationReservationService();
            return cancelledReservationService.IsCancelled(reservation);
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