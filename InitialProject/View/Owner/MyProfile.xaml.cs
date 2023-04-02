using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using InitialProject.Model;
using InitialProject.Service;

namespace InitialProject.View.Owner
{
    /// <summary>
    /// Interaction logic for MyProfile.xaml
    /// </summary>
    public partial class MyProfile : Page
    {
        public Model.Owner ProfileOwner { get; set; }
        private OwnerReviewService ownerReviewService;
        public ObservableCollection<OwnerReview> OwnerReviews { get; set; }
        public MyProfile(User user)
        {
            InitializeComponent();
            DataContext = this;
            MakeOwner(user);
            ownerReviewService = new OwnerReviewService();
            OwnerReviews = new ObservableCollection<OwnerReview>(ownerReviewService.GetAllToDisplay(ProfileOwner));
        }

        private void MakeOwner(User user)
        {
            ProfileOwner = new Model.Owner();
            OwnerService ownerService = new OwnerService();
            ProfileOwner = ownerService.GetByUsername(user.Username);
        }
    }
}
