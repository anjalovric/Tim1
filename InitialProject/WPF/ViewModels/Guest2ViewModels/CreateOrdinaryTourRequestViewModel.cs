﻿using InitialProject.Domain.Model;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest2Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class CreateOrdinaryTourRequestViewModel:INotifyPropertyChanged
    {
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
        public CreateOrdinaryTourRequestViewModel(Model.Guest2 guest2, ObservableCollection<OrdinaryTourRequests> ordinaryTourRequests)
        {
            MaxGuests = "";
            NowDate = DateTime.Now;
            StartDate = NowDate; ;
            EndDate = NowDate;
            Guest2 = guest2;
            OrdinaryTourRequests = ordinaryTourRequests;
            MakeCommands();
            AddLanguages();
            locationRepository = new LocationRepository();
            Countries = new ObservableCollection<string>(locationRepository.GetAllCountries());
            CitiesByCountry = new ObservableCollection<string>();
            IsComboBoxCityEnabled = false;
            OrdinaryTourRequests = ordinaryTourRequests;    
        }
        private void MakeCommands()
        {
            ConfirmCommand = new RelayCommand(Confirm_Executed, CanExecute);
            CancelCommand = new RelayCommand(Cancel_Executed, CanExecute);
            IncrementCommand = new RelayCommand(Increment_Executed, CanExecute);
            DecrementCommand = new RelayCommand(Decrement_Executed, CanExecute);
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
        private void Cancel_Executed(object sender)
        {
            Application.Current.Windows.OfType<CreateOrdinaryTourRequestView>().FirstOrDefault().Close();
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
            DateTime createDate=DateTime.Now;
            if (End < StartDate)
            {
                MessageBox.Show("Niste dobro popunili polja!");
                return;
            }
            OrdinaryTourRequests request = new OrdinaryTourRequests(Name,Guest2.Id, Convert.ToInt32(MaxGuests), newLocation, Description, SelectedLanguage, Convert.ToDateTime(Start), Convert.ToDateTime(End), "On waiting",Start.ToString().Split(" ")[0],End.ToString().Split(" ")[0],-1,createDate,false,-1);
            OrdinaryTourRequests savedRequest=requestService.Save(request);
            OrdinaryRequestNotification requestNotification = new OrdinaryRequestNotification(savedRequest.Id);
            RequestNotificationService requestNotificationService = new RequestNotificationService();
            requestNotificationService.Save(requestNotification);
            Application.Current.Windows.OfType<CreateOrdinaryTourRequestView>().FirstOrDefault().Close();
            SetTourRequests();
        }
        private void SetTourRequests()
        {
            OrdinaryTourRequests.Clear();
            OrdinaryTourRequestsService requestService = new OrdinaryTourRequestsService();
            foreach(OrdinaryTourRequests ordinaryTourRequests in requestService.GetByGuestId(Guest2.Id))
            {
                 OrdinaryTourRequests.Add(ordinaryTourRequests);
            }
        }
        public void CountryInput_SelectionChanged()
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
    }
}
