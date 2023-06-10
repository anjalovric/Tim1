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
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using NPOI.SS.UserModel;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;
using System.Windows.Controls;
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
        public RelayCommand SubmenuOpenedCommand { get; set; }
        public RelayCommand AnywhereAnytimeCommand { get; set; }
        public RelayCommand ReviewsCommand { get; set; }
        public RelayCommand ForumCommand { get; set; }
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
        private SuperGuestTitleService superGuestTitleService;
        public Guest1HomeViewModel(User user)
        {
            Guest1Service guest1Service = new Guest1Service();
            this.guest1 = guest1Service.GetByUsername(user.Username);
            Initialize();
            ShowSuperGuest();
            MakeCommands();
        }
        private void Initialize()
        {
            superGuestTitleService = new SuperGuestTitleService();
            Guest1SearchAccommodationView guest1SearchAccommodationView = new Guest1SearchAccommodationView(guest1);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = guest1SearchAccommodationView;
            StoredNotifications = new ObservableCollection<MenuItem>();
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
            ReviewsCommand = new RelayCommand(Reviews_Executed, CanExecute);
            AnywhereAnytimeCommand = new RelayCommand(AnywhereAnytime_Executed, CanExecute);
            ForumCommand = new RelayCommand(Forum_Executed, CanExecute);    
        }

        //execute commands
        private void AnywhereAnytime_Executed(object sender)
        {
            AnywhereAnytimeView anywhereAnytimeView = new AnywhereAnytimeView(guest1);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = anywhereAnytimeView;
        }

        private void Booking_Executed(object sender)
        {
            Guest1SearchAccommodationView guest1SearchAccommodationView = new Guest1SearchAccommodationView(guest1);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = guest1SearchAccommodationView;
        }
        private void Forum_Executed(object sender)
        {
            ForumView forumView = new ForumView(guest1);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = forumView;
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

        private void Reviews_Executed(object sender)
        {
            Guest1ReviewsView guest1ReviewsView = new Guest1ReviewsView(guest1);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = guest1ReviewsView;
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
                StoredNotifications.Add(new MenuItem { Header = "No recent notifications.", FontWeight = FontWeights.SemiBold, IsCheckable = false, Width = 300, BorderBrush = System.Windows.Media.Brushes.Black, Background = System.Windows.Media.Brushes.BlanchedAlmond, BorderThickness = new Thickness(3), Margin = new Thickness(1), VerticalAlignment = System.Windows.VerticalAlignment.Center, Height = 40, Icon = new Image { Source = new BitmapImage(new Uri("/Resources/Images/nothing.png", UriKind.Relative)), Margin = new Thickness(0, -10, -7, -3), VerticalAlignment = System.Windows.VerticalAlignment.Bottom }});
            else
            foreach (System.Windows.Documents.Hyperlink link in links)
            {
                    if (link.Tag.Equals(0))
                        StoredNotifications.Add(new MenuItem { Header = link, FontWeight = FontWeights.SemiBold, IsCheckable = false, Width = 300,Background = new LinearGradientBrush(new GradientStopCollection(){new GradientStop(Color.FromRgb(150, 206, 154), 0.9),new GradientStop(Color.FromRgb(252, 236, 185), 0.1),},  new Point(0, 0), new Point(1,1)), BorderBrush = System.Windows.Media.Brushes.Black, BorderThickness = new Thickness(3), Margin = new Thickness(1),  Icon = new Image { Source = new BitmapImage(new Uri("/Resources/Images/done.png", UriKind.Relative)), Margin = new Thickness(0,-10,-7,-3), VerticalAlignment = System.Windows.VerticalAlignment.Bottom } }); 
                    else
                        StoredNotifications.Add(new MenuItem { Header = link,  FontWeight = FontWeights.SemiBold, IsCheckable = false, Width = 300,Background = new LinearGradientBrush(new GradientStopCollection(){new GradientStop(Color.FromRgb(240, 128, 128), 0.9),new GradientStop(Color.FromRgb(252, 236, 185), 0.1),},  new Point(0, 0),new Point(1, 1)), BorderBrush = System.Windows.Media.Brushes.Black, BorderThickness = new Thickness(3), Margin = new Thickness(1), Icon = new Image { Source = new BitmapImage(new Uri("/Resources/Images/delete.png", UriKind.Relative)), Margin = new Thickness(0, -10, -7, -3), VerticalAlignment = System.Windows.VerticalAlignment.Bottom } });
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

        //other methods
        private void ShowSuperGuest()
        {
            superGuestTitleService.DeleteTitleIfNeeded(guest1);
            if (superGuestTitleService.IsAlreadySuperGuest(guest1))
            {
                superGuestTitleService.ProlongSuperGuestTitle(guest1);  //add new or delete previous title.
            }
            superGuestTitleService.MakeNewSuperGuest(guest1);
            superGuestTitleService.IsAlreadySuperGuest(guest1);
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
            return "Request status - " + completedRequest.Request.state.ToString().ToUpper()
                + "\nName: " + completedRequest.Request.Reservation.Accommodation.Name
                + "\nOwner: " + completedRequest.Request.Reservation.Accommodation.Owner.Name + " " + completedRequest.Request.Reservation.Accommodation.Owner.LastName
                + "\nFor: " + completedRequest.Request.NewArrivalDate.ToString("d") + " - " + completedRequest.Request.NewDepartureDate.ToString("d");
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
    }
}
