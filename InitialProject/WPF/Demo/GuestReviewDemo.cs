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

        private DemoIsOffView demoIsOffView;
        private DemoIsOnView demoIsOnView;
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
                demoIsOnView = new DemoIsOnView();
                demoIsOnView.Show();
            }
            if(Increment == 3)
            {
                demoIsOnView.Close();
                viewModel.ReservationsToReview.Add(guestToReview);
                viewModel.SelectedReservation = viewModel.ReservationsToReview[viewModel.ReservationsToReview.Count - 1];
                view.GuestReviewsListBox.SelectedItem = viewModel.SelectedReservation;
            }
            if(Increment == 5)
            {
                Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = formView;
            }
            if (Increment == 6)
            {
                formView.Cleanliness3.IsChecked = true;
                form.GuestReview.Cleanliness = 3;
            }
            if(Increment == 7)
            {
                formView.RulesFollowing4.IsChecked = true;
                form.GuestReview.RulesFollowing = 4;
            }
            if(Increment == 8)
            {
                formView.InputCommentInDemo();
            }
            if(Increment == 16)
            {
                form.IsOkButtonEnabled = true;
            }
            if(Increment == 17)
            {
                form.IsConfirmPressedInDemo = true;
            }
            if(Increment == 18)
            {
                MakeGuestReview();
                Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = view;
                viewModel.GuestReviews.Add(review);
                viewModel.StackPanelMessage = "Guest successfully reviewed!";
                viewModel.StackPanelVisibility = "Visible";
                viewModel.ReservationsToReview.Remove(viewModel.ReservationsToReview[viewModel.ReservationsToReview.Count-1]);
            }
            if(Increment == 19)
            {
                viewModel.IsOkPressedInDemo = true;
            }
            if(Increment == 20)
            {
                viewModel.IsOkPressedInDemo = false;
                viewModel.StackPanelVisibility = "Hidden";
            }
            if(Increment == 21)
            {
                viewModel.GuestReviews.Remove(review);
                demoIsOffView = new DemoIsOffView();
                demoIsOffView.Show();
            }
            if(Increment == 23)
            {
                demoIsOffView.Close();
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
