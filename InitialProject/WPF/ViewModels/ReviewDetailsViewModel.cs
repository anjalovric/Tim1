using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.GuideViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace InitialProject.WPF.ViewModels
{
    public class ReviewDetailsViewModel
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
    }
}
