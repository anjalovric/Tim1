using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest1Views;
using NPOI.SS.Formula.Functions;
using System.Windows.Controls;
using System.Windows;
using InitialProject.WPF.Views.GuideViews;
using InitialProject.WPF.Views;
using System.Windows.Documents;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Drawing;
using System.Windows.Media;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class Guest1HomeViewModel:INotifyPropertyChanged
    {
        private Guest1 guest1;

        public RelayCommand BookingCommand { get; set; }
        public RelayCommand MyReservationsCommand { get; set; }
        public RelayCommand MyProfileCommand { get; set; }
        public RelayCommand SentRequestsCommand { get; set; }
        public RelayCommand SignOutCommand { get; set; }
        public RelayCommand NotificationsCommand { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<MenuItem> storedNotifications;
        public ObservableCollection<MenuItem> StoredNotifications
        {
            get { return storedNotifications; }
            set
            {
                if (value != storedNotifications)
                    storedNotifications = value;
                OnPropertyChanged("StoredNotifications");
            }

        }


        public Guest1HomeViewModel(User user)
        {
            Guest1Service guest1Service = new Guest1Service();
            this.guest1 = guest1Service.GetByUsername(user.Username);
            StoredNotifications = new ObservableCollection<MenuItem>();
            MakeCommands();

        }

        //servise ukloniti iz polja
        //nazivi

        private void MakeCommands()
        {
            BookingCommand = new RelayCommand(Booking_Executed, CanExecute);
            MyReservationsCommand = new RelayCommand(MyReservations_Executed, CanExecute);
            MyProfileCommand = new RelayCommand(MyProfile_Executed, CanExecute);
            SentRequestsCommand = new RelayCommand(SentRequests_Executed, CanExecute);
            SignOutCommand = new RelayCommand(SignOut_Executed, CanExecute);
            NotificationsCommand = new RelayCommand(Notifications_Executed, CanExecute);
        }

        private bool CanExecute(object sender)
        {
            return true;
        }

        private void Booking_Executed(object sender)
        {
            Guest1SearchAccommodationView guest1SearchAccommodationView = new Guest1SearchAccommodationView(guest1);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = guest1SearchAccommodationView;
        }

        private void MyReservations_Executed(object sender)
        {
            MyAccommodationReservationsView myReservationsView = new MyAccommodationReservationsView(guest1);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = myReservationsView;
        }
        
        private void SentRequests_Executed(object sender)
        {
            SentAccommodationReservationRequestsView sentAccommodationReservationRequestsView = new SentAccommodationReservationRequestsView(guest1);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = sentAccommodationReservationRequestsView;
        }
        private void MyProfile_Executed(object sender)
        {
            Guest1ProfileView guest1ProfileView = new Guest1ProfileView(guest1);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = guest1ProfileView;
        }

        private void SignOut_Executed(object sender)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Close();
        }

        private void Notifications_Executed(object sender)
        {
            System.Windows.Documents.Hyperlink[] links = MakeNotifications();
            StoredNotifications.Clear();
            foreach (System.Windows.Documents.Hyperlink link in links)
            {
                if (link.Tag.Equals(0))
                    StoredNotifications.Add(new MenuItem { Header = link, IsCheckable = false, Width = 280 });
                else
                    StoredNotifications.Add(new MenuItem { Header = link, IsCheckable = false, Width = 280});

            }
        }

        private System.Windows.Documents.Hyperlink CreateHyperlinkNotification(String notification, String state)
        {
            System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
            link.IsEnabled = true;
            link.Inlines.Add(notification);

            if (state.Equals("Approved"))
            {
                link.Click += NavigateToApprovedRequests_Click;
                link.Tag = 0;
            }

            else if (state.Equals("Declined"))
            {
                link.Click += NavigateToDeclinedRequests_Click;
                link.Tag = 1;
            }


            return link;
        }

        private void NavigateToApprovedRequests_Click(object sender, RoutedEventArgs e)
        {
            SentAccommodationReservationRequestsView sentAccommodationReservationRequestsView = new SentAccommodationReservationRequestsView(guest1);
            sentAccommodationReservationRequestsView.RequestsTabControl.SelectedIndex = 0;
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = sentAccommodationReservationRequestsView;
        }
        private void NavigateToDeclinedRequests_Click(object sender, RoutedEventArgs e)
        {
            SentAccommodationReservationRequestsView sentAccommodationReservationRequestsView = new SentAccommodationReservationRequestsView(guest1);
            sentAccommodationReservationRequestsView.RequestsTabControl.SelectedIndex = 2;
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = sentAccommodationReservationRequestsView;
        }

        private System.Windows.Documents.Hyperlink[] MakeNotifications()
        {
            CompletedAccommodationReschedulingRequestService completedAccommodationReschedulingRequestService = new CompletedAccommodationReschedulingRequestService();
            List<CompletedAccommodationReschedulingRequest> completedRequests = completedAccommodationReschedulingRequestService.GetRequestsByGuest(guest1);
            completedRequests.Reverse();
            string[] notifications = new String[completedRequests.Count];
            System.Windows.Documents.Hyperlink[] links = new System.Windows.Documents.Hyperlink[completedRequests.Count];
            for (int i = 0; i < completedRequests.Count; i++)
            {
                notifications[i] = completedAccommodationReschedulingRequestService.GenerateNotification(completedRequests[i]);
                links[i] = CreateHyperlinkNotification(notifications[i], completedRequests[i].Request.state.ToString());
            }
            return links;
        }


        }
    }//boja obavj, border obavj, klik na zvono ne radi iz prve, komande za link???
