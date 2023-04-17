using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for GuestReview.xaml
    /// </summary>
    public partial class GuestReviewWindow : Window, INotifyPropertyChanged
    {
        private GuestReview review;
        private bool cleanlinessIsChecked;
        private bool followingRulesIsChecked;
        public ObservableCollection<AccommodationReservation> ReservationsToReview { get; set; }
        public GuestReview Review
        {
            get => review;
            set
            {
                if (value != review)
                {
                    review = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public GuestReviewWindow(AccommodationReservation reservationToReview, ObservableCollection<AccommodationReservation> allReservationsToReview)
        {
            InitializeComponent();
            DataContext = this;
            review = new GuestReview();

            review.Reservation = reservationToReview;
            ReservationsToReview = allReservationsToReview;

            OkButton.IsEnabled = false;
            cleanlinessIsChecked = false;
            followingRulesIsChecked = false;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            SaveGuestReview();
            RefreshReservationsForView();
            this.Close();
        }

        private void RefreshReservationsForView()
        {
            ReservationsToReview.Remove(Review.Reservation);
            OwnerOverview ownerOverview = Owner as OwnerOverview;
            ownerOverview.RefreshAlerts();
        }

        private void SaveGuestReview()
        {
            GuestReviewRepository guestReviewRepository = new GuestReviewRepository();
            guestReviewRepository.Add(Review);
        }

        private void Cleanliness_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)Cleanliness1.IsChecked)
                Review.Cleanliness = 1;
            else if ((bool)Cleanliness2.IsChecked)
                Review.Cleanliness = 2;
            else if ((bool)Cleanliness3.IsChecked)
                Review.Cleanliness = 3;
            else if ((bool)Cleanliness4.IsChecked)
                Review.Cleanliness = 4;
            else if ((bool)Cleanliness5.IsChecked)
                Review.Cleanliness = 5;

            cleanlinessIsChecked = true;
            EnableOkButton();
        }

        private void RulesFollowing_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)RulesFollowing1.IsChecked)
                Review.RulesFollowing = 1;
            else if ((bool)RulesFollowing2.IsChecked)
                Review.RulesFollowing = 2;
            else if ((bool)RulesFollowing3.IsChecked)
                Review.RulesFollowing = 3;
            else if ((bool)RulesFollowing4.IsChecked)
                Review.RulesFollowing = 4;
            else if ((bool)RulesFollowing5.IsChecked)
                Review.RulesFollowing = 5;

            followingRulesIsChecked = true;
            EnableOkButton();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EnableOkButton()
        {
            if (cleanlinessIsChecked && followingRulesIsChecked)
            {
                OkButton.IsEnabled = true;
            }
        }
    }
}

