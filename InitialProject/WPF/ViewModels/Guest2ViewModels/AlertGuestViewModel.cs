using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using InitialProject.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest2Views;
using System.Windows.Controls;
using InitialProject.WPF.Views.Guest1Views;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class AlertGuestViewModel
    {
        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        private List<AlertGuest2> alerts;
        private AlertGuest2Service _alertGuest2Service;
        private CheckPointService _checkPointService;
        private TourRepository _tourRepository;
        private TourInstanceService _tourInstanceService;
        private int AlertId;
        private LocationService _locationService;
        private System.Windows.Controls.Label PointLabel;
        public Action CloseAction { get; set; }
        public AlertGuestViewModel(int alertId, System.Windows.Controls.Label pointLabel)
        {
            AlertId = alertId;
            _alertGuest2Service = new AlertGuest2Service();
            _checkPointService = new CheckPointService();
            _tourRepository = new TourRepository();
            _tourInstanceService = new TourInstanceService();
            _locationService = new LocationService();
            PointLabel = pointLabel;
            MakeCommands();
            alerts = _alertGuest2Service.GetAll();
            CreateLabelContent();
        }
        private void MakeCommands()
        {
            ConfirmCommand = new RelayCommand(Confirm_Executed, CanExecute);
            CancelCommand = new RelayCommand(Cancel_Executed, CanExecute);
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private void Confirm_Executed(object sender)
        {
            foreach (AlertGuest2 alertGuest2 in alerts)
            {
                if (alertGuest2.Availability == false && alertGuest2.Id == AlertId)
                {
                    alertGuest2.Availability = true;
                    alertGuest2.Informed = true;
                    _alertGuest2Service.Update(alertGuest2);
                }
            }
            CloseAction();
        }
        private void Cancel_Executed(object sender)
        {
            foreach (AlertGuest2 alertGuest2 in alerts)
            {
                if (alertGuest2.Availability == false && alertGuest2.Id == AlertId)
                {
                    alertGuest2.Informed = true;
                    _alertGuest2Service.Update(alertGuest2);
                }
            }
            CloseAction();
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
                if (thisTour != null)
                    PointLabel.Content = "Are you present on point " + _checkPointService.GetAll().Find(n => n.Id == pointId).Name + " on tour " +
                                       thisTour.Name + " ?";
            }
        }
        public void SetLocations()
        {
            List<Location> locations = _locationService.GetAll();
            List<Tour> tours = _tourRepository.GetAll();

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
            List<Tour> tours = _tourRepository.GetAll();
            foreach (Tour tour in tours)
            {
                if (tour.Id == Tour.Id)
                {
                    Tour.Name = tour.Name;
                }
            }
        }
    }
}
