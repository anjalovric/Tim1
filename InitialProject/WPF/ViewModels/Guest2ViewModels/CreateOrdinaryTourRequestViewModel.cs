﻿using InitialProject.Domain.Model;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest2Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class CreateOrdinaryTourRequestViewModel:INotifyPropertyChanged
    {
        private TextBlock Capacity;
        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> CitiesByCountry { get; set; }
        private LocationRepository locationRepository;
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
        private ComboBox Country;
        private ComboBox City;
        private TextBox Description;
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

        private TextBox Name;
        public CreateOrdinaryTourRequestViewModel(TextBlock capacity, TextBox name,Model.Guest2 guest2, TextBox description,ComboBox country,ComboBox city)
        {
            Capacity = capacity;
            Country=country;
            City = city;  
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            Description = description;
            Name = name;
            Guest2=guest2;
            MakeCommands();
            AddLanguages();
            locationRepository = new LocationRepository();
            Countries = new ObservableCollection<string>(locationRepository.GetAllCountries());
            CitiesByCountry = new ObservableCollection<string>();
            city.IsEnabled = false;
            
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
            int changedCapacity;
            changedCapacity = Convert.ToInt32(Capacity.Text);
            changedCapacity++;
            Capacity.Text = changedCapacity.ToString();
        }
        private void Decrement_Executed(object sender)
        {
            int changedCapacity;
            changedCapacity = Convert.ToInt32(Capacity.Text);
            changedCapacity--;
            if (changedCapacity < 1)
            {
                return;
            }
            Capacity.Text = changedCapacity.ToString();
        }

        private void Confirm_Executed(object sender)
        {
            LocationService locationService = new LocationService();
            Model.Location newLocation = locationService.GetByCityAndCountry(Country.SelectedValue.ToString(), City.SelectedValue.ToString());
            OrdinaryTourRequestsService requestService = new OrdinaryTourRequestsService();
            DateTime createDate=DateTime.Now;
            if (EndDate < StartDate)
            {
                MessageBox.Show("Niste dobro popunili polja!");
                return;
            }
            OrdinaryTourRequests request = new OrdinaryTourRequests(Name.Text,Guest2.Id, Convert.ToInt32(Capacity.Text), newLocation, Description.Text, SelectedLanguage, Convert.ToDateTime(Start), Convert.ToDateTime(End), false, "On waiting",Start.ToString().Split(" ")[0],End.ToString().Split(" ")[0],-1,createDate,false,-1);
            requestService.Save(request);
            Application.Current.Windows.OfType<CreateOrdinaryTourRequestView>().FirstOrDefault().Close();
        }
        public void CountryInput_SelectionChanged()
        {
            if (Country.SelectedItem != null)
            {
                CitiesByCountry.Clear();
                foreach (string city in locationRepository.GetCitiesByCountry((string)Country.SelectedItem))
                {
                    CitiesByCountry.Add(city);
                }
                City.IsEnabled = true;
            }
        }
    }
}
