using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using DotLiquid.Tags;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.OwnerViews;
using Xceed.Wpf.Toolkit;

namespace InitialProject.WPF.Demo
{
    public class GuestReviewDemo : INotifyPropertyChanged
    {
        private int increment = -1;
        private GuestReviewViewModel viewModel;
        private GuestReviewView view;

        private GuestReviewFormViewModel form;
        private GuestReviewFormView formView;
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
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = formView;

            if (Increment == 1)
            {
                formView.Cleanliness3.IsChecked = true;
                form.GuestReview.Cleanliness = 3;
            }
            if(Increment == 2)
            {
                formView.RulesFollowing4.IsChecked = true;
                form.GuestReview.RulesFollowing = 4;
            }
            if(Increment == 3)
            {
                formView.InputCommentInDemo();
            }
            if(Increment == 11)
            {
                form.IsOkButtonEnabled = true;
            }
            if(Increment == 12)
            {
                form.IsConfirmPressedInDemo = true;
            }
            if(Increment == 13)
            {
                Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = view;
            }
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
