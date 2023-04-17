using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.GuideViews;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace InitialProject.WPF.ViewModels
{
    public class TourInstanceReviewViewModel:INotifyPropertyChanged
    {
        private ObservableCollection<GuideAndTourReview> reviews;
        public ObservableCollection<GuideAndTourReview> Reviews
        {
            get => reviews;
            set
            {
                if (value != reviews)
                {
                    reviews = value;
                    OnPropertyChanged();
                }
            }
        }
        public GuideAndTourReview Selected { get; set; }
        private User user;
        private GuideAndTourReviewService guideAndTourReviewService;
        private GuideService guideService;

        public RelayCommand ViewReviewCommand { get; set; } 
       public TourInstanceReviewViewModel(User guide)
        {
            user= guide;
            guideAndTourReviewService = new GuideAndTourReviewService();
            guideService = new GuideService();
            GetGuideReviews(guide);
            ViewReviewCommand = new RelayCommand(ViewDetailsExecuted, CanExecute);
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
        private bool CanExecute(object sender)
        {
            return true;
        }
        public void ViewDetailsExecuted(object sender)
        {
                GuideAndTourReview review = Selected;
                ReviewDetailsView reviewDetailsView = new ReviewDetailsView(review, Reviews, user);
                Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = reviewDetailsView;

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
