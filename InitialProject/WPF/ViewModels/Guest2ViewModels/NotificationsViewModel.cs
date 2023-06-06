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
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Cryptography.Xml;
using InitialProject.Help;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class NotificationsViewModel
    {
        public ObservableCollection<NewTourNotification> Notifications { get; set; }
        private NewTourNotificationService notificationService;
        private OrdinaryTourRequestsService ordinaryTourRequestsService;
        private List<OrdinaryTourRequests> OrdinaryTourRequests;
        private AlertGuest2Service alertGuest2Service;
        private List<AlertGuest2> Alerts;
        private TourInstanceService tourInstanceService;
        public List<TourInstance> TourInstances;
        public TourService tourService;
        private Guest2 guest2;
        public RelayCommand ViewCommand { get; set; }
        public ICommand HelpCommandInViewModel { get;}
        public RelayCommand DeleteCommand { get; set; }
        private NotificationsFormView org;
        public NotificationsViewModel(Guest2 guest2, NotificationsFormView org)
        {
            this.guest2 = guest2;
            notificationService = new NewTourNotificationService();
            tourService = new TourService();
            this.org = org;
            ordinaryTourRequestsService = new OrdinaryTourRequestsService();
            OrdinaryTourRequests = new List<OrdinaryTourRequests>(ordinaryTourRequestsService.GetAll());
            Alerts = new List<AlertGuest2>();
            alertGuest2Service = new AlertGuest2Service();
            tourInstanceService = new TourInstanceService();
            TourInstances = new List<TourInstance>(tourInstanceService.GetAll());
            MakeNotifications();
            Notifications = new ObservableCollection<NewTourNotification>();
            SetNotifications();
            ViewCommand = new RelayCommand(View_Executed, CanExecute);
            DeleteCommand=new RelayCommand(Delete_Executed,CanExecute);
            HelpCommandInViewModel = new RelayCommand(CommandBinding_Executed);
        }
        private void MakeNotifications()
        {
            AddNotificationsByLanguage();
            AddNotificationsByLocation();
            AddNotifications();
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private bool Exists(TourInstance tourInstance)
        {
            return notificationService.GetAll().Exists(c => c.Guest2.Id == guest2.Id && c.TourInstance.Id == tourInstance.Id);
        }
        private void SetNotifications()
        {
            foreach(NewTourNotification newTourNotification in notificationService.GetByGuestId(guest2.Id))
            {
                if (!newTourNotification.Deleted)
                    Notifications.Add(newTourNotification);
            }
        }
        private bool IsDeleted(int tourInstanceId)
        {
            List<NewTourNotification> notifications = new List<NewTourNotification>(notificationService.GetByGuestId(guest2.Id));
            NewTourNotification current = notifications.Find(c => c.TourInstance.Id == tourInstanceId);
            if (notifications.Count == 0 || current==null) return false;
            else return current.Deleted;
        }
        public void AddNotifications()
        {
            foreach (TourInstance tourInstance in TourInstances)
            {
                foreach (OrdinaryTourRequests request in ordinaryTourRequestsService.GetAcceptedRequests(OrdinaryTourRequests))
                {
                    if(request.TourInstanceId==tourInstance.Id && request.GuestId==guest2.Id && !Exists(tourInstance) && !IsDeleted(tourInstance.Id))
                    {
                        NewTourNotification guest2Notification = new NewTourNotification(guest2, "Your tour request has been accepted. Click on details for more. You can delete it.", Guest2NotificationType.REQUEST_ACCEPTED, tourInstance, false, -1);
                        notificationService.Save(guest2Notification);
                    }
                }
            }
        }
        public void AddNotificationsByLanguage()
        {
            tourInstanceService.SetLanguage(TourInstances);
            List<TourInstance> tourInstances = GetTourInstance();
            List<OrdinaryTourRequests> Requests = ordinaryTourRequestsService.GetInvalidOrWaitingRequests(OrdinaryTourRequests, guest2);

            foreach (TourInstance tourInstance in tourInstances)
            {
                foreach (OrdinaryTourRequests request in Requests)
                {
                    if (request.Language == tourInstance.Tour.Language && !Exists(tourInstance) && !IsDeleted(tourInstance.Id))
                    {
                        NewTourNotification guest2Notification = new NewTourNotification(guest2, "Your tour request has been accepted. Click on details for more. You can delete it.", Guest2NotificationType.REQUEST_ACCEPTED, tourInstance, false, -1);
                        notificationService.Save(guest2Notification);
                    }
                }
            }
        }
        public void AddNotificationsByLocation()
        {
            tourService.SetTourToTourInstance(TourInstances);
            foreach (TourInstance tourInstance in GetTourInstance())
            {
                foreach (OrdinaryTourRequests request in ordinaryTourRequestsService.GetInvalidOrWaitingRequests(OrdinaryTourRequests, guest2))
                {
                    if (request.Location.City == tourInstance.Tour.Location.City && request.Location.Country == tourInstance.Tour.Location.Country && !Exists(tourInstance) && !IsDeleted(tourInstance.Id))
                    {
                        NewTourNotification guest2Notification = new NewTourNotification(guest2, "Your tour request has been accepted. Click on details for more. You can delete it.", Guest2NotificationType.REQUEST_ACCEPTED, tourInstance, false, -1);
                        notificationService.Save(guest2Notification);
                    }
                }
            }
        }
        public List<TourInstance> GetTourInstance()
        {
            List<TourInstance> Instances = new List<TourInstance>();
            foreach (TourInstance tourInstance in TourInstances)
            {
                foreach (OrdinaryTourRequests tourRequests in ordinaryTourRequestsService.GetAcceptedRequests(OrdinaryTourRequests))
                {
                    if (tourInstance.Id == tourRequests.TourInstanceId)
                    {
                        Instances.Add(tourInstance);
                    }
                }
            }
            return Instances;
        }
        private void ShowAlertGuestForm(NewTourNotification notification)
        {
            Alerts = alertGuest2Service.GetAll();
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
                ShowAlertGuestForm(currentNotification);     
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
        private void CommandBinding_Executed(object sender)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                string str = ShowToursHelp.GetHelpKey((DependencyObject)focusedControl);
                ShowToursHelp.ShowHelpForNotification(str, org);
            }
        }
    }
}
