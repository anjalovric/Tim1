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

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class NotificationsViewModel
    {
        public ObservableCollection<Guest2Notification> Notifications { get; set; }
        private NewTourNotificationService notificationService;
        private OrdinaryTourRequestsService ordinaryTourRequests;
        private List<OrdinaryTourRequests> OrdinaryTourRequests;
        private List<OrdinaryTourRequests> AcceptedOrdinaryTourRequests;
        private TourInstanceService tourInstanceService;
        public List<TourInstance> TourInstances;
        private Guest2 guest2;
        private Guest2Notification novi;
        public RelayCommand ViewDetailsCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public NotificationsViewModel(Guest2 guest2)
        {
            this.guest2 = guest2;
            notificationService = new NewTourNotificationService();
            ordinaryTourRequests = new OrdinaryTourRequestsService();
            OrdinaryTourRequests = new List<OrdinaryTourRequests>(ordinaryTourRequests.GetByGuestId(guest2.Id));
            AcceptedOrdinaryTourRequests = new List<OrdinaryTourRequests>();
            tourInstanceService = new TourInstanceService();
            TourInstances = new List<TourInstance>(tourInstanceService.GetAll());
            FindAccepted();
            novi = new Guest2Notification();
            Notifications = new ObservableCollection<Guest2Notification>(notificationService.GetByGuestId(guest2.Id));
            ViewDetailsCommand = new RelayCommand(ViewDetails_Executed, CanExecute);
            DeleteCommand=new RelayCommand(Delete_Executed,CanExecute);
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private List<OrdinaryTourRequests> FindAccepted()
        {
            foreach (OrdinaryTourRequests ordinaryTourRequests in OrdinaryTourRequests)
            {
                if (ordinaryTourRequests.NewAccepted == true)
                {
                    AcceptedOrdinaryTourRequests.Add(ordinaryTourRequests);
                }
            }
            return AcceptedOrdinaryTourRequests;
        }
        private void ViewDetails_Executed(object sender)
        {
            Guest2Notification currentNotification = ((Button)sender).DataContext as Guest2Notification;
            OrdinaryTourRequestDetailsForm ordinaryTourRequestDetailsForm = new OrdinaryTourRequestDetailsForm(currentNotification, guest2);
            ordinaryTourRequestDetailsForm.Show();
        }
        private void Delete_Executed(object sender)
        {
            Guest2Notification currentNotification = ((Button)sender).DataContext as Guest2Notification;
            NewTourNotificationService guest2NotificationService = new NewTourNotificationService();
            guest2NotificationService.Delete(currentNotification);
            Notifications.Clear();
            Notifications = new ObservableCollection<Guest2Notification>(notificationService.GetByGuestId(guest2.Id));
        }
    }
}
