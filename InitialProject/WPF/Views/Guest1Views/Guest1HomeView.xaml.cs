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
using InitialProject.WPF.ViewModels.Guest1ViewModels;

namespace InitialProject.WPF.Views.Guest1Views
{
    /// <summary>
    /// Interaction logic for Guest1Home.xaml
    /// </summary>
    public partial class Guest1HomeView : Window
    {
        private Guest1SearchAccommodationView guest1SearchAccommodation;
        private MyAccommodationReservationsView myReservations;
        private SentAccommodationReservationRequestsView sentAccommodationReservationRequests;
        private Guest1Profile guest1Profile;
        private Guest1 guest1;
        private Guest1Service guest1Service;
        
        private Guest1HomeViewModel guest1HomeViewModel;
       
        public Guest1HomeView(User user)
        {
            InitializeComponent();
            guest1Service = new Guest1Service();
            this.guest1 = guest1Service.GetByUsername(user.Username);   //treba li gost objekat
            guest1HomeViewModel = new Guest1HomeViewModel(user);
            DataContext=guest1HomeViewModel;
            Main.Content = new Guest1SearchAccommodationView(guest1);

        }

       //servise ukloniti iz polja
       //nazivi
       //observ.u serv

      
        private void NotificationsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink[] links = MakeNotifications();
            NotificationsList.Items.Clear();
            foreach(Hyperlink link in links)
            {
                if(link.Tag.Equals(0))
                    NotificationsList.Items.Add(new MenuItem { Header = link, IsCheckable = false, Width=280, BorderBrush = Brushes.Black, Background=Brushes.PaleGreen}) ;
                else
                    NotificationsList.Items.Add(new MenuItem { Header = link, IsCheckable = false, Width = 280, BorderBrush = Brushes.Black, Background = Brushes.LightCoral });

            }
        }
        private Hyperlink CreateHyperlinkNotification(String notification, String state)
        {
            Hyperlink link = new Hyperlink();
            link.IsEnabled = true;
            link.Inlines.Add(notification);

            if(state.Equals("Approved"))
            {
                link.Click += NavigateToApprovedRequests_Click;
                link.Tag = 0;
            }
                
            else if(state.Equals("Declined"))
            {
                link.Click += NavigateToDeclinedRequests_Click;
                link.Tag = 1;
            }
                

            return link;
        }
        private void NavigateToApprovedRequests_Click(object sender, RoutedEventArgs e)
        {
            sentAccommodationReservationRequests = new SentAccommodationReservationRequestsView(guest1);
            sentAccommodationReservationRequests.RequestsTabControl.SelectedIndex = 0;
            Main.Content = sentAccommodationReservationRequests;    
        }
        private void NavigateToDeclinedRequests_Click(object sender, RoutedEventArgs e)
        {
            sentAccommodationReservationRequests = new SentAccommodationReservationRequestsView(guest1);
            sentAccommodationReservationRequests.RequestsTabControl.SelectedIndex = 2;
            Main.Content = sentAccommodationReservationRequests;
        }
        private Hyperlink[] MakeNotifications()
        {
            CompletedAccommodationReschedulingRequestService completedAccommodationReschedulingRequestService = new CompletedAccommodationReschedulingRequestService();
            List<CompletedAccommodationReschedulingRequest> completedRequests = completedAccommodationReschedulingRequestService.GetRequestsByGuest(guest1);
            completedRequests.Reverse();
            string[] notifications = new String[completedRequests.Count];
            Hyperlink[] links = new Hyperlink[completedRequests.Count];
            for (int i = 0; i < completedRequests.Count; i++)
            {
                notifications[i] = completedAccommodationReschedulingRequestService.GenerateNotification(completedRequests[i]);
                links[i]=CreateHyperlinkNotification(notifications[i], completedRequests[i].Request.state.ToString());
            }   
            return links;
        }
    }
}
//72 linije