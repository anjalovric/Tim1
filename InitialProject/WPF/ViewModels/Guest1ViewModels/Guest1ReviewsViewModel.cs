﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest1Views;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class Guest1ReviewsViewModel :INotifyPropertyChanged
    {
        private Guest1 guest1;
        GuestReviewService guestReviewService;
        public double AverageCleanliness { get; set; }
        public int ReviewsNumber { get; set; }
        public double AverageFollowingRules { get; set; }
        public int AverageRating { get; set; }

        private ObservableCollection<GuestReview> guest1Reviews;
        public ObservableCollection<GuestReview> Guest1Reviews
        {
            get { return guest1Reviews; }
            set
            {
                if (value != guest1Reviews)
                    guest1Reviews = value;
                OnPropertyChanged("Ruest1Reviews");
            }

        }
        public GuestReview SelectedReview { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RelayCommand ShowReviewDetailsCommand { get; set; }
        public Guest1ReviewsViewModel(Guest1 guest1)
        {
            this.guest1 = guest1;
            guestReviewService = new GuestReviewService();
            Guest1Reviews = new ObservableCollection<GuestReview>(guestReviewService.GetAllToDisplay(guest1));
            SetRatings();
            ShowReviewDetailsCommand = new RelayCommand(ShowReviewDetails_Executed, CanExecute);
            
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private void ShowReviewDetails_Executed(object sender)
        {
            Guest1ReviewDetailsView view = new Guest1ReviewDetailsView(guest1, SelectedReview);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = view;

        }

        private void SetRatings()
        {
            GuestAverageReviewService guestAverageReviewService = new GuestAverageReviewService();
            AverageCleanliness = guestAverageReviewService.GetAverageCleanlinessReview(guest1);
            AverageFollowingRules = guestAverageReviewService.GetAverageFollowingRulesReview(guest1);
            ReviewsNumber = guestAverageReviewService.GetReviewsNumberByGuest(guest1);
            AverageRating = guestAverageReviewService.GetAverageRating(guest1);
            AverageCleanliness = Math.Round(AverageCleanliness, 1);
            AverageFollowingRules = Math.Round(AverageFollowingRules, 1);
        }
        
    }
}