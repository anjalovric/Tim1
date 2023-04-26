using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest1Views;
using System.Windows.Media.Imaging;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using Microsoft.VisualBasic;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class AccommodationReservationFormViewModel : INotifyPropertyChanged
    {
        private Guest1 guest1;
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }
        public TimeSpan lengthOfStay { get; set; }
        public Accommodation currentAccommodation { get; set; }
        public List<AccommodationReservation> reservations { get; set; }

        private int numberOfDays;
      
        public int NumberOfDays
        {
            get { return numberOfDays; }
            set
            {
                if (value != numberOfDays)
                    numberOfDays = value;
                OnPropertyChanged("NumberOfDays");
            }

        }
        private int numberOfGuests;

        public int NumberOfGuests
        {
            get { return numberOfGuests; }
            set
            {
                if (value != numberOfGuests)
                    numberOfGuests = value;
                OnPropertyChanged("NumberOfGuests");
            }

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

        public RelayCommand DecrementDaysNumberCommand { get; set; }
        public RelayCommand IncrementDaysNumberCommand { get; set; }
        public RelayCommand DecrementGuestsNumberCommand { get; set; }
        public RelayCommand IncrementGuestsNumberCommand { get; set; }
        public RelayCommand NextCommand { get; set; }
        public RelayCommand BackCommand { get; set; }
        public RelayCommand OnPreviewMouseUpCommand { get; set; }
        public AccommodationReservationFormViewModel(Guest1 guest1, Accommodation currentAccommodation)
        {
            this.guest1 = guest1;
            this.guest1 = guest1;
            this.currentAccommodation = currentAccommodation;
            InitializeForm();
            MakeCommands();
        }
        private void Back_Executed(object sender)
        {
            Application.Current.Windows.OfType<AccommodationReservationFormView>().FirstOrDefault().Close();
        }

        private void InitializeForm()
        {
            NumberOfDays = 1;
            NumberOfGuests = 1;
            AccommodationReservationService accommodationReservationService = new AccommodationReservationService();
            this.reservations = new List<AccommodationReservation>(accommodationReservationService.GetAll());
            
        }
        private void MakeCommands()
        {
            DecrementDaysNumberCommand = new RelayCommand(DecrementDaysNumber_Executed, CanExecute);
            IncrementDaysNumberCommand = new RelayCommand(IncrementDaysNumber_Executed, CanExecute);
            DecrementGuestsNumberCommand = new RelayCommand(DecrementGuestsNumber_Executed, CanExecute);
            IncrementGuestsNumberCommand = new RelayCommand(IncrementGuestsNumber_Executed, CanExecute);
            NextCommand = new RelayCommand(Next_Executed, CanExecute);
            BackCommand = new RelayCommand(Back_Executed, CanExecute);
            OnPreviewMouseUpCommand = new RelayCommand(OnPreviewMouseUp_Executed, CanExecute);
        }
        
        
        private void DecrementDaysNumber_Executed(object sender)
        {
            int changedDaysNumber;
            if (NumberOfDays > 1)
            {
                changedDaysNumber = NumberOfDays - 1;
                NumberOfDays = changedDaysNumber;
            }
        }
        private void IncrementDaysNumber_Executed(object sender)
        {
            int changedDaysNumber;
            changedDaysNumber = NumberOfDays + 1;
            NumberOfDays = changedDaysNumber;
        }

        private void DecrementGuestsNumber_Executed(object sender)
        {
            int changedDaysNumber;
            if (NumberOfGuests > 1)
            {
                changedDaysNumber = NumberOfGuests - 1;
                NumberOfGuests = changedDaysNumber;
            }
        }
        private void IncrementGuestsNumber_Executed(object sender)
        {
            int changedDaysNumber;
            changedDaysNumber = NumberOfGuests + 1;
            NumberOfGuests = changedDaysNumber;
        }
        private bool IsValidDateInput()
        {
            return (Arrival <= Departure && Convert.ToInt32(lengthOfStay.TotalDays) >= (NumberOfDays - 1) && Arrival.Date > DateTime.Now && Arrival != null && Departure != null);
        }
        private bool IsEnteredCorrectDateRange()
        {
            return ((Convert.ToInt32(lengthOfStay.TotalDays) + 1) >= currentAccommodation.MinDaysForReservation && NumberOfDays >= currentAccommodation.MinDaysForReservation);
        }
        private bool IsEnteredCorrectGuestsNumber()
        {
            return NumberOfGuests <= currentAccommodation.Capacity;
        }
        private void Next_Executed(object sender)
        {
            lengthOfStay = Departure.Subtract(Arrival);

            if (!IsValidDateInput())
                MessageBox.Show("Non valid input, please enter values again!");
            else if (!IsEnteredCorrectDateRange())
                MessageBox.Show("The minimum number of days for booking this accommodation is " + currentAccommodation.MinDaysForReservation.ToString() + ".");
            else if (!IsEnteredCorrectGuestsNumber())
                MessageBox.Show("The maximum number of guests for this accommodation is " + currentAccommodation.Capacity.ToString() + ".");
            else
                OpenWindowWithAvailableDates();                 
        }
        private void OpenWindowWithAvailableDates()
        {
            SuggestedDatesForAccommodationReservationService suggestedDatesForAccommodationReservationService = new SuggestedDatesForAccommodationReservationService();
            suggestedDatesForAccommodationReservationService.TakeInputParameters(Arrival, Departure, NumberOfDays, NumberOfGuests);
            List<AvailableDatesForAccommodationReservation> availableDates = new List<AvailableDatesForAccommodationReservation>(suggestedDatesForAccommodationReservationService.GetAvailableDates(currentAccommodation, guest1));
            DatesForAccommodationReservationView datesForAccommodationReservationView = new DatesForAccommodationReservationView(currentAccommodation, guest1, availableDates);
            datesForAccommodationReservationView.Show();
        }


        private void OnPreviewMouseUp_Executed(Object sender)
        {
            OnPreviewMouseUp(null);
        }
        private void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            if (Mouse.Captured is CalendarItem)
            {
                Mouse.Capture(null);
            }
        }
    }
}
