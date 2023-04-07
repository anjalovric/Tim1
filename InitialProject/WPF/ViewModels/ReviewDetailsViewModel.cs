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

        public ObservableCollection<string> Points { get; set; }
        public ReviewDetailsViewModel(GuideAndTourReview review) 
        { 
            SetTourDetails(review);
            SetGrades(review);
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

    }
}
