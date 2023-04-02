using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.View;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for FinishedTourInstances.xaml
    /// </summary>
    
    public partial class FinishedTourInstances : UserControl
    {
        private Guest2 guest2;
        private ObservableCollection<TourInstance> completedTours;
        public ObservableCollection<TourInstance> CompletedTours 
        {
            get { return completedTours; }
            set
            {
                if (value != completedTours)
                    completedTours = value;
                OnPropertyChanged("CompletedTours");
            }

        }
        private TourRepository tourRepository;
        private TourInstanceRepository tourInstanceRepository;
        private LocationRepository locationRepository;
        private TourReservationRepository tourReservationRepository;
        private ObservableCollection<TourReservation> tourReservations;
        private Location location;
       
        public Location Location
        {
            get { return location; }
            set
            {
                if (value != location)
                {
                    location = value;
                    OnPropertyChanged();
                }
            }
        }
        public FinishedTourInstances(Guest2 guest2)
        {
            InitializeComponent();
            DataContext = this;
            this.guest2 = guest2;
            tourRepository = new TourRepository();
            tourInstanceRepository = new TourInstanceRepository();
            tourReservationRepository = new TourReservationRepository();
            tourReservations = new ObservableCollection<TourReservation>(tourReservationRepository.GetAll());
            CompletedTours = new ObservableCollection<TourInstance>();
            SetTourInstances(CompletedTours);
            locationRepository = new LocationRepository();
            Location = new Location();
            SetLocations();
            SetTours(CompletedTours);
        }
        private void SetTourInstances(ObservableCollection<TourInstance> CompletedTours)
        {
            List<TourInstance> tourInstances;
            tourInstances = tourInstanceRepository.GetAll();
            foreach(TourReservation tourReservation in tourReservations)
            {
                foreach (TourInstance tourInstance in tourInstances)
                {
                    if(tourReservation.TourInstanceId==tourInstance.Id && tourReservation.GuestId == guest2.Id && tourInstance.Finished==true)
                    {
                        CompletedTours.Add(tourInstance);
                    }
                }
            }
        }
        public void SetLocations()
        {
            List<Location> locations = locationRepository.GetAll();
            List<Tour> tours = tourRepository.GetAll();

            foreach (Location location in locations)
            {
                foreach (Tour tour in tours)
                {
                    if (location.Id == tour.Location.Id)
                        tour.Location = location;
                }
            }
        }
        public void SetTours(ObservableCollection<TourInstance> CompletedTours)
        {
            List<Tour> tours = tourRepository.GetAll();
            foreach (TourInstance tourInstance in CompletedTours)
            {
                foreach (Tour tour in tours)
                {
                    if (tour.Id == tourInstance.Tour.Id)
                    {
                        tourInstance.Tour = tour;
                    }
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void Rate_Click(object sender, RoutedEventArgs e)
        {
            TourInstance currentTourInstance = (TourInstance)TourListDataGrid.CurrentItem;
            foreach (TourInstance tourInstance in CompletedTours)
            {
                if (tourInstance.Id == currentTourInstance.Id)
                {
                    currentTourInstance = tourInstance;
                }
            }
            TourReservation reservation=new TourReservation();
            foreach(TourReservation tourReservation in tourReservations)
            {
                if (tourReservation.TourInstanceId == currentTourInstance.Id)
                {
                    reservation = tourReservation;
                }
            }
            //TourReservationForm tourReservationForm = new TourReservationForm(currentTourInstance, guest2, TourInstances, tourInstanceRepository, Label);
            //tourReservationForm.Show();
            RateTourAndGuide rateTourAndGuide = new RateTourAndGuide(currentTourInstance,guest2,reservation);
            rateTourAndGuide.Show();
            
        }
    }
}
