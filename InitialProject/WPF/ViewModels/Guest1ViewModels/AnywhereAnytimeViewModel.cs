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
using InitialProject.Service;
using InitialProject.WPF.Views.Guest1Views;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls.Primitives;


namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class AnywhereAnytimeViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private Guest1 guest1;
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
            NumberOfDays = "";
            NumberOfGuests = "";
            IsInputValid = true;
            IsNumberOfDaysValid = true;
            IsNumberOfGuestsValid = true;
            MakeCommands();
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

        private bool CanExecute(object sender)
        {
            return true;
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
            //Accommodations.Clear();
            // foreach (Accommodation accommodation in accommodationService.GetAll())
            //Accommodations.Add(accommodation);

            ResetAllSearchingFields();
            //SortAccommodationBySuperOwners();
        }
        private void ResetAllSearchingFields()
        {
            NumberOfDays = "";
            NumberOfGuests = "";
            IsInputValid = true;
            IsNumberOfDaysValid = true;
            IsNumberOfGuestsValid = true;
        }
        private void Search_Executed(object sender)
        {
            if (AreAllFieldsEmpty())
                ShowMessageBoxEmptyAll();
            else if(IsOnlyCalendarFilled())
            {

            }

        }

        
        private bool IsOnlyCalendarFilled()
        {
            return (Arrival != null || Departure != null) && NumberOfDays == "" && NumberOfGuests == "";
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
            return (Arrival != null && Departure != null && Arrival <= Departure && Arrival.Date > DateTime.Now);
        }
        //Validation - all fields are empty
        private bool AreAllFieldsEmpty()
        {
            return Arrival == null && Departure == null && NumberOfGuests == "" && NumberOfDays == "";
        }

        //Message box - all fields are empty
        private void ShowMessageBoxEmptyAll()
        {
            Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("Please enter data in fields!", "/Resources/Images/exclamation.png");
            messageBox.Owner = Application.Current.Windows.OfType<AccommodationReservationFormView>().FirstOrDefault();
            messageBox.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
