using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using InitialProject.Model;
using InitialProject.WPF.Views.OwnerViews;
using InitialProject.WPF.Views;
using InitialProject.Service;
using System.Windows;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class GuestReviewFormViewModel : INotifyPropertyChanged
    {
        public Guest1 Guest { get; set; }
        private AccommodationReservation reservationToReview;
        public GuestReview GuestReview { get; set; }
        private bool isOkButtonEnabled = false;
        public RelayCommand SelectCleanlinessCommand { get; set; }
        public RelayCommand SelectRulesFollowingCommand { get; set; }
        public RelayCommand ConfirmCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public GuestReviewFormViewModel(AccommodationReservation reservation)
        {
            reservationToReview = reservation;
            Guest = reservation.Guest;
            GuestReview = new GuestReview();
            GuestReview.Reservation = reservationToReview;
            MakeCommands();
        }

        private void MakeCommands()
        {
            SelectCleanlinessCommand = new RelayCommand(SelectCleanliness_Executed, CanExecute);
            SelectRulesFollowingCommand = new RelayCommand(SelectRulesFollowing_Executed, CanExecute);
            ConfirmCommand = new RelayCommand(ConfirmCommand_Executed, CanExecute);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsOkButtonEnabled
        {
            get => isOkButtonEnabled;
            set
            {
                if (value != isOkButtonEnabled)
                {
                    isOkButtonEnabled = value;
                    OnPropertyChanged();
                }
            }
        }


        private bool CanExecute(object sender)
        {
            return true;
        }

        private void SelectCleanliness_Executed(object sender)
        {
            GuestReview.Cleanliness = Convert.ToInt32(sender);
            EnableOkButton();
        }

        private void SelectRulesFollowing_Executed(object sender)
        {
            GuestReview.RulesFollowing = Convert.ToInt32(sender);
            EnableOkButton();
        }

        private void ConfirmCommand_Executed(object sender)
        {
            GuestReviewService guestReviewService = new GuestReviewService();
            guestReviewService.Add(GuestReview);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = new GuestReviewView(reservationToReview.Accommodation.Owner);
        }
        private void EnableOkButton()
        {
            if (GuestReview.Cleanliness != 0 && GuestReview.RulesFollowing != 0)
            {
                IsOkButtonEnabled = true;
            }
        }
    }
}
