using InitialProject.Domain.Model;
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
        public ObservableCollection<NewTourNotification> Notifications { get; set; }
        private NewTourNotificationService notificationService;
        private OrdinaryTourRequestsService ordinaryTourRequests;
        private List<OrdinaryTourRequests> OrdinaryTourRequests;
        private AlertGuest2Service alertGuest2Service;
        private List<AlertGuest2> Alerts;
        private TourInstanceService tourInstanceService;
        public List<TourInstance> TourInstances;
        private Guest2 guest2;
        public RelayCommand ViewCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public NotificationsViewModel(Guest2 guest2)
        {
            this.guest2 = guest2;
            notificationService = new NewTourNotificationService();
            ordinaryTourRequests = new OrdinaryTourRequestsService();
            OrdinaryTourRequests = new List<OrdinaryTourRequests>(ordinaryTourRequests.GetByGuestId(guest2.Id));
            Alerts = new List<AlertGuest2>();
            alertGuest2Service = new AlertGuest2Service();
            tourInstanceService = new TourInstanceService();
            TourInstances = new List<TourInstance>(tourInstanceService.GetAll());
            SaveCreateTourNotifications();
            Notifications = new ObservableCollection<NewTourNotification>();
            SetNotifications();
            ViewCommand = new RelayCommand(View_Executed, CanExecute);
            DeleteCommand=new RelayCommand(Delete_Executed,CanExecute);
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private bool Exists(OrdinaryTourRequests request)
        {
            return notificationService.GetAll().Exists(c => c.Guest2.Id == request.GuestId && c.RequestId == request.Id);
        }
        private void SetNotifications()
        {
            foreach(NewTourNotification newTourNotification in notificationService.GetByGuestId(guest2.Id))
            {
                if (!newTourNotification.Deleted)
                    Notifications.Add(newTourNotification);
            }
        }
        private bool IsDeleted(int requestId)
        {
            List<NewTourNotification> notifications = new List<NewTourNotification>(notificationService.GetByGuestId(guest2.Id));
            NewTourNotification current = notifications.Find(c => c.RequestId == requestId);
            if (notifications.Count == 0 || current==null) return false;
            else return current.Deleted;
        }
        private void SaveCreateTourNotifications()
        {
            foreach(TourInstance instance in TourInstances)
            {
                foreach (OrdinaryTourRequests ordinaryTourRequests in OrdinaryTourRequests)
                {
                    if (ordinaryTourRequests.NewAccepted == true && ordinaryTourRequests.TourInstanceId==instance.Id && !Exists(ordinaryTourRequests) && !IsDeleted(ordinaryTourRequests.Id))

                    {
                        NewTourNotification guest2Notification = new NewTourNotification(guest2, "Your tour request has been accepted. Click on details for more. You can delete it.", Guest2NotificationType.REQUEST_ACCEPTED, instance, false, -1, ordinaryTourRequests.Id);
                        notificationService.Save(guest2Notification);
                    }
                }
            }
            
        }

        private void ShowAlertGuestForm(NewTourNotification notification)
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
            NewTourNotification currentNotification = ((Button)sender).DataContext as NewTourNotification;
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
            NewTourNotification currentNotification = ((Button)sender).DataContext as NewTourNotification;

            NewTourNotificationService notificationService = new NewTourNotificationService();
            currentNotification.Deleted = true;
            notificationService.Update(currentNotification);
            Notifications.Clear();
            foreach(NewTourNotification guest2Notification in notificationService.GetByGuestId(guest2.Id))
            {
                if(!guest2Notification.Deleted)
                    Notifications.Add(guest2Notification);
            }
        }
    }
}
