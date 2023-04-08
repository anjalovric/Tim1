using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for Guest1Home.xaml
    /// </summary>
    public partial class Guest1Home : Window
    {
        private Guest1SearchAccommodation guest1SearchAccommodation;
        private MyAccommodationReservations myReservations;
        private Guest1 guest1;
        private Guest1Service guest1Service;
        private SentAccommodationReservationRequests sentAccommodationReservationRequests;
        private Guest1Profile guest1Profile;
        
       
        
        
        public Guest1Home(User user)
        {
            InitializeComponent();

            
            guest1Service = new Guest1Service();
            GetGuest1ByUser(user);
            
            guest1SearchAccommodation = new Guest1SearchAccommodation(guest1, ref Main);
            myReservations = new MyAccommodationReservations(guest1, ref Main);
            sentAccommodationReservationRequests = new SentAccommodationReservationRequests(guest1);
            guest1Profile = new Guest1Profile(guest1);
            Main.Content = guest1SearchAccommodation;

        }

        private void GetGuest1ByUser(User user)
        {
            this.guest1 = new Guest1();
            this.guest1 = guest1Service.GetByUsername(user.Username);
            
        }

        private void BookingMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = guest1SearchAccommodation;   //da li da se pravi novi page (kako bi se resetovalo sve?)
        }
        private void MyReservationsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            myReservations = new MyAccommodationReservations(guest1, ref Main); //moram ga opet napraviti da bi se azuriralo nakon pravljenja nove rezervacije (u upcoming reservations)
            Main.Content = myReservations;   //da li da se pravi novi page (kako bi se resetovalo sve?)
            
        }//negdje proslijedjujem ref Frame?

        private void SignOutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.Owner.Show();
        }

        private void SentRequestsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            sentAccommodationReservationRequests = new SentAccommodationReservationRequests(guest1);
            Main.Content = sentAccommodationReservationRequests;
        }

        private void MyProfileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = guest1Profile;
        }

        private void NotificationsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink[] links = GetAllNotifications();
            foreach(Hyperlink link in links)
            {
                NotificationsList.Items.Add(new MenuItem { Header = link, IsCheckable = false, Height = 80 }) ;
            }
        }

        private Hyperlink CreateHyperlinkNotification(String notification, String state)
        {
            Hyperlink link = new Hyperlink();
            link.IsEnabled = true;
            link.Inlines.Add(notification);

            if(state.Equals("Approved"))
                link.Click += NavigateToApprovedRequests_Click;
            else if(state.Equals("Declined"))
                link.Click += NavigateToDeclinedRequests_Click;

            return link;
        }

        private void NavigateToApprovedRequests_Click(object sender, RoutedEventArgs e)
        {
            sentAccommodationReservationRequests.RequestsTabControl.SelectedIndex = 0;
            Main.Content = sentAccommodationReservationRequests;    
        }
        private void NavigateToDeclinedRequests_Click(object sender, RoutedEventArgs e)
        {
            sentAccommodationReservationRequests.RequestsTabControl.SelectedIndex = 2;
            Main.Content = sentAccommodationReservationRequests;
        }

        private Hyperlink[] GetAllNotifications()
        {
            List<CompletedAccommodationReschedulingRequest> completedRequests = GetRequestsByGuest();
            completedRequests.Reverse();
            string[] notifications = new String[completedRequests.Count];
            Hyperlink[] links = new Hyperlink[completedRequests.Count];
            for (int i = 0; i < completedRequests.Count; i++)
            {
                notifications[i] = completedRequests[i].Request.Reservation.Accommodation.Owner.Name + " " + completedRequests[i].Request.Reservation.Accommodation.Owner.LastName;
                notifications[i] += " " + completedRequests[i].Request.state.ToString().ToLower() + " your request.";
                links[i]=CreateHyperlinkNotification(notifications[i], completedRequests[i].Request.state.ToString());
            }    

            return links;
        }

        private List<CompletedAccommodationReschedulingRequest> GetRequestsByGuest()
        {
            CompletedAccommodationReschedulingRequestService completedAccommodationReschedulingRequestsService = new CompletedAccommodationReschedulingRequestService();
            List<CompletedAccommodationReschedulingRequest> storedRequests = new List<CompletedAccommodationReschedulingRequest>(completedAccommodationReschedulingRequestsService.GetAll());
            List<CompletedAccommodationReschedulingRequest> filteredRequests = new List<CompletedAccommodationReschedulingRequest>();

            foreach(CompletedAccommodationReschedulingRequest completedRequest in storedRequests)
            {
                if(completedRequest.Request.Reservation.Guest.Id==guest1.Id)
                    filteredRequests.Add(completedRequest);
            }

            return filteredRequests;

        }

    }
}
