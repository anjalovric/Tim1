using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.OwnerViews;

namespace InitialProject.WPF.Demo
{
    public class GuestReviewDemo : INotifyPropertyChanged
    {
        private int increment = -1;
        private GuestReviewViewModel viewModel;
        private GuestReviewView view;

        private GuestReviewFormViewModel form;
        private GuestReviewFormView formView;
        private AccommodationReservation guestToReview;
        private GuestReview review;
        public GuestReviewDemo()
        {
            OwnerService ownerService = new OwnerService();
            Owner owner = ownerService.GetAll()[0];
            AccommodationReservationService reservationService = new AccommodationReservationService();
            view = new GuestReviewView(owner);
            formView = new GuestReviewFormView(reservationService.GetAll()[0]);
            form = formView.ViewModel;
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = view;
            viewModel = view.ViewModel;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void PlayDemo()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += new EventHandler(Timer_Tick);
            dispatcherTimer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            Increment++;
            MakeGuestToReview();
            if(Increment == 1)
            {
                viewModel.ReservationsToReview.Add(guestToReview);
                viewModel.SelectedReservation = viewModel.ReservationsToReview[viewModel.ReservationsToReview.Count - 1];
            }
            if(Increment == 2)
            {
                Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = formView;
            }
            if (Increment == 3)
            {
                formView.Cleanliness3.IsChecked = true;
                form.GuestReview.Cleanliness = 3;
            }
            if(Increment == 4)
            {
                formView.RulesFollowing4.IsChecked = true;
                form.GuestReview.RulesFollowing = 4;
            }
            if(Increment == 5)
            {
                formView.InputCommentInDemo();
            }
            if(Increment == 13)
            {
                form.IsOkButtonEnabled = true;
            }
            if(Increment == 14)
            {
                form.IsConfirmPressedInDemo = true;
            }
            if(Increment == 15)
            {
                MakeGuestReview();
                Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = view;
                viewModel.GuestReviews.Add(review);
                viewModel.StackPanelMessage = "Guest successfully reviewed!";
                viewModel.StackPanelVisibility = "Visible";
            }
            if(Increment == 16)
            {
                viewModel.IsOkPressedInDemo = true;
            }
            if(Increment == 17)
            {
                viewModel.IsOkPressedInDemo = false;
                viewModel.StackPanelVisibility = "Hidden";
            }
            if(Increment == 18)
            {
                viewModel.ReservationsToReview.Remove(guestToReview);
                viewModel.GuestReviews.Remove(review);
            }
        }

        private void MakeGuestToReview()
        {
            guestToReview = new AccommodationReservation();
            guestToReview.Accommodation.Name = "My Accommodation";
            LocationService locationService = new LocationService();
            guestToReview.Accommodation.Location = locationService.GetAll()[0];
            Guest1Service guest1Service = new Guest1Service();
            guestToReview.Guest = guest1Service.GetById(1);
        }

        private void MakeGuestReview()
        {
            review = new GuestReview();
            review.Reservation = guestToReview;
            review.Cleanliness = 3;
            review.RulesFollowing = 4;
        }
        public int Increment
        {
            get { return increment; }
            set
            {
                if (value != increment)
                {
                    increment = value;
                    OnPropertyChanged();
                }
            }
        }
        
    }

    
}
