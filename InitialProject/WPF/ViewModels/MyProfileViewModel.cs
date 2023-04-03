using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using InitialProject.Model;
using InitialProject.Service;

namespace InitialProject.WPF.ViewModels
{
    public class MyProfileViewModel : INotifyPropertyChanged
    {
        public Model.Owner ProfileOwner { get; set; }
        private OwnerReviewService ownerReviewService;
        public ObservableCollection<OwnerReview> OwnerReviews { get; set; }
        private double averageRate;
        private int numberOfRates;

        public event PropertyChangedEventHandler? PropertyChanged;

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
        public MyProfileViewModel(User user)
        {
            ownerReviewService = new OwnerReviewService();
            MakeOwner(user);
            OwnerReviews = new ObservableCollection<OwnerReview>(ownerReviewService.GetAllToDisplay(ProfileOwner));
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

            CalculateAverageRate();
            GetNumberOfRates();
        }

        private void CalculateAverageRate()
        {
            AverageRate = ownerReviewService.CalculateAverageRateByOwner(ProfileOwner);
        }

        private void GetNumberOfRates()
        {
            NumberOfRates = ownerReviewService.GetNumberOfReviewsByOwner(ProfileOwner);
        }
    }

}
