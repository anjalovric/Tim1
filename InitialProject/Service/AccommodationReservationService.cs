using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;

namespace InitialProject.Service
{//sta sa cancelled repoz.koji pravim i ovdje? da li da bude injector?
    public class AccommodationReservationService
    {
        private Guest1 guest1;
        private List<AccommodationReservation> storedReservations;
        public List<AccommodationReservation> CompletedReservations;
        public List<AccommodationReservation> NotCompletedReservations;
        private IAccommodationReservationRepository accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();

        public AccommodationReservationService()
        {
            NotCompletedReservations = new List<AccommodationReservation>();
            CompletedReservations = new List<AccommodationReservation>();
            SetAccommodationReservations();
        }
        public List<AccommodationReservation> GetAll()
        {
            return storedReservations;
        }
        public void SetAccommodationReservations()
        {
            storedReservations = new List<AccommodationReservation>(accommodationReservationRepository.GetAll());
            SetAccommodationsToReservations();
            SetOwnerToAccommodationReservations();
            SetAccommodationTypes();
            SetAccommodationLocations();
            SetGuests();
        }
        public void Add(AccommodationReservation reservation)
        {
            accommodationReservationRepository.Add(reservation);
        }

        
        public List<AccommodationReservation> GetCompletedReservations(Guest1 guest1)
        {
            SetAccommodationReservations();
            CompletedReservations = new List<AccommodationReservation>();
            foreach (AccommodationReservation reservation in storedReservations)
            {
                if (reservation.Departure < DateTime.Now && reservation.Guest.Id == guest1.Id)
                {
                    CompletedReservations.Add(reservation);
                }
            }
            return CompletedReservations;
        }
        public List<AccommodationReservation> GetNotCompletedReservations(Guest1 guest1)
        {
            SetAccommodationReservations();
            NotCompletedReservations = new List<AccommodationReservation>();
            foreach (AccommodationReservation reservation in storedReservations)
            {
                if (reservation.Departure >= DateTime.Now && reservation.Guest.Id == guest1.Id)
                {
                    NotCompletedReservations.Add(reservation);
                }
            }
            return NotCompletedReservations;
        }
        private void SetOwnerToAccommodationReservations()
        {
            OwnerService ownerService = new OwnerService();
            foreach (AccommodationReservation reservation in storedReservations)
            {
                reservation.Accommodation.Owner = ownerService.GetById(reservation.Accommodation.Owner.Id);
            }
        }
        private void SetAccommodationsToReservations()
        {
            AccommodationService accommodationService = new AccommodationService();
            foreach (AccommodationReservation reservation in storedReservations)
            {
                reservation.Accommodation = accommodationService.GetById(reservation.Accommodation.Id);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void SetAccommodationLocations()
        {
            LocationService locationService = new LocationService();

            List<Location> locations = locationService.GetAll();
            foreach (AccommodationReservation reservation in storedReservations)
            {
                reservation.Accommodation.Location = locationService.GetById(reservation.Accommodation.Location.Id);
            }
            
        }
        
        
        private void SetAccommodationTypes()
        {
            AccommodationTypeService accommodationTypeService = new AccommodationTypeService();
            List<AccommodationType> types = accommodationTypeService.GetAll();
            foreach (AccommodationReservation reservation in storedReservations)
            {
                if (accommodationTypeService.GetById(reservation.Accommodation.Type.Id) != null)
                {
                    reservation.Accommodation.Type = accommodationTypeService.GetById(reservation.Accommodation.Type.Id);
                }
            }
        }
        public void Delete(AccommodationReservation accommodationReservation)
        {
            accommodationReservationRepository.Delete(accommodationReservation);
        }
        private void SetGuests()
        {
            Guest1Service guest1Service = new Guest1Service();
            List<Guest1> allGuest = guest1Service.GetAll(); 
            foreach(AccommodationReservation reservation in storedReservations)
            {
                Guest1 guestForReservation = allGuest.Find(n => n.Id == reservation.Guest.Id);
                    if(guestForReservation != null)
                    {
                        reservation.Guest = guestForReservation;
                    }
            }
        }
        public bool IsCancelled(AccommodationReservation reservation)
        {
            CancelledAccommodationReservationService cancelledReservationService = new CancelledAccommodationReservationService();
            List<AccommodationReservation> allCancelledReservations = cancelledReservationService.GetAll();
            return allCancelledReservations.Find(n => n.Id == reservation.Id) != null; 
        }
        public void Update(AccommodationReservation reservation)
        {
            accommodationReservationRepository.Update(reservation);
        }
        public bool IsAvailableInDateRange(AccommodationReservation reservation, DateTime startDate, DateTime endDate)
        {
            DateTime date = startDate;
            while (date<=endDate)
            {
                if (!IsAvailableOnDate(reservation, date))
                    return false;
                date = date.AddDays(1);
            }
            return true;
        }
        private bool IsAvailableOnDate(AccommodationReservation reservationToCheck, DateTime date)
        {
            bool isAvailable = true;
            foreach(var reservation in storedReservations)
            {
                bool isSameAccommodation = reservation.Accommodation.Id == reservationToCheck.Accommodation.Id;
                bool isSameGuest = reservation.Guest.Id == reservationToCheck.Guest.Id;
                if (isSameAccommodation && !isSameGuest)
                    isAvailable = isAvailable && !(date > reservation.Arrival && date < reservation.Departure);
            }
            return isAvailable;
        }

        public List<AccommodationReservation> GetAllForReviewByOwner(Owner owner)
        {
            List<AccommodationReservation> reservationsToReview = new List<AccommodationReservation>();
            GuestReviewService guestReviewService = new GuestReviewService();

            if (storedReservations.Count > 0)
            {
                foreach (AccommodationReservation reservation in storedReservations)
                {
                    bool stayedLessThan5DaysAgo = (reservation.Departure.Date < DateTime.Now.Date) && (DateTime.Now.Date - reservation.Departure.Date).TotalDays <= 5;
                    bool alreadyReviewed = guestReviewService.HasReview(reservation);
                    bool isThisOwner = reservation.Accommodation.Owner.Id == owner.Id;

                    if (stayedLessThan5DaysAgo && !alreadyReviewed && isThisOwner)
                        reservationsToReview.Add(reservation);
                }
            }
            return reservationsToReview;
        }
    }
}
//92 linija
//15-16 metoda