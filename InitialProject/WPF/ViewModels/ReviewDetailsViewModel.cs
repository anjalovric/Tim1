using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.GuideViews;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace InitialProject.WPF.ViewModels
{
    public class ReviewDetailsViewModel:INotifyPropertyChanged
    {
        public string Name { get;set; }
        public string Language { get;set; }
        public string StartDate { get;set; }
        public Location Location { get;set; }
        public string StartTime { get;set; }

        public int Guide { get; set; }
        public int LanguageKnowledge { get; set; }

        public int Interestigness { get; set; } 

        public string Comment { get; set; }
        public string Guest { get; set; }

        private GuideAndTourReview currentReview;
        public ObservableCollection<string> Points { get; set; }

        private StackPanel toastMessage;

        private List<TourReviewImage> images;

        private BitmapImage current;
        public BitmapImage Current
        {
            get => current;
            set
            {
                if (value != current)
                {
                    current = value;
                    OnPropertyChanged();
                }
            }
        }
        private int currentCounter = 0;
        public ReviewDetailsViewModel(GuideAndTourReview review,StackPanel toast) 
        { 
            SetTourDetails(review);
            SetGrades(review);
            currentReview= review;
            toastMessage = toast;
        }

        public void SetTourDetails(GuideAndTourReview review)
        {
            Name=review.TourInstance.Tour.Name;
            Language=review.TourInstance.Tour.Language;
            Location = review.TourInstance.Tour.Location;
            StartDate = review.TourInstance.Date;
            StartTime = review.TourInstance.StartClock;
            FindCheckPoints(review);
            SetFirstImages(review);

        }

        public void SetGrades(GuideAndTourReview review)
        {
            Guide = review.Knowledge;
            Interestigness = review.InterestingFacts;
            LanguageKnowledge = review.Language;
            Comment = review.Comment;
            Guest = review.Guest2.Name + " " + review.Guest2.LastName + "'s review";
        }

        private void FindCheckPoints(GuideAndTourReview review)
        {
            TourDetailsService tourDetailsService = new TourDetailsService();
            Points= new ObservableCollection<string>(tourDetailsService.GetPointsForGuest(review.Guest2.Id,review.TourInstance));
        }

        public void Valid()
        {
            GuideAndTourReviewService guideAndTourReviewService = new GuideAndTourReviewService();
            if(currentReview.Valid)
            {
                currentReview.Valid = false;
                currentReview.ValidationUri = "Resources/Images/decline.jpg";
                guideAndTourReviewService.Update(currentReview);
                toastMessage.Visibility = Visibility.Visible;

            }
            else
            {
                currentReview.Valid = true;
                currentReview.ValidationUri ="Resources/Images/corect.png";
                guideAndTourReviewService.Update(currentReview);
                toastMessage.Visibility = Visibility.Visible;
            }
        }

        private void SetFirstImages(GuideAndTourReview review)
        {
            TourReviewImageService tourReviewImageService = new TourReviewImageService();
            images = tourReviewImageService.GetByReviewId(review.Id);
            Current = new BitmapImage(new Uri("/" + images[0].RelativeUri, UriKind.Relative));

        }

        public void GoBack()
        {
            currentCounter--;
            if (currentCounter < 0)
                currentCounter = images.Count - 1;
            Current = new BitmapImage(new Uri("/" + images[currentCounter].RelativeUri, UriKind.Relative));
        }

        public void GoForward()
        {
            currentCounter++;
            if (currentCounter >= images.Count)
                currentCounter = 0;
            Current = new BitmapImage(new Uri("/" + images[currentCounter].RelativeUri, UriKind.Relative));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
