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
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class MyProfileViewModel : INotifyPropertyChanged
    {
        public Owner ProfileOwner { get; set; }
        private OwnerReviewService ownerReviewService;
        private OwnerReview selectedOwnerReview;
        public ObservableCollection<OwnerReview> OwnerReviews { get; set; }
        private double averageRate;
        private int numberOfRates;
        private string title;
        private string starVisibility;
        public RelayCommand ViewCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public MyProfileViewModel(Owner owner)
        {
            ownerReviewService = new OwnerReviewService();
            MakeOwner(owner);
            OwnerReviews = new ObservableCollection<OwnerReview>(ownerReviewService.GetAllToDisplay(ProfileOwner));
            InitializeSelectedOwnerReview();
            ViewCommand = new RelayCommand(View_Executed, CanExecute);
        }

        private bool CanExecute(object sender)
        {
            return true;
        }

        private void View_Executed(object sender)
        {
            if (SelectedOwnerReview != null)
            {
                OwnerReviewView ownerReviewView = new OwnerReviewView(SelectedOwnerReview);
                Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = ownerReviewView;
            }
        }
        public double AverageRate
        {
            get => averageRate;
            set
            {
                if (value != averageRate)
                {
                    averageRate = value;
                    OnPropertyChanged();
                }
            }
        }
        public int NumberOfRates
        {
            get => numberOfRates;
            set
            {
                if (value != numberOfRates)
                {
                    numberOfRates = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Title
        {
            get => title;
            set
            {
                if (!value.Equals(title))
                {
                    title = value;
                    OnPropertyChanged();
                }
            }
        }
        public string StarVisibility
        {
            get => starVisibility;
            set
            {
                if (!value.Equals(starVisibility))
                {
                    starVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        public OwnerReview SelectedOwnerReview
        {
            get => selectedOwnerReview;
            set
            {
                if (value != selectedOwnerReview)
                {
                    selectedOwnerReview = value;
                    OnPropertyChanged();
                }
            }
        }

        private void InitializeSelectedOwnerReview()
        {
            selectedOwnerReview = new OwnerReview();
            if (OwnerReviews.Count > 0)
            {
                SelectedOwnerReview = OwnerReviews[0];
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void MakeOwner(Owner owner)
        {
            ProfileOwner = owner;
            OwnerService ownerService = new OwnerService();
            ProfileOwner.IsSuperOwner = ownerService.IsSuperOwner(ProfileOwner);

            CalculateAverageRate();
            GetNumberOfRates();
            DisplayTitle();
        }

        private void CalculateAverageRate()
        {
            AverageRate = ownerReviewService.CalculateAverageRateByOwner(ProfileOwner);
        }

        private void GetNumberOfRates()
        {
            NumberOfRates = ownerReviewService.GetNumberOfReviewsByOwner(ProfileOwner);
        }

        private void DisplayTitle()
        {
            if (ProfileOwner.IsSuperOwner)
            {
                Title = "Super Owner!";
                StarVisibility = "Visible";
            }
            else
            {
                Title = "Owner";
                StarVisibility = "Hidden";
            }
        }

    }

}
