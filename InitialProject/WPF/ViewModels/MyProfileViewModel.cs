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

namespace InitialProject.WPF.ViewModels
{
    public class MyProfileViewModel : INotifyPropertyChanged
    {
        public Model.Owner ProfileOwner { get; set; }
        private OwnerReviewService ownerReviewService;
        private OwnerReview selectedOwnerReview;
        public ObservableCollection<OwnerReview> OwnerReviews { get; set; }
        private double averageRate;
        private int numberOfRates;
        private string title;
        private string starVisibility;

        public event PropertyChangedEventHandler? PropertyChanged;

        public MyProfileViewModel(User user)
        {
            ownerReviewService = new OwnerReviewService();
            MakeOwner(user);
            selectedOwnerReview = new OwnerReview();
            OwnerReviews = new ObservableCollection<OwnerReview>(ownerReviewService.GetAllToDisplay(ProfileOwner));
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
                if (!value.Equals(selectedOwnerReview))
                {
                    selectedOwnerReview = value;
                    OnPropertyChanged();
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void MakeOwner(User user)
        {
            ProfileOwner = new Model.Owner();
            OwnerService ownerService = new OwnerService();
            ProfileOwner = ownerService.GetByUsername(user.Username);
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
