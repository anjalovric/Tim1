﻿using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using InitialProject.Repository;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class NotificationsViewModel
    {
        public ObservableCollection<Guest2Notification> Notifications { get; set; }
        private Guest2NotificationService notificationService;
        private OrdinaryTourRequestsService ordinaryTourRequests;
        private List<OrdinaryTourRequests> OrdinaryTourRequests;
        private List<OrdinaryTourRequests> AcceptedOrdinaryTourRequests;
        private AlertGuest2Service alertGuest2Service;
        private List<AlertGuest2> Alerts;
        private TourInstanceService tourInstanceService;
        public List<TourInstance> TourInstances;
        private Guest2 guest2;
        private Guest2Notification novi;
        public RelayCommand ViewCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public NotificationsViewModel(Guest2 guest2)
        {
            this.guest2 = guest2;
            notificationService = new Guest2NotificationService();
            ordinaryTourRequests = new OrdinaryTourRequestsService();
            OrdinaryTourRequests = new List<OrdinaryTourRequests>(ordinaryTourRequests.GetByGuestId(guest2.Id));
            Alerts = new List<AlertGuest2>();
            alertGuest2Service = new AlertGuest2Service();
            AcceptedOrdinaryTourRequests = new List<OrdinaryTourRequests>();
            tourInstanceService = new TourInstanceService();
            TourInstances = new List<TourInstance>(tourInstanceService.GetAll());
           // FindAccepted();
            novi = new Guest2Notification();
            Notifications = new ObservableCollection<Guest2Notification>(notificationService.GetByGuestId(guest2.Id));
            ViewCommand = new RelayCommand(View_Executed, CanExecute);
            DeleteCommand=new RelayCommand(Delete_Executed,CanExecute);
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        /*private List<OrdinaryTourRequests> FindAccepted()
        {
            foreach (OrdinaryTourRequests ordinaryTourRequests in OrdinaryTourRequests)
            {
                if (ordinaryTourRequests.NewAccepted == true)
                {
                    AcceptedOrdinaryTourRequests.Add(ordinaryTourRequests);
                }
            }
            return AcceptedOrdinaryTourRequests;
        }*/
        private void ShowAlertGuestForm(Guest2Notification notification)
        {
            Alerts = alertGuest2Service.GetAll();
            CheckPointService checkPointService = new CheckPointService();
            List<CheckPoint> CheckPoints = checkPointService.GetByInstance(notification.TourInstance.Id);
            if (Alerts.Count() != 0)
            {
                foreach (AlertGuest2 alert in Alerts)
                {
                    if (alert.Guest2Id == guest2.Id && alert.Seen == false && alert.Id == notification.AlertGuest2Id)
                    {
                        AlertGuestFormView alertGuestForm = new AlertGuestFormView(alert.Id);
                        alertGuestForm.Show();
                    }
                }
            }
        }

        private void View_Executed(object sender)
        {
            Guest2Notification currentNotification = ((Button)sender).DataContext as Guest2Notification;
            OrdinaryTourRequestDetailsForm ordinaryTourRequestDetailsForm = new OrdinaryTourRequestDetailsForm(currentNotification, guest2);
            if (currentNotification.Type==Guest2NotificationType.REQUEST_ACCEPTED)
                ordinaryTourRequestDetailsForm.Show();
            else
            {
                ShowAlertGuestForm(currentNotification);
                
            }
                

        }
        private void Delete_Executed(object sender)
        {
            Guest2Notification currentNotification = ((Button)sender).DataContext as Guest2Notification;
            Guest2NotificationService guest2NotificationService = new Guest2NotificationService();
            guest2NotificationService.Delete(currentNotification);
            Notifications.Clear();
            foreach(Guest2Notification guest2Notification in guest2NotificationService.GetByGuestId(guest2.Id))
            {
                Notifications.Add(guest2Notification);
            }
        }
    }
}
