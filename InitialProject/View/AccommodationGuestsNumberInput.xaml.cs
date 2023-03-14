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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for AccommodationGuestsNumberInput.xaml
    /// </summary>
    public partial class AccommodationGuestsNumberInput : Window
    {
        public Accommodation currentAccommodation { get; set; }
        public AvailableDatesForAccommodationReservation selectedDateRange { get; set; }
        private AccommodationReservationRepository accommodationReservationRepository;
        public ObservableCollection<AvailableDatesForAccommodationReservation> availableDatesForAccommodations { get; set; }
        public AccommodationGuestsNumberInput(Accommodation currentAccommodation, AvailableDatesForAccommodationReservation selectedDateRange, AccommodationReservationRepository accommodationReservationRepository, ObservableCollection<AvailableDatesForAccommodationReservation> availableDatesForAccommodations)
        {
            InitializeComponent();
            this.DataContext = this;
            this.currentAccommodation = currentAccommodation;
            this.selectedDateRange = selectedDateRange;
            this.accommodationReservationRepository = accommodationReservationRepository;
            this.availableDatesForAccommodations = availableDatesForAccommodations;
        }

        private void ConfirmReservation_Click(object sender, RoutedEventArgs e)
        {
            if(Convert.ToInt32(numberOfGuests.Text) > currentAccommodation.Capacity)
            {
                MessageBox.Show("Maximum number of guests for this accommodation is " + currentAccommodation.Capacity.ToString()+ ".");  
            }
            else
            {
                MessageBoxResult result = ConfirmReservation();
                if(result == MessageBoxResult.Yes)
                {
                    makeNewReservation();
                    UpdateAvailableDates();
                    this.Close();
                    this.Owner.Close();    
                }
                else if(result == MessageBoxResult.No)
                {
                    this.Close();
                    this.Owner.Activate();
                }
            }
        }

        private void makeNewReservation()
        {
            AccommodationReservation newReservation = new AccommodationReservation(1, currentAccommodation, selectedDateRange.Start, selectedDateRange.End);
            accommodationReservationRepository.Add(newReservation);
        }

        private void UpdateAvailableDates()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(AccommodationReservationForm))
                {
                    (window as AccommodationReservationForm).GetAvailableDates(currentAccommodation.Id);
                }
            }
           
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
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void DecrementGuestsNumber(object sender, RoutedEventArgs e)
        {
            int changedGuestsNumber;
            if (Convert.ToInt32(numberOfGuests.Text) > 1)
            {
                changedGuestsNumber = Convert.ToInt32(numberOfGuests.Text) - 1;
                numberOfGuests.Text = changedGuestsNumber.ToString();
            }
        }
        private void IncrementGuestsNumber(object sender, RoutedEventArgs e)
        {
            int changedGuestsNumber;
            changedGuestsNumber = Convert.ToInt32(numberOfGuests.Text) + 1;
            numberOfGuests.Text = changedGuestsNumber.ToString();
        }  
    }
}
