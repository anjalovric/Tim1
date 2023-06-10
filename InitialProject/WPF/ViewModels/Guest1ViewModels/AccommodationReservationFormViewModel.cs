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
            Initialize();
            MakeCommands();
        }
        private void Initialize()
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
        
        //execute commands
        private void Back_Executed(object sender)
        {
            Application.Current.Windows.OfType<AccommodationReservationFormView>().FirstOrDefault().Close();
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

        private void Next_Executed(object sender)
        {
            lengthOfStay = Departure.Subtract(Arrival);

            if (!IsValidDateInput())
                ShowMessageBoxForInvalidDateInput();

            else if (!IsEnteredCorrectDateRange())
                ShowMessageBoxForIncorrectDateRange();
           
            else if (!IsEnteredCorrectGuestsNumber())
                ShowMessageBoxForIncorrectGuestsNumber();
                
            else
                OpenWindowWithAvailableDates();                 
        }
        
        //other methods
        private void OpenWindowWithAvailableDates()
        {
            SuggestedDatesForAccommodationReservationService suggestedDatesForAccommodationReservationService = new SuggestedDatesForAccommodationReservationService();
            suggestedDatesForAccommodationReservationService.TakeInputParameters(Arrival, Departure, NumberOfDays, NumberOfGuests);
            List<AvailableDatesForAccommodation> availableDates = new List<AvailableDatesForAccommodation>(suggestedDatesForAccommodationReservationService.GetAvailableDates(currentAccommodation, guest1));
            DatesForAccommodationReservationView datesForAccommodationReservationView = new DatesForAccommodationReservationView(currentAccommodation, guest1, availableDates);
            datesForAccommodationReservationView.Owner = Application.Current.Windows.OfType<AccommodationReservationFormView>().FirstOrDefault();
            datesForAccommodationReservationView.ShowDialog();
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
        //Validation - date input (calendars and num. of days)
        private bool IsValidDateInput()
        {
            return (Arrival <= Departure && Convert.ToInt32(lengthOfStay.TotalDays) >= (NumberOfDays - 1) && Arrival.Date > DateTime.Now && Arrival != DateTime.MinValue && Departure != DateTime.MinValue);
        }
        //Validation - Owner-s conditions for min. num. of days
        private bool IsEnteredCorrectDateRange()
        {
            return ((Convert.ToInt32(lengthOfStay.TotalDays) + 1) >= currentAccommodation.MinDaysForReservation && NumberOfDays >= currentAccommodation.MinDaysForReservation);
        }
        //Validation Owner's conditions for capacity
        private bool IsEnteredCorrectGuestsNumber()
        {
            return NumberOfGuests <= currentAccommodation.Capacity;
        }

        //Validation Message boxes
        private void ShowMessageBoxForInvalidDateInput()
        {
            Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("Invalid input, please enter values again!", "/Resources/Images/exclamation.png");
            messageBox.Owner = Application.Current.Windows.OfType<AccommodationReservationFormView>().FirstOrDefault();
            messageBox.ShowDialog();
        }
        private void ShowMessageBoxForIncorrectDateRange()
        {
            Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("The minimum number of days for booking this accommodation is " + currentAccommodation.MinDaysForReservation.ToString() + ".", "/Resources/Images/exclamation.png");
            messageBox.Owner = Application.Current.Windows.OfType<AccommodationReservationFormView>().FirstOrDefault();
            messageBox.ShowDialog();
        }
        private void ShowMessageBoxForIncorrectGuestsNumber()
        {
            Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("The maximum number of guests for this accommodation is " + currentAccommodation.Capacity.ToString() + ".", "/Resources/Images/exclamation.png");
            messageBox.Owner = Application.Current.Windows.OfType<AccommodationReservationFormView>().FirstOrDefault();
            messageBox.ShowDialog();
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
