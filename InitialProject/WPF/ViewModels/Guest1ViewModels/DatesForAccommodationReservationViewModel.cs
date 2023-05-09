﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest1Views;
using InitialProject.WPF.Views.Guest2Views;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class DatesForAccommodationReservationViewModel : INotifyPropertyChanged
    {
        private Guest1 guest1;
        private Accommodation currentAccommodation;
        AccommodationReservationService accommodationReservationService;
        public ObservableCollection<AvailableDatesForAccommodation> availableDatesForAccommodations { get; set; }
        private AvailableDatesForAccommodation selectedDateRange;
        public AvailableDatesForAccommodation SelectedDateRange
        {
            get { return selectedDateRange; }
            set
            {
                if (selectedDateRange != value)
                {
                    selectedDateRange = value;
                    this.OnPropertyChanged("SelectedDateRange");
                }
            }
        }
        public RelayCommand ChooseDateCommand { get; set; }
        public RelayCommand BackCommand { get; set; }

        public DatesForAccommodationReservationViewModel(Guest1 guest1, Accommodation currentAccommodation, List<AvailableDatesForAccommodation> availableDates)
        {
            this.guest1 = guest1;
            this.currentAccommodation = currentAccommodation;
            accommodationReservationService = new AccommodationReservationService();
            availableDatesForAccommodations = new ObservableCollection<AvailableDatesForAccommodation>(availableDates);
            ChooseDateCommand = new RelayCommand(ChooseDate_Executed, CanExecute);
            BackCommand = new RelayCommand(Back_Executed, CanExecute);
        }
        private void Back_Executed(object sender)
        {
            Application.Current.Windows.OfType<DatesForAccommodationReservationView>().FirstOrDefault().Close();
        }
        
        private async void ChooseDate_Executed(object sender)
        {
            Task<bool> result = ConfirmReservationMessageBox();
            bool IsYesClicked = await result;
            if(IsYesClicked)
                MakeNewReservation();
        }
        public async Task<bool> ConfirmReservationMessageBox()
        {
            var result = new TaskCompletionSource<bool>();
            Guest1YesNoMessageBoxView messageBox = new Guest1YesNoMessageBoxView("Do you want to make a reservation?", "/Resources/Images/qm.png", result);
            messageBox.Owner = Application.Current.Windows.OfType<DatesForAccommodationReservationView>().FirstOrDefault();
            messageBox.ShowDialog();
            var returnedResult = await result.Task;
            return returnedResult;
        }
       
        
        private void ShowMessageBoxForSentReservation()
        {
            Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("Successfully done!", "/Resources/Images/done.png");
            messageBox.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
            messageBox.ShowDialog();
        }

        private void MakeNewReservation()
        { 
            AccommodationReservation newReservation = new AccommodationReservation(guest1, currentAccommodation, selectedDateRange.Arrival, selectedDateRange.Departure);
            accommodationReservationService.Add(newReservation);
            DecrementSuperGuestPoints();
            Application.Current.Windows.OfType<DatesForAccommodationReservationView>().FirstOrDefault().Close();
            Application.Current.Windows.OfType<AccommodationReservationFormView>().FirstOrDefault().Close();
            ShowMessageBoxForSentReservation();           
        }
        private void DecrementSuperGuestPoints()
        {
            SuperGuestTitleService superGuestTitleService = new SuperGuestTitleService();
            superGuestTitleService.DecrementPoints(guest1);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private bool CanExecute(object sender)
        {
            return true;
        }

    }
}
