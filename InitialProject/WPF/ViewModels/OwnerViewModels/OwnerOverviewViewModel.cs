using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.OwnerViews;
using MathNet.Numerics.Distributions;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class OwnerOverviewViewModel
    {
        public Owner ProfileOwner { get; set; }
        public RelayCommand RequestsCommand { get; set; }
        public RelayCommand ReviewGuestCommand { get; set; }
        public OwnerOverviewViewModel(Owner owner)
        {
            ProfileOwner = owner;
            RequestsCommand = new RelayCommand(Request_Executed, CanExecute);
            ReviewGuestCommand = new RelayCommand(ReviewGuest_Executed, CanExecute);
        }

        private bool CanExecute(object sender)
        {
            return true;
        }

        private void Request_Executed(object sender)
        {
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = new ReservationReschedulingView(ProfileOwner);
        }

        private void ReviewGuest_Executed(object sender)
        {
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = new GuestReviewView(ProfileOwner);
        }
    }
}
