﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Cache;
using System.Runtime.CompilerServices;
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
using InitialProject.Serializer;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for TourReservationForm.xaml
    /// </summary>
    public partial class TourReservationForm : Window, INotifyPropertyChanged
    {
        private int CurrentGuestsNumber;
        private int GuestsNumber;
        private int GuestId;
        private TourInstance CurrentTourInstance;
        private TourReservationRepository tourReservationRepository;
        private List<TourReservation> tourReservations;
        private List<TourInstance> tourInstances;
        private TourInstanceRepository tourInstanceRepository;

        private ShowTours ShowTours;

        private Guest2Overview Guest2Overview;
        private Boolean withVoucher = true;

        public ObservableCollection<TourInstance> TourInstances { get; set; }
        public Label Label { get; set; }
        private string age;
        public string Age
        {
            get => age;
            set
            {
                if (value != age)
                {
                    age = value;
                    OnPropertyChanged();
                }
            }
        }
        private Guest2 guest2;
        public TourReservationForm(TourInstance currentTourInstance,Guest2 guest2,ObservableCollection<TourInstance> TourInstance,TourInstanceRepository tourInstanceRepository,Label label)
        {
            InitializeComponent();
            DataContext = this;
            CurrentTourInstance = currentTourInstance;
            this.TourInstances = TourInstance;
            this.Label = label;
            this.tourInstanceRepository = tourInstanceRepository;
            tourInstances = tourInstanceRepository.GetAll();
            tourReservationRepository = new TourReservationRepository();
            tourReservations = tourReservationRepository.GetAll();
            ShowTours = new ShowTours(guest2);
            GetCurrentGuestsNumber();
            this.guest2 = guest2;
            GuestId = guest2.Id;
        }
        private int GetReservationsNumber()
        {
            int reservationsNumber = 0;
            foreach (TourReservation tourReservation in tourReservations)
            {
                if (CurrentTourInstance.Id == tourReservation.TourInstanceId)
                {
                    CurrentGuestsNumber = tourReservation.CurrentGuestsNumber;
                    continue;
                }
                reservationsNumber++;
            }
            return reservationsNumber;
        }
        public int GetCurrentGuestsNumber()
        {
            if (tourReservations.Count == 0 || GetReservationsNumber()==tourReservations.Count)
            {
                CurrentGuestsNumber = CurrentTourInstance.Tour.MaxGuests;
            }
            return CurrentGuestsNumber;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void incrementGuestsNumber_Click(object sender, RoutedEventArgs e)
        {
            int changedGuestsNumber;
            changedGuestsNumber = Convert.ToInt32(capacityNumber.Text) + 1;
            capacityNumber.Text = changedGuestsNumber.ToString();
        }
        private void decrementGuestsNumber_Click(object sender, RoutedEventArgs e)
        {
            int changedGuestsNumber;
            if (Convert.ToInt32(capacityNumber.Text) > 1)
            {
                changedGuestsNumber = Convert.ToInt32(capacityNumber.Text) - 1;
                capacityNumber.Text = changedGuestsNumber.ToString();
            }
        }
        public void FindAvailableTours()
        {
            Boolean existed = false;
            ObservableCollection<TourInstance> storedTourInstances= new ObservableCollection<TourInstance>(tourInstanceRepository.GetAll());
            ShowTours.SetLocations();
            ShowTours.SetTours(storedTourInstances);
            TourInstances.Clear();
            foreach (TourInstance tourInstance in storedTourInstances)
            {
                foreach (TourReservation tourReservation in tourReservations)
                {
                    if (tourReservation.TourInstanceId == tourInstance.Id && tourInstance.Id != CurrentTourInstance.Id && tourInstance.Tour.Location.City == CurrentTourInstance.Tour.Location.City && tourInstance.Tour.Location.Country == CurrentTourInstance.Tour.Location.Country && tourReservation.CurrentGuestsNumber > 0 && tourInstance.Finished==false)
                    {
                        TourInstances.Add(tourInstance);
                        existed = true;
                    }
                }
                if (tourInstance.Finished==false && !existed && CurrentTourInstance.Id != tourInstance.Id && tourInstance.Tour.Location.City == CurrentTourInstance.Tour.Location.City && tourInstance.Tour.Location.Country == CurrentTourInstance.Tour.Location.Country)
                {
                    TourInstances.Add(tourInstance);
                }
            }
        }
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            GuestsNumber = CurrentGuestsNumber - Convert.ToInt32(capacityNumber.Text);
            if (CurrentGuestsNumber == 0)
            {
                MessageBox.Show("There is no enough places for choosen number of people. Tour is completed.");
                FindAvailableTours();
                Label.Content = "Showing available tours: ";
                this.Close();
                return;
            }
            else if (GuestsNumber < 0 && CurrentGuestsNumber != 0)
            {
                MessageBox.Show("There is no enough places for choosen number of people. Available number of places for guest is " + CurrentGuestsNumber + ".");
                return;
            }
            else if (tourReservations.Count == 0)
            {
                TourReservation newTourReservation = new TourReservation(CurrentTourInstance.Id, GuestsNumber, GuestId,Convert.ToDouble(Age),Convert.ToInt32(capacityNumber.Text),withVoucher);
                tourReservationRepository.Save(newTourReservation);
            }
            ChangeTourReservation();
            this.Close();
        }
        private void ChangeTourReservation()
        {
            Boolean changed = false;
            foreach (TourReservation tourReservation in tourReservations)
            {
                if (CurrentTourInstance.Id == tourReservation.TourInstanceId)
                {
                    tourReservationRepository.Update(tourReservation, GuestsNumber);
                    changed = true;
                }
                else if (changed)
                {
                    break;
                }
            }
            if (!changed)
            {
                TourReservation newTourReservation = new TourReservation(CurrentTourInstance.Id, GuestsNumber, GuestId, Convert.ToDouble(Age),Convert.ToInt32(capacityNumber.Text),withVoucher);
                tourReservationRepository.Save(newTourReservation);
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
