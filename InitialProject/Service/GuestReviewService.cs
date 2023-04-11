using System.Collections.Generic;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.Service
{
    public class GuestReviewService
    {
        private IGuestReviewRepository guestReviewRepository = Injector.CreateInstance<IGuestReviewRepository>();
        private List<GuestReview> guestReviews;

        public GuestReviewService()
        {
            guestReviews = new List<GuestReview>(guestReviewRepository.GetAll());
            MakeReservations();
        }

        public bool IsGuestReviewed(AccommodationReservation accommodationReservation)
        {
            return guestReviewRepository.HasReview(accommodationReservation);
        }

        private void MakeReservations()
        {
            AccommodationReservationService accommodationReservationService = new AccommodationReservationService();
            List<AccommodationReservation> accommodationReservations = new List<AccommodationReservation>();
            accommodationReservations = accommodationReservationService.GetAll();

            foreach (GuestReview review in guestReviews)
            {
                AccommodationReservation ownerReservation = accommodationReservations.Find(n => n.Id == review.Reservation.Id);
                if (ownerReservation != null)
                {
                    review.Reservation = ownerReservation;
                }
            }
        }

        public bool HasReview(AccommodationReservation reservation)
        {
            return guestReviewRepository.HasReview(reservation);
        }
        public List<GuestReview> GetAllByOwner(Owner owner)
        {
            List<GuestReview> reviewsByOwner = new List<GuestReview>();

            foreach (GuestReview review in guestReviews)
            {
                if (review.Reservation != null && review.Reservation.Accommodation.Owner.Id == owner.Id)
                    reviewsByOwner.Add(review);
            }
            return reviewsByOwner;
        }

        public void Save(GuestReview guestReview)
        {
            guestReviewRepository.Save(guestReview);
        }
    }
}
