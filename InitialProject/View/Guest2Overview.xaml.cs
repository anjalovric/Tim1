﻿using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Policy;
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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for Guest2Overview.xaml
    /// </summary>
    public partial class Guest2Overview : Window
    {
        public ObservableCollection<Tour> Tours
        {
            get { return tours; }
            set
            {
                if(value!=tours)
                    tours = value;
                OnPropertyChanged("Tours");
            }
        }
        private const string FilePath = "../../../Resources/Data/alertsGuest2.csv";
        private const string filePath = "../../../Resources/Data/users.csv";
        private ObservableCollection<TourInstance> TourInstances;
        private ObservableCollection<Tour> tours { get; set; }  
        public Tour _selected;
        private TourRepository _tourRepository;
        private TourInstanceRepository _tourInstanceRepository;
        private Serializer<AlertGuest2> _alertGuestSerializer;
        private Serializer<User> _userSerializer;
        private List<AlertGuest2> alerts;
        private List<User> users;
        private int GuestId;
        public Guest2Overview()
        {
            InitializeComponent();
            DataContext = this;
            _alertGuestSerializer = new Serializer<AlertGuest2>();
            _userSerializer = new Serializer<User>();
            _tourRepository = new TourRepository();
            _tourInstanceRepository = new TourInstanceRepository();
            TourInstances = new ObservableCollection<TourInstance>(_tourInstanceRepository.GetAll());
            Tours = new ObservableCollection<Tour>();
            GetAllTours();
            SetLocations();
            users = _userSerializer.FromCSV(filePath);
            alerts=_alertGuestSerializer.FromCSV(FilePath);
            GuestId = GetGuest2Id();
            if (alerts.Count() != 0)
            {
                foreach(AlertGuest2 alert in alerts)
                {
                    AlertGuestForm alertGuestForm = new AlertGuestForm();
                    if(GuestId==alert.Guest2Id)
                        alertGuestForm.Show();
                }
            }
        }
        private ObservableCollection<Tour> GetAllTours()
        {
            foreach (TourInstance tourInstance in TourInstances)
            {
                Tours.Add(tourInstance.Tour);
            }
            return Tours;
        }
        public void SetLocations()
        {
            Serializer<Location> _serializerLocation = new Serializer<Location>();
            List<Location> locations = _serializerLocation.FromCSV("../../../Resources/Data/locations.csv");
            foreach (Tour tour in Tours)
            {
                if (locations.Find(n => n.Id == tour.Location.Id) != null)
                {
                    tour.Location = locations.Find(n => n.Id == tour.Location.Id);
                }
            }
        }
        private void SignOut_Click(object sender, RoutedEventArgs e)
        {

            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            this.Close();

        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            List<Tour> listTours = new List<Tour>();
            foreach (TourInstance tourInstance in TourInstances)
            {
                listTours.Add(tourInstance.Tour);
            }
            Tours.Clear();
            foreach (Tour tour in listTours)
            {
                Tours.Add(tour);
            }
            foreach (Tour tour in listTours)
            {
                if (tour.Location.City != null)
                {
                    if (!tour.Location.City.ToLower().Contains(cityInput.Text.ToLower()))
                    {
                        Tours.Remove(tour);
                    }
                }
                if (tour.Location.Country != null)
                {
                    if (!tour.Location.Country.ToLower().Contains(countryInput.Text.ToLower()))
                    {
                        Tours.Remove(tour);
                    }
                }
                if (tour.Duration != null)
                {
                    if (durationInput.Text != "")
                    {
                        if (tour.Duration < Convert.ToDouble(durationInput.Text))
                        {
                            Tours.Remove(tour);
                        }
                    }
                }
                if (tour.Language != null)
                {
                    if (!tour.Language.ToLower().Contains(languageInput.Text.ToLower()))
                    {
                        Tours.Remove(tour);
                    }
                }
                if (tour.MaxGuests != null)
                {
                    if (Convert.ToInt32(capacityNumber.Text) > tour.MaxGuests)
                    {
                        Tours.Remove(tour);
                    }
                    
                }
                


            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void incrementCapacityNumber_Click(object sender, RoutedEventArgs e)
        {
            int changedCapacityNumber;
            changedCapacityNumber = Convert.ToInt32(capacityNumber.Text) + 1;
            capacityNumber.Text = changedCapacityNumber.ToString();
        }

        private void decrementCapacityNumber_Click(object sender, RoutedEventArgs e)
        {
            int changedCapacityNumber;
            if (Convert.ToInt32(capacityNumber.Text) > 1)
            {
                changedCapacityNumber = Convert.ToInt32(capacityNumber.Text) - 1;
                capacityNumber.Text = changedCapacityNumber.ToString();
            }
        }
        private int GetGuest2Id() //ovo ce morati drugacije kada budemo imali vise gostiju
        {
            foreach(User user in users)
            {
                if (user.Role.ToString()=="GUEST2")
                {
                    return user.Id;
                }
            }
            return 0;
        }
        private void Reserve(object sender, RoutedEventArgs e)
        {
            Tour currentTour = (Tour)TourListDataGrid.CurrentItem;
            TourInstance currentTourInstance=new TourInstance();
            foreach(TourInstance tourInstance in TourInstances)
            {
                if (tourInstance.Tour.Id == currentTour.Id && (TourListDataGrid.SelectedIndex+1)==tourInstance.Id)
                {
                    currentTourInstance = tourInstance;
                }
            }
           
            TourReservationForm tourReservationForm = new TourReservationForm(currentTour,currentTourInstance,GuestId);
            tourReservationForm.Show();
        }
    }
}
