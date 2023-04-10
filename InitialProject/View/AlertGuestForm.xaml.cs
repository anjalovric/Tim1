using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using InitialProject.Service;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for AlertGuestForm.xaml
    /// </summary>
    public partial class AlertGuestForm : Window
    {
        private List<AlertGuest2> alerts;
        private AlertGuest2Service _alertGuest2Service;
        private CheckPointService _checkPointService;
        private TourService _tourService;
        private TourInstanceService _tourInstanceService;
        private int AlertId;
        private LocationService _locationService;
        public AlertGuestForm(int alertId)
        {
            InitializeComponent();
            AlertId = alertId;
            DataContext = this;
            _alertGuest2Service = new AlertGuest2Service();
            _checkPointService = new CheckPointService();
            _tourService = new TourService();
            _tourInstanceService = new TourInstanceService();
            _locationService = new LocationService();
            alerts =new List<AlertGuest2>(_alertGuest2Service.GetAll());
            CreateLabelContent();
            
        }
        private void CreateLabelContent()
        {
            int pointId = _alertGuest2Service.GetAll().Find(n => n.Id == AlertId).CheckPointId;
            int instanceId = _alertGuest2Service.GetAll().Find(n => n.Id == AlertId).InstanceId;
            if (_tourInstanceService.GetAll().Count > 0)
            {
                Tour thisTour;
                thisTour = _tourInstanceService.GetAll().Find(n => n.Id == instanceId).Tour;
                SetLocations();
                SetTour(thisTour);
                if(thisTour != null) 
                 PointLabel.Content = "Are you present on point " + _checkPointService.GetAll().Find(n => n.Id == pointId).Name + " on tour " +
                                    thisTour.Name + " ?";
            }
        }
        public void SetLocations()
        {
            List<Location> locations = _locationService.GetAll();
            List<Tour> tours = _tourService.GetAll();

            foreach (Location location in locations)
            {
                foreach (Tour tour in tours)
                {
                    if (location.Id == tour.Location.Id)
                        tour.Location = location;
                }
            }
        }
        public void SetTour(Tour Tour)
        {
            List<Tour> tours = _tourService.GetAll();
            foreach(Tour tour in tours)
            {
                if (tour.Id == Tour.Id)
                {
                    Tour.Name = tour.Name;
                }
            }
        }
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            foreach(AlertGuest2 alertGuest2 in alerts)
            {
                if (alertGuest2.Availability == false && alertGuest2.Id==AlertId)
                {
                    alertGuest2.Availability = true;
                    alertGuest2.Informed = true;
                    _alertGuest2Service.Update(alertGuest2);
                }
            }
            this.Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            foreach (AlertGuest2 alertGuest2 in alerts)
            {
                if (alertGuest2.Availability == false && alertGuest2.Id == AlertId)
                {
                    alertGuest2.Informed = true;
                    _alertGuest2Service.Update(alertGuest2);
                }
            }
            this.Close();
        }
    }
}
