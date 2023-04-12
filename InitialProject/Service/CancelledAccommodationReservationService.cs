using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.Service
{
    public class CancelledAccommodationReservationService
    {
        private Guest1 guest1;
        private ICancelledAccommodationReservationRepository cancelledAccommodationReservationRepository = Injector.CreateInstance<ICancelledAccommodationReservationRepository>();
        private List<AccommodationReservation> storedCancelledReservations;

        public CancelledAccommodationReservationService()
        {
            SetCancelledAccommodationReservations();
        }
        public void Add(AccommodationReservation reservation)
        {
            cancelledAccommodationReservationRepository.Add(reservation);
        }
        public List<AccommodationReservation> GetAll()
        {
            return cancelledAccommodationReservationRepository.GetAll();
        }
        public void SetCancelledAccommodationReservations()
        {
            storedCancelledReservations = new List<AccommodationReservation>(cancelledAccommodationReservationRepository.GetAll());

            SetAccommodationsToReservations();
            SetOwnerToAccommodationReservations();
            SetAccommodationTypes();
            SetAccommodationLocations();
            SetGuests();
        }
        private void SetAccommodationsToReservations()
        {
            AccommodationService accommodationService = new AccommodationService();
            foreach (AccommodationReservation reservation in storedCancelledReservations)
            {
                reservation.Accommodation = accommodationService.GetById(reservation.Accommodation.Id);
            }
        }
        private void SetOwnerToAccommodationReservations()
        {
            OwnerService ownerService = new OwnerService();
            foreach (AccommodationReservation reservation in storedCancelledReservations)
            {
                reservation.Accommodation.Owner = ownerService.GetById(reservation.Accommodation.Owner.Id);
            }
        }
        private void SetAccommodationLocations()
        {
            LocationService locationService = new LocationService();
            List<Location> locations = locationService.GetAll();
            foreach (AccommodationReservation reservation in storedCancelledReservations)
            {
                reservation.Accommodation.Location = locationService.GetById(reservation.Accommodation.Location.Id);
            }
        }
        private void SetAccommodationTypes()
        {
            AccommodationTypeService accommodationTypeService = new AccommodationTypeService();
            List<AccommodationType> types = accommodationTypeService.GetAll();
            foreach (AccommodationReservation reservation in storedCancelledReservations)
            {
                if (accommodationTypeService.GetById(reservation.Accommodation.Type.Id) != null)
                {
                    reservation.Accommodation.Type = accommodationTypeService.GetById(reservation.Accommodation.Type.Id);
                }
            }
        }
        private void SetGuests()
        {
            Guest1Service guest1Service = new Guest1Service();
            List<Guest1> allGuest = guest1Service.GetAll();
            foreach (AccommodationReservation reservation in storedCancelledReservations)
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
//58 linija