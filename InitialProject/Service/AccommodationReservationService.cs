using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Repository;

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
    }
}
