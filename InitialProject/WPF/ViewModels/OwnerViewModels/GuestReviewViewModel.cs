using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.OwnerViews;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class GuestReviewViewModel : INotifyPropertyChanged
    {
        private Owner profileOwner;
        private AccommodationReservation selectedReservation;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<AccommodationReservation> ReservationsToReview { get; set; }
        public ObservableCollection<GuestReview> GuestReviews { get; set; }
        public RelayCommand ReviewCommand { get; set; }
        public GuestReviewViewModel(Owner owner)
        {
            profileOwner = owner;
            MakeReservationsToReview();
            MakeGuestReviews();
            ReviewCommand = new RelayCommand(Review_Executed, CanExecute);
        }

        private void MakeReservationsToReview()
        {
            AccommodationReservationService reservationService = new AccommodationReservationService();
            ReservationsToReview = new ObservableCollection<AccommodationReservation>(reservationService.GetAllForReviewByOwner(profileOwner));
        }

        private void MakeGuestReviews()
        {
            GuestReviewService guestReviewService = new GuestReviewService();
            GuestReviews = new ObservableCollection<GuestReview>(guestReviewService.GetAllByOwner(profileOwner));
        }
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

        private bool CanExecute(object sender)
        {
            return true;
        }

        private void Review_Executed(object sender)
        {
            if (SelectedReservation != null)
            {
                GuestReviewFormView guestReviewFormView = new GuestReviewFormView(SelectedReservation);
                Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = guestReviewFormView;
            }
        }
    }
}
