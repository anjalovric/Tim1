using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.Service
{
    public class CancelAccommodationReservationService
    {
        private CancelledAccommodationReservationRepository cancelledAccommodationReservationRepository;
        private Guest1 guest1;
        private List<AccommodationReservation> allCancelledReservations;
        private LocationService locationService;
        private AccommodationTypeService accommodationTypeService;
        private AccommodationService accommodationService;
        private OwnerService ownerService;


        public CancelAccommodationReservationService()
        {
            cancelledAccommodationReservationRepository = new CancelledAccommodationReservationRepository();
            accommodationService = new AccommodationService();
            ownerService = new OwnerService();
            locationService = new LocationService();
            accommodationTypeService = new AccommodationTypeService();
            FillAccommodationReservations();
        }

        public void Add(AccommodationReservation reservation)
        {
            cancelledAccommodationReservationRepository.Add(reservation);
        }

        public List<AccommodationReservation> GetAll()
        {
            return cancelledAccommodationReservationRepository.GetAll();
        }
        private void FillAccommodationsToReservations()
        {
            foreach (AccommodationReservation reservation in allCancelledReservations)
            {
                reservation.Accommodation = accommodationService.GetById(reservation.Accommodation.Id);
            }
        }
        public void FillAccommodationReservations()
        {
            allCancelledReservations = new List<AccommodationReservation>(cancelledAccommodationReservationRepository.GetAll());

            FillAccommodationsToReservations();
            FillOwnerToAccommodationReservations();
            SetAccommodationTypes();
            SetAccommodationLocations();
            AddGuests();
        }
        private void FillOwnerToAccommodationReservations()
        {
            foreach (AccommodationReservation reservation in allCancelledReservations)
            {
                reservation.Accommodation.Owner = ownerService.GetById(reservation.Accommodation.Owner.Id);
            }
        }

        private void SetAccommodationLocations()
        {
            List<Location> locations = locationService.GetAll();
            foreach (AccommodationReservation reservation in allCancelledReservations)
            {
                reservation.Accommodation.Location = locationService.GetById(reservation.Accommodation.Location.Id);
            }
        }

        private void SetAccommodationTypes()
        {
            List<AccommodationType> types = accommodationTypeService.GetAll();
            foreach (AccommodationReservation reservation in allCancelledReservations)
            {
                if (accommodationTypeService.GetById(reservation.Accommodation.Type.Id) != null)
                {
                    reservation.Accommodation.Type = accommodationTypeService.GetById(reservation.Accommodation.Type.Id);
                }
            }
        }

        private void AddGuests()
        {
            Guest1Service guest1Service = new Guest1Service();
            List<Guest1> allGuest = guest1Service.GetAll();
            foreach (AccommodationReservation reservation in allCancelledReservations)
            {
                Guest1 guestForReservation = allGuest.Find(n => n.Id == reservation.Guest.Id);
                if (guestForReservation != null)
                {
                    reservation.Guest = guestForReservation;
                }
            }
        }

        public bool IsCancellationAllowed(AccommodationReservation SelectedNotCompletedReservation)
        {
            return DateTime.Now <= SelectedNotCompletedReservation.Arrival.AddHours(-24) && DateTime.Now <= SelectedNotCompletedReservation.Arrival.AddDays(-SelectedNotCompletedReservation.Accommodation.MinDaysToCancel);
        }

        public MessageBoxResult ConfirmCancellationMessageBox()           
        {
            string sMessageBoxText = $"Do you want to cancel this reservation?\n";
            string sCaption = "Cancel reservation";
            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Question;
            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

    }
}
