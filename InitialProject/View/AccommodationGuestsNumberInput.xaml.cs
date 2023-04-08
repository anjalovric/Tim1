﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for AccommodationGuestsNumberInput.xaml
    /// </summary>
    public partial class AccommodationGuestsNumberInput : Window
    {
        public Accommodation currentAccommodation { get; set; }
        private AccommodationReservationService accommodationReservationService;
        private CancelledAccommodationReservationRepository cancelledAccommodationReservationRepository;

        private AvailableDatesForAccommodationReservation selectedDateRange;
        private Guest1 guest1;
        public ObservableCollection<AvailableDatesForAccommodationReservation> availableDatesForAccommodations { get; set; }
        public AccommodationGuestsNumberInput(Accommodation currentAccommodation, AvailableDatesForAccommodationReservation selectedDateRange, AccommodationReservationService accommodationReservationService, ObservableCollection<AvailableDatesForAccommodationReservation> availableDatesForAccommodations, Guest1 guest1)
        {
            InitializeComponent();
            this.DataContext = this;

            this.guest1 = guest1;
            this.currentAccommodation = currentAccommodation;
            this.accommodationReservationService = accommodationReservationService;
            cancelledAccommodationReservationRepository = new CancelledAccommodationReservationRepository();
            this.selectedDateRange = selectedDateRange;
            this.availableDatesForAccommodations = availableDatesForAccommodations;
        }

        private void ConfirmReservationButton_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(numberOfGuests.Text) > currentAccommodation.Capacity)
            {
                MessageBox.Show("Maximum number of guests for this accommodation is " + currentAccommodation.Capacity.ToString() + ".");
            }
            else
            {
                ContinueReservation();
            }
        }

       /* private Guest1 MakeNewGuest()
        {
            Guest1 guest = new Guest1("Anja", "Ducic");
            guest.Id = 1;
            return guest;
        }*/

        private void MakeNewReservation()
        {
            AccommodationReservation newReservation = new AccommodationReservation(guest1, currentAccommodation, selectedDateRange.Arrival, selectedDateRange.Departure);
            accommodationReservationService.Add(newReservation);
        }

        private MessageBoxResult ConfirmReservation()
        {
            string sMessageBoxText = $"Do you want to make a reservation?\n";
            string sCaption = "Confirm reservation";
            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Question;
            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }
        private void ContinueReservation()
        {
            MessageBoxResult result = ConfirmReservation();
            if (result == MessageBoxResult.Yes)
            {
                MakeNewReservation();
                this.Close();
                this.Owner.Close();
                this.Owner.Owner.Close();
            }
            else if (result == MessageBoxResult.No)
            {
                this.Close();
                this.Owner.Close();
            }
        }




        private void DecrementGuestsNumberButton_Click(object sender, RoutedEventArgs e)
        {
            int changedGuestsNumber;
            if (Convert.ToInt32(numberOfGuests.Text) > 1)
            {
                changedGuestsNumber = Convert.ToInt32(numberOfGuests.Text) - 1;
                numberOfGuests.Text = changedGuestsNumber.ToString();
            }
        }
        private void IncrementGuestsNumberButton_Click(object sender, RoutedEventArgs e)
        {
            int changedGuestsNumber;
            changedGuestsNumber = Convert.ToInt32(numberOfGuests.Text) + 1;
            numberOfGuests.Text = changedGuestsNumber.ToString();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
          
    }
}
