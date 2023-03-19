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
        private Guest1Repository guest1Repository;
        private GuestReviewRepository guestReviewRepository;
        public ObservableCollection<GuestReview> Reviews { get; set; }

        public GuestReviewsOverview(Model.Owner owner)
        {
            InitializeComponent();
            DataContext = this;
            this.owner = owner;

            guest1Repository = new Guest1Repository();
            guestReviewRepository = new GuestReviewRepository();
            MakeReviewsForView();
        }

        private void MakeReviewsForView()
        {
            Reviews = new ObservableCollection<GuestReview>(GetAllByOwner());
            if(Reviews.Count > 0)
            {
                AddGuestsToReviews();
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddGuestsToReviews()
        {
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
            List<GuestReview> allReviews = new List<GuestReview>(guestReviewRepository.GetAll());
            SetReservationToReview(allReviews);
            List<GuestReview> reviewsByOwner = new List<GuestReview>();

            foreach(GuestReview review in allReviews)
            {
                if(review .Reservation != null && review.Reservation.Accommodation.Owner.Id == owner.Id)
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
                    if(review.Reservation != null)
                    {
                        Accommodation reservationAccommodation = accommodations.Find(n => n.Id == review.Reservation.Accommodation.Id);
                        if (reservationAccommodation != null)
                            review.Reservation.Accommodation = reservationAccommodation;
                    }
            }
            SetOwnerToReview(guestReviews);
        }

        private void SetReservationToReview(List<GuestReview> guestReviews)
        {
            AccommodationReservationRepository accommodationReservationRepository = new AccommodationReservationRepository();
            List<AccommodationReservation> accommodationReservations = accommodationReservationRepository.GetAll();

            foreach(GuestReview review in guestReviews)
            {
                if(review.Reservation != null)
                {
                    review.Reservation = accommodationReservations.Find(n => n.Id == review.Reservation.Id);
                }
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
                if (review.Reservation != null)
                {
                    Model.Owner accommodationOwner = owners.Find(n => n.Id == review.Reservation.Accommodation.Owner.Id);
                    if (accommodationOwner != null)
                        review.Reservation.Accommodation.Owner = accommodationOwner;
                }
            }
        }

        private void SetGuestToReview(List<GuestReview> guestReviews)
        {
            List<Guest1> guests = guest1Repository.GetAll();

            foreach(GuestReview review in guestReviews)
            {
                if (review.Reservation != null)
                {
                    Guest1 reservationGuest = guests.Find(n => n.Id == review.Reservation.Guest.Id);
                    if (reservationGuest != null)
                        review.Reservation.Guest = reservationGuest;
                }
            }
        }
    }
}
