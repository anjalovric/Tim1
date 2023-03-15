using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            SetAccommodationsLocation();
            SetAccommodationsType();
            guests = new ObservableCollection<Guest1>();
            GetAllGuestsForReview();
            MakeAlerts();
        }

        private void GetAllGuestsForReview()
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
                    bool stayedLessThan5DaysAgo = (reservation.Departure.Date < DateTime.Now.Date) && (DateTime.Now.Date - reservation.Departure.Date).TotalDays <= 5;
                    bool alreadyReviewed = guestReviewRepository.HasReview(guest);
                    bool isThisOwner = reservation.Accommodation.Owner.Id == WindowOwner.Id;
                    if (hasReservation && stayedLessThan5DaysAgo && !alreadyReviewed && isThisOwner)
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
                reservation.Accommodation = accommodations.Find(n => n.Id == reservation.Accommodation.Id);
                GetOwner(reservation);
            }
        }

        private void AddAccommodationButton_Click(object sender, RoutedEventArgs e)
        {
            AccommodationForm accommodationForm = new AccommodationForm(accommodations, WindowOwner);
            accommodationForm.Show();
        }

        private void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            this.Close();
        }

        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            GuestReviewWindow guestReview = new GuestReviewWindow(SelectedGuest, guests, WindowOwner);
            guestReview.Owner = this;
            guestReview.Show();
        }

        private void ReviewOverviewButton_Click(object sender, RoutedEventArgs e)
        {
            GuestReviewsOverview guestReviewsOverview = new GuestReviewsOverview(WindowOwner);
            guestReviewsOverview.Show();
        }

        private void MakeAlerts()
        {
            foreach(Guest1 guest in guests)
            {
                Label guestLabel = new Label();
                guestLabel.Background = Brushes.CadetBlue;
                guestLabel.Content = "You have not rated " + guest.Name + " " + guest.LastName;
                NotificationStack.Children.Add(guestLabel);
                NotificationStack.Background = Brushes.LightGreen;
            }
        }

        public void RefreshAlerts()
        {
            NotificationStack.Children.Clear();
            MakeAlerts();
        }

        private void GetOwnerByUser(User user)
        {
            OwnerRepository ownerRepository = new OwnerRepository();
            WindowOwner = ownerRepository.GetByUsername(user.Username);
        }

        private void GetOwner(AccommodationReservation reservation)
        {
            OwnerRepository ownerRepository = new OwnerRepository();
            reservation.Accommodation.Owner = ownerRepository.GetById(reservation.Accommodation.Owner.Id);
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

        public void SetAccommodationsLocation()
        {
            LocationRepository locationRepository = new LocationRepository();
            List<Location> locations = locationRepository.GetAll();
            foreach (Accommodation accommodation in accommodations)
            {
                if (locations.Find(n => n.Id == accommodation.Location.Id) != null)
                {
                    accommodation.Location = locations.Find(n => n.Id == accommodation.Location.Id);
                }
            }
        }

        public void SetAccommodationsType()
        {
            AccommodationTypeRepository typeRepository = new AccommodationTypeRepository();
            List<AccommodationType> types = typeRepository.GetAll();
            foreach (Accommodation accommodation in accommodations)
            {
                if (types.Find(n => n.Id == accommodation.Type.Id) != null)
                {
                    accommodation.Type = types.Find(n => n.Id == accommodation.Type.Id);
                }
            }
        }
    }
}
