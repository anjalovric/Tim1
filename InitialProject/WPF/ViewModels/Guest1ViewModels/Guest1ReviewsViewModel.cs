using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using InitialProject.Model;
using InitialProject.Service;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class Guest1ReviewsViewModel :INotifyPropertyChanged
    {
        private Guest1 guest1;
        GuestReviewService guestReviewService;
        public double AverageCleanliness { get; set; }
        public int ReviewsNumber { get; set; }
        public double AverageFollowingRules { get; set; }
        public int AverageRating { get; set; }

        private ObservableCollection<GuestReview> guest1Reviews;
        public ObservableCollection<GuestReview> Guest1Reviews
        {
            get { return guest1Reviews; }
            set
            {
                if (value != guest1Reviews)
                    guest1Reviews = value;
                OnPropertyChanged("Ruest1Reviews");
            }

        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Guest1ReviewsViewModel(Guest1 guest1)
        {
            this.guest1 = guest1;
            guestReviewService = new GuestReviewService();
            Guest1Reviews = new ObservableCollection<GuestReview>(guestReviewService.GetAllToDisplay(guest1));
            AverageCleanliness = guestReviewService.GetAverageCleanlinessReview(guest1);
            AverageFollowingRules = guestReviewService.GetAverageFollowingRulesReview(guest1);
            ReviewsNumber = guestReviewService.GetReviewsNumberByGuest(guest1);
            AverageRating = guestReviewService.GetAverageRating(guest1);
            
        }
        
    }
}
