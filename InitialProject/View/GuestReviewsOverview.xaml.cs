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
            Reviews = new ObservableCollection<GuestReview>(GetAllByOwner());
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
                guest = guest1Repository.GetAll().Find(n => n.Id == review.Reservation.Guest.Id);
                if(guest != null)
                    review.Reservation.Guest = guest;
            }
        }

        private List<GuestReview> GetAllByOwner()
        {
            GuestReviewRepository guestReviewRepository = new GuestReviewRepository();
            List<GuestReview> allReviews = new List<GuestReview>(guestReviewRepository.GetAll());
            SetReservationToReview(allReviews);
            List<GuestReview> reviewsByOwner = new List<GuestReview>();
            foreach(GuestReview review in allReviews)
            {
                if(review.Reservation.Accommodation.Owner.Id == owner.Id)
                    reviewsByOwner.Add(review);
            }
            return reviewsByOwner;
        }

        private void SetAccommodationToReview(List<GuestReview> guestReviews)
        {
            AccommodationRepository accommodationRepository = new AccommodationRepository();
            List<Accommodation> accommodations = accommodationRepository.GetAll();
            foreach(GuestReview review in guestReviews)
            {
                review.Reservation.Accommodation = accommodations.Find(n => n.Id == review.Reservation.Accommodation.Id);
            }
            SetOwnerToReview(guestReviews);
        }

        private void SetReservationToReview(List<GuestReview> guestReviews)
        {
            AccommodationReservationRepository accommodationReservationRepository = new AccommodationReservationRepository();
            List<AccommodationReservation> accommodationReservations = accommodationReservationRepository.GetAll();
            foreach(GuestReview review in guestReviews)
            {
                review.Reservation = accommodationReservations.Find(n => n.Id == review.Reservation.Id);
            }
            SetAccommodationToReview(guestReviews);
            SetGuestToReview(guestReviews);
        }

        private void SetOwnerToReview(List<GuestReview> guestReviews)
        {
            OwnerRepository ownerRepository = new OwnerRepository();
            List<Model.Owner> owners = ownerRepository.GetAll();
            foreach(GuestReview review in guestReviews)
            {
                review.Reservation.Accommodation.Owner = owners.Find(n => n.Id == review.Reservation.Accommodation.Owner.Id);
            }
        }

        private void SetGuestToReview(List<GuestReview> guestReviews)
        {
            Guest1Repository guest1Repository = new Guest1Repository();
            List<Guest1> guests = guest1Repository.GetAll();
            foreach(GuestReview review in guestReviews)
            {
                review.Reservation.Guest = guests.Find(n => n.Id == review.Reservation.Guest.Id);
            }
        }
    }
}
