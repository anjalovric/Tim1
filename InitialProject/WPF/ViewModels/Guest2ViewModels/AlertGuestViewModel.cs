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
using InitialProject.Domain.Model;
using System.Collections.ObjectModel;
using System.Security.Principal;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class AlertGuestViewModel
    {
        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        private List<AlertGuest2> alerts;
        private AlertGuest2Service _alertGuest2Service;
        private CheckPointService _checkPointService;
        private TourRepository _tourRepository;
        private TourInstanceService _tourInstanceService;
        private int AlertId;
        private LocationService _locationService;
        public string PointLabel { get; set; }
        private GuideRepository _guideRepository;
        public Action CloseAction { get; set; }
        public string GuideLabel { get; set; }
        private WinningAVoucher WinningAVoucher;
        public AlertGuestViewModel(int alertId)
        {
            AlertId = alertId;
            _alertGuest2Service = new AlertGuest2Service();
            _checkPointService = new CheckPointService();
            _tourRepository = new TourRepository();
            _tourInstanceService = new TourInstanceService();
            _locationService = new LocationService();
            _guideRepository = new GuideRepository();
            MakeCommands();
            alerts = _alertGuest2Service.GetAll();
            CreateLabelContent();
        }
        private void MakeCommands()
        {
            ConfirmCommand = new RelayCommand(Confirm_Executed, CanExecute);
            CancelCommand = new RelayCommand(Cancel_Executed, CanExecute);
            CloseCommand = new RelayCommand(Close_Executed, CanExecute);
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private void Confirm_Executed(object sender)
        {
            foreach (AlertGuest2 alertGuest2 in alerts)
            {
                if (alertGuest2.Id == AlertId && alertGuest2.Seen==false)
                {
                    alertGuest2.Availability = true;
                    alertGuest2.Informed = true;
                    alertGuest2.Seen = true;
                    _alertGuest2Service.Update(alertGuest2);
                    DeleteFromNotifications(alertGuest2);
                    WinningAVoucher winningAVoucher = new WinningAVoucher(alertGuest2.Guest2Id);
                    winningAVoucher.CountOfTours();
                }
            }
            CloseAction();
        }
        private void DeleteFromNotifications(AlertGuest2 alertGuest2)
        {
            NewTourNotificationService newTourNotificationService = new NewTourNotificationService();
            List<NewTourNotification> notifications = new List<NewTourNotification>(newTourNotificationService.GetAll());
            foreach(NewTourNotification notification in notifications)
            {
                if (notification.AlertGuest2Id == alertGuest2.Id)
                    newTourNotificationService.Delete(notification);
            }
        }
        private void Close_Executed(object sender)
        {
            CloseAction();
        }
        private void Cancel_Executed(object sender)
        {
            foreach (AlertGuest2 alertGuest2 in alerts)
            {
                if (alertGuest2.Availability == false && alertGuest2.Id == AlertId && alertGuest2.Seen == false)
                {
                    alertGuest2.Informed = true;
                    alertGuest2.Seen = true;
                    _alertGuest2Service.Update(alertGuest2);
                    DeleteFromNotifications(alertGuest2);
                   
                }
            }
            CloseAction();
        }
        private void CreateLabelContent()
        {
            int pointId = _alertGuest2Service.GetAll().Find(n => n.Id == AlertId).CheckPointId;
            int instanceId = _alertGuest2Service.GetAll().Find(n => n.Id == AlertId).InstanceId;
            int guideId = _alertGuest2Service.GetAll().Find(n => n.Id == AlertId).GuideId;
            if (_tourInstanceService.GetAll().Count > 0)
            {
                Tour thisTour;
                Guide guide;
                thisTour = _tourInstanceService.GetAll().Find(n => n.Id == instanceId).Tour;
                //string username=_.
                guide = _guideRepository.GetById(guideId);
                SetLocations(thisTour);
                if (thisTour != null)
                    PointLabel = "Name: " + thisTour.Name + "\n\nLocation: " + thisTour.Location+"\n\nDuration: "+thisTour.Duration+"\n\nCheckpoint: "+ _checkPointService.GetAll().Find(n => n.Id == pointId).Name;
                if (guide != null)
                    GuideLabel = "Name: " + guide.Name + "\n\nLast name: " + guide.LastName + "\n\nUsername: " + guide.Username;
            }
        }
        public void SetLocations(Tour Tour)
        {
            List<Location> locations = _locationService.GetAll();
            List<Tour> tours = _tourRepository.GetAll();

            foreach (Location location in locations)
            {
                foreach (Tour tour in tours)
                {
                    if (location.Id == tour.Location.Id && tour.Id == Tour.Id)
                    {
                        Tour.Location = location;
                        Tour.Name = tour.Name;
                        Tour.Duration = tour.Duration;
                    }
                }
            }
        }
    }
}
