﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest1Views;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class Guest1ProfileViewModel : INotifyPropertyChanged
    {
        public Guest1 Guest1 { get; set; }
        private BitmapImage imageSource;
        public BitmapImage ImageSource
        {
            get { return imageSource; }
            set
            {
                if (value != imageSource)
                    imageSource = value;
                OnPropertyChanged("ImageSource");
            }

        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int AverageRating { get; set; }
        public int ReviewsNumber { get; set; }
        public Guest1ProfileViewModel(Guest1 guest1)
        {
            this.Guest1 = guest1;
            string relative = FindImageRelativePath();
            ImageSource = new BitmapImage(new Uri(relative, UriKind.Relative));
            GetAverageRating();
            GetReviewsNumber();
        }
        private void GetReviewsNumber()
        {
            GuestReviewService guestReviewService = new GuestReviewService();
            ReviewsNumber = guestReviewService.GetReviewsNumberByGuest(Guest1);
        }
        private void GetAverageRating()
        {
            GuestReviewService guestReviewService = new GuestReviewService();
            AverageRating = guestReviewService.GetAverageRating(Guest1);
        }
        private string FindImageRelativePath()
        {
            return Guest1.ImagePath;
        }
        
    }
}
