﻿using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for TourCheckPoints.xaml
    /// </summary>
    public partial class TourCheckPoints : Window, INotifyPropertyChanged
    {
        public ObservableCollection<CheckPoint> AllPoints { get; set; }
        public ObservableCollection<CheckPoint> CurrentPoint { get; set; }


        private CheckPointRepository _pointsRepository;
        private TourRepository _tourRepository;
        private TourInstanceRepository _tourInstanceRepository;
        private TourReservationRepository tourReservationRepository;
        private AlertGuest2Repository alertGuest2Repository;


        private TourInstance _selected;

        private int counter = 1;


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TourCheckPoints(TourInstance selected)
        {
            InitializeComponent();
            DataContext = this;
            _pointsRepository = new CheckPointRepository();
            _tourInstanceRepository= new TourInstanceRepository();
            tourReservationRepository = new TourReservationRepository();
            alertGuest2Repository = new AlertGuest2Repository();
            AllPoints = new ObservableCollection<CheckPoint>();
            CurrentPoint = new ObservableCollection<CheckPoint>();
            List<CheckPoint> points = _pointsRepository.GetAll();

            if (selected != null)
            {
                foreach (CheckPoint point in points)
                {
                    if (point.TourId == selected.Tour.Id)
                    {
                        AllPoints.Add(point);
                    }
                }
                foreach (CheckPoint point in AllPoints)
                {
                    if (point.Order == 1)
                        point.Checked = true;
                }

                _selected = selected;
            }
           
            CurrentPoint.Add(AllPoints.ToList().Find(n=>n.Order==counter));
            AddAlerts(CurrentPoint[0].Id);

        }

        private void FinishTour(object sender, RoutedEventArgs e)
        {
            _selected.Finished = true;
            
            foreach (TourInstance tour in _tourInstanceRepository.GetAll())
                if (tour.Id == _selected.Id)
                    tour.Finished = true;
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            List<CheckPoint> points = AllPoints.ToList();
            CheckPoint checkPoint = points.Find(n => n.Order == counter);
            checkPoint.Checked= true;
            CurrentPoint.Remove(points.Find(n => n.Order == counter));
            counter++;
            CurrentPoint.Add(points.Find(n => n.Order == counter));
            if (counter == AllPoints.ToList().Count)
            {
                this.Next.IsEnabled = false;
                _selected.Finished = false;
                Finish.IsEnabled = false;
                MessageBox.Show("This tour instance is finished");
            }
            AddAlerts(CurrentPoint[0].Id);

        }

        private void AddAlerts(int currentPointId)
        {
            List<TourReservation> tourReservations= tourReservationRepository.GetAll();
            List<TourReservation> availableReservations = new List<TourReservation>();
            foreach(TourReservation tour in tourReservations)
            {
                if (tour.TourInstanceId == _selected.Id)
                {
                    availableReservations.Add(tour);
                }
            }
            foreach(TourReservation tour in availableReservations)
            {
                AlertGuest2 alertGuest2 = new AlertGuest2();
                alertGuest2.Availability = false;
                alertGuest2.ReservationId = tour.Id;
                alertGuest2.Guest2Id = tour.GuestId;
                alertGuest2.CheckPointId = currentPointId;
                AlertGuest2 savedAlert=alertGuest2Repository.Save(alertGuest2);

            }
        }
    }
}
