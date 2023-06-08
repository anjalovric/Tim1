﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using InitialProject.WPF.Views;

namespace InitialProject.WPF.Demo
{
    public class MyProfileDemo : INotifyPropertyChanged
    {
        private int increment = -1;
        private MyProfileView profileView;
        private MyProfileViewModel profileViewModel;
        private OwnerReviewView ownerReviewView;
        private OwnerReviewViewModel ownerReviewViewModel;
        private OwnerReview review;
        public MyProfileDemo()
        {
            profileView = (MyProfileView?)Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content;
            profileViewModel = profileView.MyProfileViewModel;
            MakeReview();
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
            if (Increment == 1)
            {
                profileViewModel.SelectedOwnerReview = profileViewModel.OwnerReviews[profileViewModel.OwnerReviews.Count - 1];
                profileViewModel.IsViewPressedInDemo = true;
            }
            if(Increment == 4)
            {
                profileViewModel.IsViewPressedInDemo = false;
                ownerReviewView = new OwnerReviewView(review);
                Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = ownerReviewView;
                ownerReviewViewModel = ownerReviewView.OwnerReviewViewModel;
                AddImages();
                ownerReviewViewModel.IsDemoOn = true;
                ownerReviewViewModel.ImageUrl = "/Resources/Images/a1.jpg";
            }
            if(Increment == 7)
            {
                ownerReviewViewModel.IsNextPicturePressedInDemo = true;
                ownerReviewView.PressButtons();
            }
            if(Increment == 8)
            {
                ownerReviewViewModel.IsNextPicturePressedInDemo = false;
                ownerReviewView.PressButtons();
                ownerReviewViewModel.ImageUrl = "/Resources/Images/a2.jpg";
            }
            if(Increment == 10)
            {
                ownerReviewViewModel.IsCancelPressedInDemo = true;
                ownerReviewView.PressButtons();
            }
            if(Increment == 13)
            {
                Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = profileView;
            }
            if(Increment == 15)
            {
                profileViewModel.IsGeneratePressedInDemo = true;
            }
            if(Increment == 16)
            {
                profileViewModel.IsGeneratePressedInDemo = false;
                profileViewModel.GenerateReportCommand.Execute(null);
            }
            if(Increment==22)
            {
                Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = profileView;
            }
        }

        private void MakeReview()
        {
            review = new OwnerReview();
            review.Reservation.Guest.Name = "Anja";
            review.Reservation.Guest.LastName = "Ducic";
            review.Cleanliness = 5;
            review.Correctness = 5;
            review.Comment = "Great accommodation and polite owner. I enjoyed my visit!";
            review.Reservation.Arrival = DateTime.Now;
            review.Reservation.Accommodation.Name = "My accommodation";
            review.Id = 0;
            profileViewModel.OwnerReviews.Add(review);
        }

        private void AddImages()
        {
            AccommodationReviewImageService imageService = new AccommodationReviewImageService();
            ownerReviewViewModel.Images.Add(imageService.GetAll()[0]);
            ownerReviewViewModel.Images.Add(imageService.GetAll()[1]);
        }
    }
}
