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
        public RelayCommand OKCommand { get; set; }
        private string stackPanelVisibility;
        private string stackPanelMessage;
        private bool isOkPressedInDemo;
        public GuestReviewViewModel(Owner owner)
        {
            profileOwner = owner;
            MakeReservationsToReview();
            MakeGuestReviews();
            ReviewCommand = new RelayCommand(Review_Executed, CanExecute);
            OKCommand = new RelayCommand(OK_Executed, CanExecute);
            DisplayNotificationPanel();
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

        private void OK_Executed(object sender)
        {
            StackPanelVisibility = "Hidden";
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

        public bool IsOkPressedInDemo
        {
            get { return isOkPressedInDemo; }
            set
            {
                if (value != isOkPressedInDemo)
                {
                    isOkPressedInDemo = value;
                    OnPropertyChanged();
                }
            }
        }
        public string StackPanelMessage
        {
            get { return stackPanelMessage; }
            set
            {
                if (value != stackPanelMessage)
                {
                    stackPanelMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StackPanelVisibility
        {
            get { return stackPanelVisibility; }
            set
            {
                if (value != stackPanelVisibility)
                {
                    stackPanelVisibility = value;
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

        private void DisplayNotificationPanel()
        {
            OwnerNotificationsService notificationsService = new OwnerNotificationsService();
            if (notificationsService.IsGuestReviewed(profileOwner))
            {
                StackPanelMessage = "Guest successfully reviewed!";
                StackPanelVisibility = "Visible";
                notificationsService.Delete(Domain.Model.OwnerNotificationType.GUEST_REVIEWED, profileOwner);
            }
            else
            {
                StackPanelVisibility = "Hidden";
            }
        }
    }
}
