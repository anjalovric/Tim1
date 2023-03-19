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
        private AccommodationReservation selectedReservation;
        private AccommodationRepository accommodationRepository;
        private OwnerRepository ownerRepository;
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public ObservableCollection<AccommodationReservation> ReservationsToReview { get; set; }
        public Model.Owner WindowOwner { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public AccommodationReservation SelectedReservation
        {
            get => selectedReservation;
            set
            {
                if (value != selectedReservation)
                {
                    selectedReservation = value;
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
            
            accommodationRepository = new AccommodationRepository();
            ownerRepository = new OwnerRepository();
            WindowOwner = new Model.Owner();
            GetOwnerByUser(user);

            MakeAccommodationsForView();
            MakeReservationsToReview();
            MakeAlerts();
        }

        private void MakeReservationsToReview()
        {
            ReservationsToReview = new ObservableCollection<AccommodationReservation>();
            GetAllReservationsForReview();
            SetGuestToReservation();
        }

        private void MakeAccommodationsForView()
        {
            Accommodations = new ObservableCollection<Accommodation>(GetAllByOwner());
            if(Accommodations.Count > 0)
            {
                SetAccommodationsLocation();
                SetAccommodationsType();
            }
        }

        private void GetAllReservationsForReview()
        {
            AccommodationReservationRepository reservationRepository = new AccommodationReservationRepository();
            List<AccommodationReservation> reservations = reservationRepository.GetAll();
            GuestReviewRepository guestReviewRepository = new GuestReviewRepository();

            if(reservations.Count > 0)
            {
                AddAccommodationToReservation(reservations);

                foreach (AccommodationReservation reservation in reservationRepository.GetAll())
                {
                    bool stayedLessThan5DaysAgo = (reservation.Departure.Date < DateTime.Now.Date) && (DateTime.Now.Date - reservation.Departure.Date).TotalDays <= 5;
                    bool alreadyReviewed = guestReviewRepository.HasReview(reservation);
                    bool isThisOwner = reservation.Accommodation.Owner.Id == WindowOwner.Id;
                    
                    if (stayedLessThan5DaysAgo && !alreadyReviewed && isThisOwner)
                        ReservationsToReview.Add(reservation);
                }
            }
        }

        private void AddAccommodationToReservation(List<AccommodationReservation> reservations)
        {
            List<Accommodation> accommodations = new List<Accommodation>(accommodationRepository.GetAll());

            foreach (AccommodationReservation reservation in reservations)
            {
                Accommodation reservationAccommodation = accommodations.Find(n => n.Id == reservation.Accommodation.Id);
                if (reservationAccommodation != null)
                {
                    reservation.Accommodation = reservationAccommodation;
                    SetOwnerToAccommodation(reservation);
                }
            }
        }

        private void AddAccommodationButton_Click(object sender, RoutedEventArgs e)
        {
            AccommodationForm accommodationForm = new AccommodationForm(Accommodations, WindowOwner);
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
            GuestReviewWindow guestReview = new GuestReviewWindow(SelectedReservation, ReservationsToReview);
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
            foreach(AccommodationReservation reservation in ReservationsToReview)
            {
                Label guestLabel = new Label();
                guestLabel.Background = Brushes.CadetBlue;
                guestLabel.Content = "You have not rated " + reservation.Guest.Name + " " + reservation.Guest.LastName;
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
            WindowOwner = ownerRepository.GetByUsername(user.Username);
        }

        private void SetOwnerToAccommodation(AccommodationReservation reservation)
        {
            reservation.Accommodation.Owner = ownerRepository.GetById(reservation.Accommodation.Owner.Id);
        }

        private List<Accommodation> GetAllByOwner()
        {
            List<Accommodation> accommodationsByOwner = new List<Accommodation>();

            foreach (Accommodation accommodation in accommodationRepository.GetAll())
            {
                if (accommodation.Owner.Id == WindowOwner.Id)
                {
                    accommodationsByOwner.Add(accommodation);
                }
            }
            return accommodationsByOwner;
        }

        private void PicturesButton_Click(object sender, RoutedEventArgs e)
        {
             AccommodationImageRepository accommodationImageRepository = new AccommodationImageRepository();
             Accommodation selectedAccommodation = AccommodationDataGrid.SelectedItem as Accommodation;

            try
            {
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
            }catch(Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
            }

        }

        private void SetAccommodationsLocation()
        {
            LocationRepository locationRepository = new LocationRepository();
            List<Location> allLocations = locationRepository.GetAll();

            foreach (Accommodation accommodation in Accommodations)
            {
                Location accommodationLocation = allLocations.Find(n => n.Id == accommodation.Location.Id);
                if (accommodationLocation != null)
                {
                    accommodation.Location = accommodationLocation;
                }
            }
        }

        private void SetAccommodationsType()
        {
            AccommodationTypeRepository typeRepository = new AccommodationTypeRepository();
            List<AccommodationType> types = typeRepository.GetAll();

            foreach (Accommodation accommodation in Accommodations)
            {
                AccommodationType type = types.Find(n => n.Id == accommodation.Type.Id);
                if (type != null)
                {
                    accommodation.Type = type;
                }
            }
        }

        private void SetGuestToReservation()
        {
            Guest1Repository guest1Repository = new Guest1Repository();
            List<Guest1> allGuests = new List<Guest1>(guest1Repository.GetAll());

            foreach(AccommodationReservation reservation in ReservationsToReview)
            {
                Guest1 reservationGuest = allGuests.Find(n => n.Id == reservation.Guest.Id);
                if (reservationGuest != null)
                {
                    reservation.Guest = reservationGuest;
                }
            }
        }
    }
}
