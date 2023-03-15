using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for GuestReviewsOverview.xaml
    /// </summary>
    public partial class GuestReviewsOverview : Window
    {
        private Model.Owner owner;
        public ObservableCollection<GuestReview> Reviews { get; set; }
        public GuestReviewsOverview(Model.Owner owner)
        {
            InitializeComponent();
            DataContext = this;
            this.owner = owner;
            GuestReviewRepository guestReviewRepository = new GuestReviewRepository();
            Reviews = new ObservableCollection<GuestReview>(guestReviewRepository.GetAllByOwnerId(owner.Id));
            AddGuestsToReviews();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddGuestsToReviews()
        {
            Guest1Repository guest1Repository = new Guest1Repository();
            Guest1 guest = new Guest1();
            foreach (GuestReview review in Reviews)
            {
                guest = guest1Repository.GetAll().Find(n => n.Id == review.Guest.Id);
                if(guest != null)
                    review.Guest = guest;
            }
        }

    }
}
