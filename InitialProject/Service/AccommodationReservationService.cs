using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.RepositoryInterfaces;
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
        private OwnerReviewService ownerReviewService;
        public ObservableCollection<AccommodationReservation> NotFinishedReservations;
        public ObservableCollection<AccommodationReservation> CompletedAccommodationReservations;

        private AccommodationReservationRepository accommodationReservationRepository;
        public AccommodationReservationService()
        {
            accommodationReservationRepository = new AccommodationReservationRepository();

            accommodationService = new AccommodationService();
            ownerService = new OwnerService();
            ownerReviewService = new OwnerReviewService();
            locationService = new LocationService();
            accommodationTypeService = new AccommodationTypeService();
            allReservations = new List<AccommodationReservation>(accommodationReservationRepository.GetAll());

            FillAccommodationsToReservations();
            FillOwnerToAccommodationReservations();
            SetAccommodationTypes();
            SetAccommodationLocations();
        }

        public List<AccommodationReservation> GetAll()
        {
            return accommodationReservationRepository.GetAll();
        }



        public void Add(AccommodationReservation reservation)
        {
            accommodationReservationRepository.Add(reservation);
        }

        public ObservableCollection<AccommodationReservation> FillUpcomingAndCurrentReservations(Guest1 guest1)
        {
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


    }
}
