using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Domain.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest1Views;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using InitialProject.APPLICATION.UseCases;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class AnywhereAnytimeViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private Guest1 guest1;
        private AnywhereAnytimeSuggestedReservationService anywhereAnytimeSuggestedReservationService;
        private AccommodationReservationService accommodationReservationService;
        private SuperGuestTitleService superGuestTitleService;
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }
        private ObservableCollection<SuggestedReservationViewModel> suggestedReservations;
        public ObservableCollection<SuggestedReservationViewModel> SuggestedReservations
        {
            get { return suggestedReservations; }
            set
            {
                if (value != suggestedReservations)
                {
                    suggestedReservations = value;
                    OnPropertyChanged("SuggestedReservations");
                }
            }
        }
        private string numberOfDays;
        public string NumberOfDays
        {
            get { return numberOfDays; }
            set
            {
                if (!value.Equals(numberOfDays))
                {
                    numberOfDays = value;
                    OnPropertyChanged("NumberOfDays");
                }
            }
        }
        private string numberOfGuests;
        public string NumberOfGuests
        {
            get { return numberOfGuests; }
            set
            {
                if (!value.Equals(numberOfGuests))
                {
                    numberOfGuests = value;
                    OnPropertyChanged("NumberOfGuests");
                }
            }
        }


        //Validation for Number of guests and Number od days fields
        public string Error => null;
        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }
        private bool isInputValid;
        public bool IsInputValid
        {
            get { return isInputValid; }
            set
            {
                if (value != isInputValid)
                {
                    isInputValid = value;
                    OnPropertyChanged("IsInputValid");
                }
            }
        }
        private bool isNumberOfDaysValid;
        public bool IsNumberOfDaysValid
        {
            get { return isNumberOfDaysValid; }
            set
            {
                if (value != isNumberOfDaysValid)

                {
                    isNumberOfDaysValid = value;
                    OnPropertyChanged("IsNumberOfDaysValid");
                }
            }
        }
        private bool isNumberOfGuestsValid;
        public bool IsNumberOfGuestsValid
        {
            get { return isNumberOfGuestsValid; }
            set
            {
                if (value != isNumberOfGuestsValid)
                {
                    isNumberOfGuestsValid = value;
                    OnPropertyChanged("IsNumberOfGuestsValid");
                }
            }
        }
        public override string ToString()
        {
            return $"{NumberOfDays} {NumberOfGuests}";
        }
        private readonly string[] _validatedProperties = { "NumberOfDays", "NumberOfGuests" };
        public string this[string columnName]
        {
            get
            {
                if (columnName == "NumberOfDays")
                {
                    var content = NumberOfDays;
                    Match match = CreateValidationNumberRegex(content);
                    if (!match.Success && NumberOfDays != "")
                    {
                        IsNumberOfDaysValid = false;
                        IsInputValid = false;
                        return "Enter an integer greater than zero.";
                    }
                    IsNumberOfDaysValid = true;
                    if (IsNumberOfGuestsValid)
                        IsInputValid = true;
                }

                if (columnName == "NumberOfGuests")
                {
                    var content = NumberOfGuests;
                    Match match = CreateValidationNumberRegex(content);
                    if (!match.Success && NumberOfGuests != "")
                    {
                        IsInputValid = false;
                        IsNumberOfGuestsValid = false;
                        return "Enter an integer greater than zero.";
                    }

                    IsNumberOfGuestsValid = true;
                    if (IsNumberOfDaysValid)
                        IsInputValid = true;
                }
                return null;
            }
        }
        private TimeSpan lengthOfStay;
        public RelayCommand ReserveCommand { get; set; }
        public RelayCommand DetailsCommand { get; set; }
        public RelayCommand ResetCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand IncrementGuestsNumberCommand { get; set; }
        public RelayCommand IncrementDaysNumberCommand { get; set; }
        public RelayCommand DecrementGuestsNumberCommand { get; set; }
        public RelayCommand DecrementDaysNumberCommand { get; set; }
        public RelayCommand OnPreviewMouseUpCommand { get; set; }


        public AnywhereAnytimeViewModel(Guest1 guest1)
        {
            this.guest1 = guest1;
            Initialize();
            MakeCommands();
        }
        private void Initialize()
        {
            NumberOfDays = "";
            NumberOfGuests = "";
            IsInputValid = true;
            IsNumberOfDaysValid = true;
            IsNumberOfGuestsValid = true;
            anywhereAnytimeSuggestedReservationService = new AnywhereAnytimeSuggestedReservationService();
            accommodationReservationService = new AccommodationReservationService();
            superGuestTitleService = new SuperGuestTitleService();
        }

        private void MakeCommands()
        {
            ResetCommand = new RelayCommand(Reset_Executed, CanExecute);
            IncrementDaysNumberCommand = new RelayCommand(IncrementDaysNumber_Executed, CanExecute);
            IncrementGuestsNumberCommand = new RelayCommand(IncrementGuestsNumber_Executed, CanExecute);
            DecrementDaysNumberCommand = new RelayCommand(DecrementDaysNumber_Executed, CanExecute);
            DecrementGuestsNumberCommand = new RelayCommand(DecrementGuestsNumber_Executed, CanExecute);
            OnPreviewMouseUpCommand = new RelayCommand(OnPreviewMouseUp_Executed, CanExecute);
            SearchCommand = new RelayCommand(Search_Executed, CanExecute);
            ReserveCommand = new RelayCommand(Reserve_Executed, CanExecute);
            DetailsCommand = new RelayCommand(Details_Executed, CanExecute);
        }

        //execute commands
        private void Details_Executed(object sender)
        {
            Accommodation currentAccommodation = (((Button)sender).DataContext as SuggestedReservationViewModel).Accommodation;
            DateTime arrival = (((Button)sender).DataContext as SuggestedReservationViewModel).Arrival;
            DateTime departure = (((Button)sender).DataContext as SuggestedReservationViewModel).Departure;

            AccommodationDetailsView details = new AccommodationDetailsView(currentAccommodation, guest1, arrival, departure);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = details;
 
        }
        private async void Reserve_Executed(object sender)
        {
            Task<bool> result = ConfirmReservationMessageBox();
            bool IsYesClicked = await result;
            if (IsYesClicked)
            {
                Accommodation currentAccommodation = (((Button)sender).DataContext as SuggestedReservationViewModel).Accommodation;
                DateTime arrival = (((Button)sender).DataContext as SuggestedReservationViewModel).Arrival;
                DateTime departure = (((Button)sender).DataContext as SuggestedReservationViewModel).Departure;
                MakeNewReservation(currentAccommodation, arrival, departure);
                if (Arrival != DateTime.MinValue && Departure != DateTime.MinValue)
                    UpdateSuggestedReservations();   //update suggested reservations list
                else
                    UpdateSuggestedReservationsNoInputDates();
                ShowMessageBoxForSentReservation();
            }
            
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
        private void DecrementGuestsNumber_Executed(object sender)
        {
            int changedGuestsNumber;
            if (NumberOfGuests != "" && Convert.ToInt32(NumberOfGuests) > 1)
            {
                changedGuestsNumber = Convert.ToInt32(NumberOfGuests) - 1;
                NumberOfGuests = changedGuestsNumber.ToString();
            }
        }
        private void IncrementGuestsNumber_Executed(object sender)
        {
            int changedGuestsNumber;
            if (NumberOfGuests == "")
                NumberOfGuests = "1";
            else
            {
                changedGuestsNumber = Convert.ToInt32(NumberOfGuests) + 1;
                NumberOfGuests = changedGuestsNumber.ToString();
            }
        }
        private void DecrementDaysNumber_Executed(object sender)
        {
            int changedDaysNumber;
            if (NumberOfDays != "" && Convert.ToInt32(NumberOfDays) > 1)
            {
                changedDaysNumber = Convert.ToInt32(NumberOfDays) - 1;
                NumberOfDays = changedDaysNumber.ToString();
            }
        }
        private void IncrementDaysNumber_Executed(object sender)
        {
            int changedDaysNumber;
            if (NumberOfDays == "")
                NumberOfDays = "1";
            else
            {
                changedDaysNumber = Convert.ToInt32(NumberOfDays) + 1;
                NumberOfDays = changedDaysNumber.ToString();
            }
        }

        private void Reset_Executed(object sender)
        {
            AnywhereAnytimeView view = new AnywhereAnytimeView(guest1);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = view;

        }

        private void Search_Executed(object sender)
        {
            if (AreTextboxesDataValid())
            {
                if(Arrival!=DateTime.MinValue || Departure!=DateTime.MinValue)//if calendar is selected
                {
                    lengthOfStay = Departure.Subtract(Arrival);
                    if (IsValidDateInput())  //all fields are filled
                        UpdateSuggestedReservations();
                    else
                        ShowMessageBoxForInvalidDateInput();
                }
                else //only textbox input 
                    UpdateSuggestedReservationsNoInputDates();
            }
            else
                ShowMessageBoxForInvalidInput();
        }

        //other methods
        private void SortSuggestedReservationsBySuperOwners()
        {
            SuggestedReservations = new ObservableCollection<SuggestedReservationViewModel>(SuggestedReservations.OrderByDescending(x => x.Accommodation.Owner.IsSuperOwner).ToList());
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
        private void MakeNewReservation(Accommodation currentAccommodation, DateTime arrival, DateTime departure)
        {
            AccommodationReservation newReservation = new AccommodationReservation(guest1, currentAccommodation, arrival, departure);
            accommodationReservationService.Add(newReservation);
            superGuestTitleService.DecrementPoints(guest1);
        }
        private void UpdateSuggestedReservationsNoInputDates()
        {
            SuggestedReservations = new ObservableCollection<SuggestedReservationViewModel>();
            SuggestedReservations.Clear();
            List<Tuple<Accommodation, AvailableDatesForAccommodation>> allSuggestedDates = anywhereAnytimeSuggestedReservationService.GetAvailableDatesNoInputDates(Convert.ToInt32(NumberOfDays), Convert.ToInt32(NumberOfGuests));
            foreach (Tuple<Accommodation, AvailableDatesForAccommodation> suggested in allSuggestedDates)
            {
                SuggestedReservations.Add(new SuggestedReservationViewModel(suggested.Item1, suggested.Item2.Arrival, suggested.Item2.Departure));
            }
            SortSuggestedReservationsBySuperOwners();
        }
        private void UpdateSuggestedReservations()
        {
            SuggestedReservations = new ObservableCollection<SuggestedReservationViewModel>();
            SuggestedReservations.Clear();
            List<Tuple<Accommodation, AvailableDatesForAccommodation>> allSuggestedDates = anywhereAnytimeSuggestedReservationService.GetAvailableDates(Arrival, Departure, Convert.ToInt32(NumberOfDays), Convert.ToInt32(NumberOfGuests));
            foreach (Tuple<Accommodation, AvailableDatesForAccommodation> suggested in allSuggestedDates)
            {
                SuggestedReservations.Add(new SuggestedReservationViewModel(suggested.Item1, suggested.Item2.Arrival, suggested.Item2.Departure));
            }
            SortSuggestedReservationsBySuperOwners();
        }
       
        

        //Validation for textboxes
        private Match CreateValidationNumberRegex(string content)
        {
            var regex = "^([1-9][0-9]*)$";
            Match match = Regex.Match(content, regex, RegexOptions.IgnoreCase);
            return match;
        }
        //Validation - date input (calendars and num. of days)
        private bool IsValidDateInput() //if only calendar are filled (treba prije poziva ove metode provjera da li je bar 1 kalendar selektovan)
        {
            return (Arrival != DateTime.MinValue && Departure != DateTime.MinValue && Arrival <= Departure && Arrival.Date > DateTime.Now && Convert.ToInt32(lengthOfStay.TotalDays) >= (Convert.ToInt32(NumberOfDays) - 1));
        }
        //Validation - all textboxes are empty
        private bool AreTextboxesDataValid()
        {
            return NumberOfGuests != "" && NumberOfDays != "";
        }

        //Message box - all textboxes are empty
        private void ShowMessageBoxForInvalidInput()
        {
            Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("Please enter number of days and guests (and/or choose dates)!", "/Resources/Images/exclamation.png");
            messageBox.Owner = Application.Current.Windows.OfType<AccommodationReservationFormView>().FirstOrDefault();
            messageBox.ShowDialog();
        }

        //Message box - invalid date input
        private void ShowMessageBoxForInvalidDateInput()
        {
            Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("Please enter valid dates in calendar!", "/Resources/Images/exclamation.png");
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
