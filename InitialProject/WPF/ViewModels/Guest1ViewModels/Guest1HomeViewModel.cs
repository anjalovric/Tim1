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
using Color = System.Windows.Media.Color;
using NPOI.SS.UserModel;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;
using System.Windows.Controls;

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

        public RelayCommand SubmenuOpenedCommand { get; set; }

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
            Guest1SearchAccommodationView guest1SearchAccommodationView = new Guest1SearchAccommodationView(guest1);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = guest1SearchAccommodationView;
            StoredNotifications = new ObservableCollection<MenuItem>();
            MakeCommands();
        }

        private void MakeCommands()
        {
            BookingCommand = new RelayCommand(Booking_Executed, CanExecute);
            MyReservationsCommand = new RelayCommand(MyReservations_Executed, CanExecute);
            MyProfileCommand = new RelayCommand(MyProfile_Executed, CanExecute);
            SentRequestsCommand = new RelayCommand(SentRequests_Executed, CanExecute);
            SignOutCommand = new RelayCommand(SignOut_Executed, CanExecute);
            NotificationsCommand = new RelayCommand(Notifications_Executed, CanExecute);
            SubmenuOpenedCommand = new RelayCommand(Submenu_Executed, CanExecute);
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

        private void Submenu_Executed(object sender)
        {
            MenuItem_SubmenuOpened(sender, null);
        }
        private void MenuItem_SubmenuOpened(object sender, RoutedEventArgs e) 
        {
            MenuItem owner = (MenuItem)sender;
            Popup child = (Popup)owner.Template.FindName("PART_Popup", owner);
            child.Placement = PlacementMode.Left;
            child.HorizontalOffset = owner.ActualWidth;
            child.VerticalOffset = owner.ActualHeight;
        }

        private void Notifications_Executed(object sender)
        {
            System.Windows.Documents.Hyperlink[] links = MakeNotifications();
            StoredNotifications.Clear();
            if (links.Length == 0)
                StoredNotifications.Add(new MenuItem { Header = "No recent notifications." });
            else
            foreach (System.Windows.Documents.Hyperlink link in links)
            {
                if (link.Tag.Equals(0))
                    StoredNotifications.Add(new MenuItem { Header = link, IsCheckable = false, Width = 300, Background = System.Windows.Media.Brushes.PaleGreen, BorderBrush = System.Windows.Media.Brushes.Black, BorderThickness = new Thickness(2), Icon = new Image { Source = new BitmapImage(new Uri("/Resources/Images/greenCorect.png", UriKind.Relative)) } }) ;
                else
                    StoredNotifications.Add(new MenuItem { Header = link, IsCheckable = false, Width = 300, Background = System.Windows.Media.Brushes.LightCoral, BorderBrush = System.Windows.Media.Brushes.Black, BorderThickness = new Thickness(2), Icon = new Image { Source = new BitmapImage(new Uri("/Resources/Images/redIncorect.png", UriKind.Relative)) } });
            }
        }

        private System.Windows.Documents.Hyperlink CreateHyperlinkNotification(String notification, String state)
        {
            System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink();
            link.IsEnabled = true;
            link.Inlines.Add(notification);
            SetCommandToLink(ref link, state);
            return link;
        }

        private void SetCommandToLink(ref System.Windows.Documents.Hyperlink link, String state)
        {
            if (state.Equals("Approved"))
            {
                link.Command = new RelayCommand(NavigateToApprovedRequests_Executed, CanExecute);
                link.Tag = 0;

            }
            else if (state.Equals("Declined"))
            {
                link.Command = new RelayCommand(NavigateToDeclinedRequests_Executed, CanExecute);
                link.Tag = 1;
            }
        }

        private void NavigateToApprovedRequests_Executed(object sender)
        {
            SentAccommodationReservationRequestsView sentAccommodationReservationRequestsView = new SentAccommodationReservationRequestsView(guest1);
            sentAccommodationReservationRequestsView.RequestsTabControl.SelectedIndex = 0;
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = sentAccommodationReservationRequestsView;
            Keyboard.ClearFocus();  //to highlight other controls when mouse on it
        }
        private void NavigateToDeclinedRequests_Executed(object sender)
        {
            SentAccommodationReservationRequestsView sentAccommodationReservationRequestsView = new SentAccommodationReservationRequestsView(guest1);
            sentAccommodationReservationRequestsView.RequestsTabControl.SelectedIndex = 2;
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = sentAccommodationReservationRequestsView;
            Keyboard.ClearFocus(); //to highlight other controls when mouse on it
        }

        private System.Windows.Documents.Hyperlink[] MakeNotifications()
        {
            CompletedAccommodationReschedulingRequestService completedAccommodationReschedulingRequestService = new CompletedAccommodationReschedulingRequestService();
            List<CompletedAccommodationReschedulingRequest> completedRequests = completedAccommodationReschedulingRequestService.GetRequestsByGuest(guest1);
            completedRequests.Reverse();    //last notification will be shown first in notifications list
            string[] notifications = new String[completedRequests.Count];
            System.Windows.Documents.Hyperlink[] links = new System.Windows.Documents.Hyperlink[completedRequests.Count];
            for (int i = 0; i < completedRequests.Count; i++)
            {
                notifications[i] = GenerateNotification(completedRequests[i]);
                links[i] = CreateHyperlinkNotification(notifications[i], completedRequests[i].Request.state.ToString());
            }
            return links;
        }
        public String GenerateNotification(CompletedAccommodationReschedulingRequest completedRequest)
        {
            return "Request status - " + completedRequest.Request.state.ToString().ToUpper() + "\nOwner: " + completedRequest.Request.Reservation.Accommodation.Owner.Name + " " + completedRequest.Request.Reservation.Accommodation.Owner.LastName + "\n"
                + "Name: " + completedRequest.Request.Reservation.Accommodation.Name
                + "\nFor: " + completedRequest.Request.NewArrivalDate.ToString("d") + " - " + completedRequest.Request.NewDepartureDate.ToString("d");
        }


    }
    }
