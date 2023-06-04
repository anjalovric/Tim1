using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest1Views;
using InitialProject.Domain.Model;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class Guest1ProfileViewModel : INotifyPropertyChanged
    {
        public bool IsSuperGuest {get;set;}
        public SuperGuestTitle SuperGuest { get; set; }
        public Guest1 Guest1 { get; set; }
        private BitmapImage imageSource;
        public BitmapImage ImageSource
        {
            get { return imageSource; }
            set
            {
                if (value != imageSource)
                    imageSource = value;
                OnPropertyChanged("ImageSource");
            }

        }
        public int AverageRating { get; set; }
        public int ReviewsNumber { get; set; }
        private SuperGuestTitleService superGuestTitleService;
        public Guest1ProfileViewModel(Guest1 guest1)
        {
            this.Guest1 = guest1;
            Initialize();
        }

        private void Initialize()
        {
            string relative = FindImageRelativePath();
            ImageSource = new BitmapImage(new Uri(relative, UriKind.Relative));
            superGuestTitleService = new SuperGuestTitleService();
            GetAverageRating();
            GetReviewsNumber();
            ShowSuperGuest();
        }

        private void ShowSuperGuest()
        {
            superGuestTitleService.DeleteTitleIfNeeded(Guest1);
            if (superGuestTitleService.IsAlreadySuperGuest(Guest1))
            {
                SuperGuest = superGuestTitleService.ProlongSuperGuestTitle(Guest1);  //add new or delete previous title.
            }
            SuperGuest = superGuestTitleService.MakeNewSuperGuest(Guest1);
            IsSuperGuest = superGuestTitleService.IsAlreadySuperGuest(Guest1);
        }
       
        private void GetReviewsNumber()
        {
            GuestAverageReviewService guestAverageReviewService = new GuestAverageReviewService();
            ReviewsNumber = guestAverageReviewService.GetReviewsNumberByGuest(Guest1);
        }
        private void GetAverageRating()
        {
            GuestAverageReviewService guestAverageReviewService = new GuestAverageReviewService();
            AverageRating = guestAverageReviewService.GetAverageRating(Guest1);
        }
        private string FindImageRelativePath()
        {
            return Guest1.ImagePath;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
