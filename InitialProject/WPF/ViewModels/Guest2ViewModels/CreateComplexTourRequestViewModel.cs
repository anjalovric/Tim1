using InitialProject.APPLICATION.UseCases;
using InitialProject.Domain.Model;
using InitialProject.Help;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.WPF.Validations.Guest2Validations;
using InitialProject.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Type = InitialProject.Domain.Model.Type;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class CreateComplexTourRequestViewModel : INotifyPropertyChanged
    {
        public RelayCommand CountryInputSelectionChangedCommand { get; set; }
        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> CitiesByCountry { get; set; }
        private LocationRepository locationRepository;
        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }
        private string complexName;
        public string ComplexName
        {
            get => complexName;
            set
            {
                if (value != complexName)
                {
                    complexName = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime Start;
        public DateTime StartDate
        {
            get => Start;
            set
            {
                if (value != Start)
                {
                    Start = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime End;
        public DateTime EndDate
        {
            get => End;
            set
            {
                if (value != End)
                {
                    End = value;
                    OnPropertyChanged();
                }
            }
        }

        private string city;
        public string City
        {
            get => city;
            set
            {
                if (value != city)
                {
                    city = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool isComboBoxCityEnabled;
        public bool IsComboBoxCityEnabled
        {
            get => isComboBoxCityEnabled;
            set
            {
                if (value != isComboBoxCityEnabled)
                {
                    isComboBoxCityEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        private string country;
        public string Country
        {
            get => country;
            set
            {
                if (value != country)
                {
                    country = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime NowDate { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand IncrementCommand { get; set; }
        public RelayCommand DecrementCommand { get; set; }
        public RelayCommand DodajCommand { get; set; }
        public ObservableCollection<string> Languages { get; set; }
        private string description;
        public string Description
        {
            get => description;
            set
            {
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged();
                }
            }
        }
        private Model.Guest2 Guest2;
        private string language;
        public string SelectedLanguage
        {
            get => language;
            set
            {
                if (value != language)
                {
                    language = value;
                    OnPropertyChanged();
                }
            }
        }
        private double duration;
        public double Duration
        {
            get => duration;
            set
            {
                if (value != duration)
                {
                    duration = value;
                    OnPropertyChanged();
                }
            }
        }

        private string maxGuests;

        public string MaxGuests
        {
            get => maxGuests;
            set
            {
                if (value != maxGuests)
                {
                    maxGuests = value;
                    OnPropertyChanged("MaxGuests");
                }
            }
        }
        public ObservableCollection<OrdinaryTourRequests> OrdinaryTourRequests { get; set; }
        public ObservableCollection<ComplexTourRequests> TourRequests;
        private APPLICATION.UseCases.ComplexTourRequestsService ComplexTourRequestsService;
        private CreateComplexTourRequestView org;
        public ICommand HelpCommandInViewModel { get; }
        public CreateComplexTourRequestViewModel(Model.Guest2 guest2, ObservableCollection<ComplexTourRequests> complexTourRequests,CreateComplexTourRequestView org)
        {
            MaxGuests = "";
            NowDate = DateTime.Now;
            StartDate = NowDate; ;
            EndDate = NowDate;
            Guest2 = guest2;
            this.org = org;
            ComplexTourRequestsService = new ComplexTourRequestsService();
            TourRequests = complexTourRequests;
            OrdinaryTourRequests = new ObservableCollection<OrdinaryTourRequests>();
            OrdinaryTourRequests.Clear();
            MakeCommands();
            AddLanguages();
            locationRepository = new LocationRepository();
            Countries = new ObservableCollection<string>(locationRepository.GetAllCountries());
            CitiesByCountry = new ObservableCollection<string>();
            IsComboBoxCityEnabled = false;
            HelpCommandInViewModel = new RelayCommand(CommandBinding_Executed);
        }
        private void MakeCommands()
        {
            ConfirmCommand = new RelayCommand(Confirm_Executed, CanExecute);
            CancelCommand = new RelayCommand(Cancel_Executed, CanExecute);
            IncrementCommand = new RelayCommand(Increment_Executed, CanExecute);
            DecrementCommand = new RelayCommand(Decrement_Executed, CanExecute);
            CountryInputSelectionChangedCommand = new RelayCommand(CountryInputSelectionChanged_Executed, CanExecute);
            DodajCommand= new RelayCommand(Dodaj, CanConfirmExecute);
        }
        private void AddLanguages()
        {
            Languages = new ObservableCollection<string>();
            Languages.Add("english");
            Languages.Add("spanish");
            Languages.Add("russian");
            Languages.Add("arabic");
            Languages.Add("serbian");
            Languages.Add("italian");
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private bool CanConfirmExecute(object sender)
        {
            return OrdinaryTourRequests.Count!=0;
        }
        private void Cancel_Executed(object sender)
        {
            Application.Current.Windows.OfType<CreateComplexTourRequestView>().FirstOrDefault().Close();
        }

        private void Increment_Executed(object sender)
        {
            int changedDaysNumber;
            if (MaxGuests == "")
                MaxGuests = "1";
            else
            {
                changedDaysNumber = Convert.ToInt32(MaxGuests) + 1;
                MaxGuests = changedDaysNumber.ToString();
            }
        }
        private void Decrement_Executed(object sender)
        {
            int changedDaysNumber;
            if (MaxGuests != "" && Convert.ToInt32(MaxGuests) > 1)
            {
                changedDaysNumber = Convert.ToInt32(MaxGuests) - 1;
                MaxGuests = changedDaysNumber.ToString();
            }
        }
        private void Confirm_Executed(object sender)
        {
            LocationService locationService = new LocationService();
            Model.Location newLocation = locationService.GetByCityAndCountry(Country.ToString(), City.ToString());
            OrdinaryTourRequestsService requestService = new OrdinaryTourRequestsService();
            DateTime createDate = DateTime.Now;
            if (End < StartDate)
            {
                MessageBox.Show("Invalid!");
                return;
            }
            OrdinaryTourRequests request = new OrdinaryTourRequests(Name, Guest2.Id, Convert.ToInt32(MaxGuests), newLocation, Description, SelectedLanguage, Convert.ToDateTime(Start), Convert.ToDateTime(End), Status.ONWAITING, Start.ToString().Split(" ")[0], End.ToString().Split(" ")[0], -1, createDate, false, -1,TourRequests.Count+1);
            OrdinaryTourRequests savedRequest = requestService.Save(request);
            OrdinaryTourRequests.Add(request);
            ResetAllFields();
            MessageBox.Show("The request has been added to the list of requests.");
        }
        private void ResetAllFields()
        {
            Name = "";
            Country = null;
            City = null;
            Description = "";
            SelectedLanguage = null;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            MaxGuests = "";
        }
        private void Dodaj(object sender)
        {
            ComplexTourRequests request = new ComplexTourRequests(ComplexName, Guest2, Type.ONWAITING);
            ComplexTourRequests savedRequest = ComplexTourRequestsService.Save(request);
            MessageBox.Show("Request is created.");
            Application.Current.Windows.OfType<CreateComplexTourRequestView>().FirstOrDefault().Close();
            SetTourRequests();
            
        }
        private void SetTourRequests()
        {
            TourRequests.Clear();
            ComplexTourRequestsService requestService = new ComplexTourRequestsService();
            foreach (ComplexTourRequests complexTourRequests in requestService.GetByGuestId(Guest2.Id))
            {
                TourRequests.Add(complexTourRequests);
            }
        }
 
        public void CountryInputSelectionChanged_Executed(object sender)
        {
            if (Country != null)
            {
                CitiesByCountry.Clear();
                foreach (string city in locationRepository.GetCitiesByCountry((string)Country))
                {
                    CitiesByCountry.Add(city);
                }
                IsComboBoxCityEnabled = true;
            }
        }
        private void CommandBinding_Executed(object sender)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                string str = ShowToursHelp.GetHelpKey((DependencyObject)focusedControl);
                ShowToursHelp.ShowHelpForCreateComplexRequest(str, org);
            }
        }
    }
}
