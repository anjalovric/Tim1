using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using InitialProject.Model;
using InitialProject.Repository;
using static NPOI.HSSF.Util.HSSFColor;

namespace InitialProject.Service
{
    public class AccommodationReservationService
    {
        private Guest1 guest1;
        private List<AccommodationReservation> allReservations;
        private LocationService locationService;
        private AccommodationTypeService accommodationTypeService;
        private AccommodationService accommodationService;
        private OwnerService ownerService;
        //private OwnerReviewService ownerReviewService;
        
        public ObservableCollection<AccommodationReservation> CompletedAccommodationReservations;
      /*  private ObservableCollection<AccommodationReservation> notFinishedReservations;
        public ObservableCollection<AccommodationReservation> NotFinishedReservations
        {
            get { return notFinishedReservations; }
            set
            {
                if (value != notFinishedReservations)
                    notFinishedReservations = value;
                OnPropertyChanged("NotFinishedReservations");
            }

        }*/

        private AccommodationReservationRepository accommodationReservationRepository;
        private List<AccommodationReservation> accommodationReservations;
        public AccommodationReservationService()
        {
            accommodationReservationRepository = new AccommodationReservationRepository();

            accommodationService = new AccommodationService();
            ownerService = new OwnerService();
            //ownerReviewService = new OwnerReviewService();
            locationService = new LocationService();
            accommodationTypeService = new AccommodationTypeService();
          //  NotFinishedReservations = new ObservableCollection<AccommodationReservation>();
            CompletedAccommodationReservations = new ObservableCollection<AccommodationReservation>();
            FillAccommodationReservations();
        }

        public List<AccommodationReservation> GetAll()
        {
            return allReservations;
        }

        public void FillAccommodationReservations()
        {
            allReservations = new List<AccommodationReservation>(accommodationReservationRepository.GetAll());

            FillAccommodationsToReservations();
            FillOwnerToAccommodationReservations();
            SetAccommodationTypes();
            SetAccommodationLocations();
            AddGuests();
        }

        public void Add(AccommodationReservation reservation)
        {
            accommodationReservationRepository.Add(reservation);
        }

        public ObservableCollection<AccommodationReservation> FillUpcomingAndCurrentReservations(Guest1 guest1, ObservableCollection<AccommodationReservation> NotFinishedReservations)
        {
            FillAccommodationReservations();
            NotFinishedReservations = new ObservableCollection<AccommodationReservation>();

            foreach (AccommodationReservation reservation in allReservations)
            {
                if (reservation.Departure >= DateTime.Now && reservation.Guest.Id == guest1.Id)
                {
                    NotFinishedReservations.Add(reservation);
                }
            }

            return NotFinishedReservations;
        }
        private void FillOwnerToAccommodationReservations()
        {
            foreach (AccommodationReservation reservation in allReservations)
            {
                reservation.Accommodation.Owner = ownerService.GetById(reservation.Accommodation.Owner.Id);
            }
        }

        public ObservableCollection<AccommodationReservation> FillCompletedReservations(Guest1 guest1)
        {
            FillAccommodationReservations();
            CompletedAccommodationReservations = new ObservableCollection<AccommodationReservation>();

            foreach (AccommodationReservation reservation in allReservations)
            {
                if (reservation.Departure < DateTime.Now && reservation.Guest.Id == guest1.Id)
                {
                    CompletedAccommodationReservations.Add(reservation);
                }
            }
            return CompletedAccommodationReservations;
        }

        private void FillAccommodationsToReservations()
        {
            foreach (AccommodationReservation reservation in allReservations)
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
            List<Location> locations = locationService.GetAll();
            foreach (AccommodationReservation reservation in allReservations)
            {
                reservation.Accommodation.Location = locationService.GetById(reservation.Accommodation.Location.Id);
            }
        }

        private void SetAccommodationTypes()
        {
            List<AccommodationType> types = accommodationTypeService.GetAll();
            foreach (AccommodationReservation reservation in allReservations)
            {
                if (accommodationTypeService.GetById(reservation.Accommodation.Type.Id) != null)
                {
                    reservation.Accommodation.Type = accommodationTypeService.GetById(reservation.Accommodation.Type.Id);
                }
            }
        }

        private void MakeReservations()
        {
            accommodationReservations = accommodationReservationRepository.GetAll();
            AddAccommodations();
            AddGuests();
        }

        private void AddAccommodations()
        {
            AccommodationService accommodationService = new AccommodationService();
            List<Accommodation> allAccommodation = accommodationService.GetAll();
            foreach(AccommodationReservation reservation in accommodationReservations)
            {
                Accommodation reservationAccommodation = allAccommodation.Find(n => n.Id == reservation.Accommodation.Id);
                if(reservationAccommodation != null)
                {
                    reservation.Accommodation = reservationAccommodation;
                }
            }
        }

        public void Delete(AccommodationReservation accommodationReservation)
        {
            accommodationReservationRepository.Delete(accommodationReservation);
        }

        private void AddGuests()
        {
            Guest1Service guest1Service = new Guest1Service();
            List<Guest1> allGuest = guest1Service.GetAll(); 
            foreach(AccommodationReservation reservation in allReservations)
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
            CancelAccommodationReservationService cancelReservationService = new CancelAccommodationReservationService();
            List<CancelledAccommodationReservation> allCancelledReservations = cancelReservationService.GetAll();
            return allCancelledReservations.Find(n => n.reservation.Id == reservation.Id) != null; 
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
            foreach(var reservation in allReservations)
            {
                bool isSameAccommodation = reservation.Accommodation.Id == reservationToCheck.Accommodation.Id;
                bool isSameGuest = reservation.Guest.Id == reservationToCheck.Guest.Id;
                if (isSameAccommodation && !isSameGuest)
                    isAvailable = isAvailable && !(date > reservation.Arrival && date < reservation.Departure);
            }
            return isAvailable;
        }
    }
}
