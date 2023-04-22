﻿using System;
using System.Collections.Generic;
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
using MathNet.Numerics.Distributions;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class OwnerOverviewViewModel : INotifyPropertyChanged
    {
        public Owner ProfileOwner { get; set; }
        public RelayCommand RequestsCommand { get; set; }
        public RelayCommand ReviewGuestCommand { get; set; }
        private OwnerNotificationsService notificationsService;
        private double guestReviewsHeight = 0;
        private double reschedulingRequestsHeight = 0;

        public OwnerOverviewViewModel(Owner owner)
        {
            ProfileOwner = owner;
            RequestsCommand = new RelayCommand(Request_Executed, CanExecute);
            ReviewGuestCommand = new RelayCommand(ReviewGuest_Executed, CanExecute);
            notificationsService = new OwnerNotificationsService();
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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public double GuestReviewsHeight
        {
            get
            {
                if (notificationsService.HasGuestToReview(ProfileOwner))
                    return 54;
                else
                    return 0;
            }
            set
            {
                if (value != guestReviewsHeight)
                {
                    guestReviewsHeight = value;
                    OnPropertyChanged();
                }
            }
        }

        public double ReschedulingRequestsHeight
        {
            get
            {
                if (notificationsService.HasReschedulingRequest(ProfileOwner))
                    return 54;
                else
                    return 0;
            }
            set
            {
                if (value != reschedulingRequestsHeight)
                {
                    reschedulingRequestsHeight = value;
                    OnPropertyChanged();
                }
            }
        }


    }
}
