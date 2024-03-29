﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class OwnerReviewViewModel : INotifyPropertyChanged
    {
        public OwnerReview OwnerReview { get; set; }

        private string imageUrl;
        public List<AccommodationReviewImage> Images { get; set; }
        private int imageCounter;
        public RelayCommand NextImageCommand { get; set; }
        public RelayCommand PreviousImageCommand { get; set; }
        public DateOnly Arrival { get; set; }
        private bool isCancelPressedInDemo = false;
        private bool isNextPicturePressedInDemo = false;
        private bool isDemoOn = false;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public OwnerReviewViewModel(OwnerReview ownerReview)
        {
            OwnerReview = ownerReview;
            Arrival = DateOnly.FromDateTime(OwnerReview.Reservation.Arrival);
            MakeImagesForReview();
            MakeFirstImage();
            imageCounter = 0;
            MakeCommands();
        }

        private void MakeCommands()
        {
            NextImageCommand = new RelayCommand(NextImage_Executed, CanExecute);
            PreviousImageCommand = new RelayCommand(PreviousImage_Executed, CanExecute);
        }

        private bool CanExecute(object sender)
        {
            if (Images.Count > 1)
                return true;
            else
                return false;
        }

        private void NextImage_Executed(object sender)
        {
            GetNextImage();
        }

        private void PreviousImage_Executed(object sender)
        {
            GetPreviousImage();
        }
        private void MakeFirstImage()
        {
            if (Images.Count > 0)
            {
                ImageUrl = "/" + Images[0].RelativeUri;
            }
        }

        private void MakeImagesForReview()
        {
            Images = new List<AccommodationReviewImage>();
            AccommodationReviewImageService imageService = new AccommodationReviewImageService();
            Images = imageService.GetAllByReservation(OwnerReview.Reservation);
        }

        public string ImageUrl
        {
            get => imageUrl;
            set
            {
                if (!value.Equals(imageUrl))
                {
                    imageUrl = value;
                    OnPropertyChanged();
                }
            }
        }

        public void GetNextImage()
        {
            if (imageCounter != Images.Count - 1)
                imageCounter += 1;
            else
                imageCounter = 0;
            ImageUrl = "/" + Images[imageCounter].RelativeUri;
        }

        public void GetPreviousImage()
        {
            if (imageCounter != Images.Count - 1)
                imageCounter += 1;
            else
                imageCounter = 0;
            ImageUrl = "/" + Images[imageCounter].RelativeUri;
        }

        public bool IsCancelPressedInDemo
        {
            get => isCancelPressedInDemo;
            set
            {
                if (value != isCancelPressedInDemo)
                {
                    isCancelPressedInDemo = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsNextPicturePressedInDemo
        {
            get => isNextPicturePressedInDemo;
            set
            {
                if (value != isNextPicturePressedInDemo)
                {
                    isNextPicturePressedInDemo = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsDemoOn
        {
            get => isDemoOn;
            set
            {
                if (value != isDemoOn)
                {
                    isDemoOn = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
