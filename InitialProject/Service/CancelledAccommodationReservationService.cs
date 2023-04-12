using System;
using System.Collections.Generic;
using System.Windows;
using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;

namespace InitialProject.Service
{
    public class CancelledAccommodationReservationService
    {
        private ICancelledAccommodationReservationRepository cancelledAccommodationReservationRepository = Injector.CreateInstance<ICancelledAccommodationReservationRepository>();
        private List<AccommodationReservation> cancelledReservations;
        public CancelledAccommodationReservationService()
        {
            cancelledReservations = new List<AccommodationReservation>(cancelledAccommodationReservationRepository.GetAll());
            SetAccommodations();
        }

        private void SetAccommodations()
        {
            AccommodationService accommodationService = new AccommodationService();
            foreach (AccommodationReservation reservation in cancelledReservations)
            {
                reservation.Accommodation = accommodationService.GetById(reservation.Accommodation.Id);
            }
        }
        public void Add(AccommodationReservation reservation)
        {
            cancelledAccommodationReservationRepository.Add(reservation);
        }
        public List<AccommodationReservation> GetAll()
        {
            return cancelledReservations;
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
        public bool IsCancelled(AccommodationReservation reservation)
        {
            return cancelledReservations.Find(n => n.Id == reservation.Id) != null;
        }
    }
}