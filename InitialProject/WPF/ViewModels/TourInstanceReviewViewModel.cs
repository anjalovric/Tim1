using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.GuideViews;
using MathNet.Numerics;
using NPOI.POIFS.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Window = System.Windows.Window;

namespace InitialProject.WPF.ViewModels
{
    public class TourInstanceReviewViewModel
    {
       public ObservableCollection<GuideAndTourReview> Reviews { get; set; }  
       public GuideAndTourReview Selected { get; set; }

        private GuideAndTourReviewService guideAndTourReviewService;
        private GuideService guideService;

       public TourInstanceReviewViewModel(User guide)
        {
            guideAndTourReviewService = new GuideAndTourReviewService();
            guideService = new GuideService();
            GetGuideReviews(guide);


        }
        private void GetGuideReviews(User guide)
        {
            Guide loggedGuide = guideService.GetByUsername(guide.Username);
            List<GuideAndTourReview> reviews = guideAndTourReviewService.GetReviewsByGuide(loggedGuide.Id);
            List<GuideAndTourReview> filledGuets= guideAndTourReviewService.FillWithGuests(reviews);
            List<GuideAndTourReview> filledInstances = guideAndTourReviewService.FillWithInstance(filledGuets);
            List<GuideAndTourReview> filledTours = guideAndTourReviewService.FillWithTour(filledInstances);
            Reviews = new ObservableCollection<GuideAndTourReview>(guideAndTourReviewService.FillWithLocation(filledTours));

        }

        public void ViewDetails()
        {
            GuideAndTourReview review = Selected;
            ReviewDetailsView reviewDetailsView = new ReviewDetailsView(review);
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = reviewDetailsView;
        }
    }
}
