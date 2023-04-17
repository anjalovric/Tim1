using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
        public ObservableCollection<GuideAndTourReview> Reviews { get; set; }

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
        public RelayCommand GoBackCommand { get; set; }
        public RelayCommand GoForwardCommand { get;set; }
        public RelayCommand ValidCommand { get; set; }

        private string toastVisibility;
        public string ToastVisibility
        {
            get => toastVisibility;
            set
            {
                if (!value.Equals(toastVisibility))
                {
                    toastVisibility = value;
                    OnPropertyChanged();
                }
            }
        }
        private User loggedUser;
        public ReviewDetailsViewModel(GuideAndTourReview review,ObservableCollection<GuideAndTourReview> reviews,User user) 
        { 
            loggedUser = user;
            SetTourDetails(review);
            SetGrades(review);
            currentReview= review;
            Reviews = reviews;
            ToastVisibility = "Hidden";
            MakeCommands();
        }
        private void MakeCommands()
        {
            GoBackCommand = new RelayCommand(GoBackExecuted, CanExecute);
            GoForwardCommand=new RelayCommand(GoForwardExecuted, CanExecute);
            ValidCommand=new RelayCommand(ValidExecuted,CanExecute);
        }
        private bool CanExecute(object sender)
        {
            return true;
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
            CheckPointService checkPointService = new CheckPointService();
            Points= new ObservableCollection<string>(checkPointService.GetPointsForGuest(review.Guest2.Id,review.TourInstance));
        }
        public void ValidExecuted(object sender)
        {
            if(currentReview.Valid)
                ChangeValidationState(false, "Resources/Images/redIncorect.png");
            else
                ChangeValidationState(true, "Resources/Images/greenCorect.png");
        }
        private void ChangeValidationState(bool state,string url)
        {
                GuideAndTourReviewService guideAndTourReviewService = new GuideAndTourReviewService();
                currentReview.Valid = state;
                currentReview.ValidationUri = url;
                guideAndTourReviewService.Update(currentReview);
                ReplaceReview();
                ToastVisibility = "Visible";
        }
        private void ReplaceReview()
        {
            Reviews.Clear();
            GuideService guideService= new GuideService();
            Guide loggedGuide = guideService.GetByUsername(loggedUser.Username);
            GuideAndTourReviewService guideAndTourReviewService = new GuideAndTourReviewService();
            List<GuideAndTourReview> reviews = guideAndTourReviewService.GetReviewsByGuide(loggedGuide.Id);
            List<GuideAndTourReview> filledGuets = guideAndTourReviewService.FillWithGuests(reviews);
            List<GuideAndTourReview> filledInstances = guideAndTourReviewService.FillWithInstance(filledGuets);
            List<GuideAndTourReview> filledTours = guideAndTourReviewService.FillWithTour(filledInstances);
            foreach (GuideAndTourReview review in guideAndTourReviewService.FillWithLocation(filledTours)) 
                Reviews.Add(review);
        }
        private void SetFirstImages(GuideAndTourReview review)
        {
            TourReviewImageService tourReviewImageService = new TourReviewImageService();
            images = tourReviewImageService.GetByReviewId(review.Id);
            Current = new BitmapImage(new Uri("/" + images[0].RelativeUri, UriKind.Relative));
        }
        public void GoBackExecuted(object sender)
        {
            currentCounter--;
            if (currentCounter < 0)
                currentCounter = images.Count - 1;
            Current = new BitmapImage(new Uri("/" + images[currentCounter].RelativeUri, UriKind.Relative));
        }
        public void GoForwardExecuted(object sender)
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
