using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.View.Owner;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for OwnerOverview.xaml
    /// </summary>
    public partial class OwnerOverview : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Accommodation> accommodations { get; set; }
        public ObservableCollection<Guest1> guests { get; set; }
        private Guest1 selectedGuest;
        public Model.Owner WindowOwner { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Guest1 SelectedGuest
        {
            get => selectedGuest;
            set
            {
                if (value != selectedGuest)
                {
                    selectedGuest = value;
                    OnPropertyChanged();
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public OwnerOverview(User user)
        {
            InitializeComponent();
            DataContext = this;
            WindowOwner = new Model.Owner();
            GetOwnerByUser(user);
            accommodations = new ObservableCollection<Accommodation>(GetAllByOwner());
            guests = new ObservableCollection<Guest1>();
            GetAllGuestsToReview();
            MakeAlert();
        }

        private void GetAllGuestsToReview()
        {
            AccommodationReservationRepository reservationRepository = new AccommodationReservationRepository();
            List<AccommodationReservation> reservations = reservationRepository.GetAll();
            Guest1Repository guest1Repository = new Guest1Repository();
            GuestReviewRepository guestReviewRepository = new GuestReviewRepository();
            AddAccommodationToReservation(reservations);
            
            foreach (Guest1 guest in guest1Repository.GetAll())
            {
                AccommodationReservation reservation = reservations.Find(n => n.GuestId == guest.Id);
                if (reservation != null)
                {
                    bool hasReservation = reservation != null;
                    bool stayedLessThan5DaysAgo = (reservation.LeavingDate.Date < DateTime.Now.Date) && (DateTime.Now.Date - reservation.LeavingDate.Date).TotalDays < 5;
                    bool alreadyReviewed = guestReviewRepository.HasReview(guest);
                    bool isThisOwner = reservation.currentAccommodation.Owner.Id == WindowOwner.Id;
                    if (hasReservation && stayedLessThan5DaysAgo && !alreadyReviewed)
                        guests.Add(guest);
                }
            }
        }

        private void AddAccommodationToReservation(List<AccommodationReservation> reservations)
        {
            AccommodationRepository accommodationRepository = new AccommodationRepository();
            List<Accommodation> accommodations = new List<Accommodation>(accommodationRepository.GetAll());
            foreach (AccommodationReservation reservation in reservations)
            {
                reservation.currentAccommodation = accommodations.Find(n => n.Id == reservation.currentAccommodation.Id);
                GetOwner(reservation);
            }
        }
        private void AddAccommodationClick(object sender, RoutedEventArgs e)
        {
            AccommodationForm accommodationForm = new AccommodationForm(accommodations, WindowOwner);
            accommodationForm.Show();
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            this.Close();
        }

        private void Review_Click(object sender, RoutedEventArgs e)
        {
            GuestReview guestReview = new GuestReview(SelectedGuest, guests);
            guestReview.Show();
        }

        private void ReviewOverview_Click(object sender, RoutedEventArgs e)
        {
            GuestReviewsOverview guestReviewsOverview = new GuestReviewsOverview();
            guestReviewsOverview.Show();

        }

        private void MakeAlert()
        {
            foreach(Guest1 guest in guests)
            {
                Label guestLabel = new Label();
                guestLabel.Background = Brushes.CadetBlue;
                guestLabel.Content = "You have not reviewed " + guest.Name + " " + guest.LastName;
                NotificationStack.Children.Add(guestLabel);
                NotificationStack.Background = Brushes.LightGreen;
            }
        }

        private void GetOwnerByUser(User user)
        {
            OwnerRepository ownerRepository = new OwnerRepository();
            WindowOwner = ownerRepository.GetByUsername(user.Username);
        }

        private void GetOwner(AccommodationReservation reservation)
        {
            OwnerRepository ownerRepository = new OwnerRepository();
            reservation.currentAccommodation.Owner = ownerRepository.GetById(reservation.currentAccommodation.Owner.Id);
        }

        private List<Accommodation> GetAllByOwner()
        {
            List<Accommodation> result = new List<Accommodation>();
            AccommodationRepository accommodationRepository = new AccommodationRepository();
            foreach (Accommodation accommodation in accommodationRepository.GetAll())
            {
                if (accommodation.Owner.Id == WindowOwner.Id)
                {
                    result.Add(accommodation);
                }
            }
            return result;
        }
        private void PicturesButton_Click(object sender, RoutedEventArgs e)
        {
            AccommodationImageRepository accommodationImageRepository = new AccommodationImageRepository();
            Accommodation selectedAccommodation = AccommodationDataGrid.SelectedItem as Accommodation;
            List<string> images = new List<string>(accommodationImageRepository.GetUrlByAccommodationId(selectedAccommodation.Id));
            if (images.Count > 0)
            {
                AccommodationPhotosView accommodationPhotosView = new AccommodationPhotosView(images);
                accommodationPhotosView.Show();
            }
            else
            {
                MessageBox.Show("No available pictures", "No Picture", MessageBoxButton.OK);
            }
        }
    }
}
