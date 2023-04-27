using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Model;

namespace InitialProject.Service
{
    public class DeletingAccommodationService
    {
        private Accommodation accommodationToDelete;
        public DeletingAccommodationService()
        {
            accommodationToDelete = new Accommodation();
        }

        public void Delete(Accommodation accommodation)
        {
            accommodationToDelete = accommodation;
            DeleteImages();
            DeleteOwnerReviews();
            DeleteGuestReviews();
            DeleteRequestForRescheduling();
            DeleteReservations();
            DeleteCancelledReservations();
            DeleteRenovationSuggestions();
            DeleteRenovations();
            DeleteAccommodation();
        }

        private void DeleteImages()
        {
            AccommodationImageService imageService = new AccommodationImageService();
            foreach (var image in imageService.GetAllByAccommodation(accommodationToDelete))
            {
                imageService.Delete(image);
            }
        }

        private void DeleteOwnerReviews()
        {
            OwnerReviewService ownerReviewService = new OwnerReviewService();
            foreach (var review in ownerReviewService.GetAll().FindAll(n => n.Reservation.Accommodation.Id == accommodationToDelete.Id))
            {
                ownerReviewService.Delete(review);
            }
        }

        private void DeleteGuestReviews()
        {
            GuestReviewService guestReviewService = new GuestReviewService();
            foreach (var review in guestReviewService.GetAll().FindAll(n => n.Reservation.Accommodation.Id == accommodationToDelete.Id))
            {
                guestReviewService.Delete(review);
            }
        }

        private void DeleteRequestForRescheduling()
        {
            ReschedulingAccommodationRequestService requestService = new ReschedulingAccommodationRequestService();
            foreach (var request in requestService.GetAll().FindAll(n => n.Reservation.Accommodation.Id == accommodationToDelete.Id))
            {
                DeleteCompletedRequests(request);
                requestService.Delete(request);
            }
        }
        private void DeleteReservations()
        {
            AccommodationReservationService reservationService = new AccommodationReservationService();
            foreach (var reservation in reservationService.GetAll().FindAll(n => n.Accommodation.Id == accommodationToDelete.Id))
            {
                reservationService.Delete(reservation);
            }
        }

        private void DeleteCompletedRequests(ReschedulingAccommodationRequest request)
        {
            CompletedAccommodationReschedulingRequestService requestService = new CompletedAccommodationReschedulingRequestService();
            CompletedAccommodationReschedulingRequest completedRequest = requestService.GetAll().Find(n => n.Request.Id == request.Id);
            if (completedRequest != null)
                requestService.Delete(completedRequest);
        }

        private void DeleteCancelledReservations()
        {
            CancelledAccommodationReservationService reservationService = new CancelledAccommodationReservationService();
            foreach(var reservation in reservationService.GetAll().FindAll(n => n.Accommodation.Id == accommodationToDelete.Id))
            {
                reservationService.Delete(reservation);
            }
        }
        private void DeleteRenovations()
        {
            AccommodationRenovationService renovationService = new AccommodationRenovationService();
            foreach(var renovation in renovationService.GetAllByOwner(accommodationToDelete.Owner).FindAll(n => n.Accommodation.Id == accommodationToDelete.Id))
            {
                renovationService.Delete(renovation);
            }
        }

        private void DeleteRenovationSuggestions()
        {
            AccommodationRenovationSuggestionService renovationService = new AccommodationRenovationSuggestionService();
            foreach (var renovation in renovationService.GetAll().FindAll(n => n.Reservation.Accommodation.Id == accommodationToDelete.Id))
            {
                renovationService.Delete(renovation);
            }
        }

        private void DeleteAccommodation()
        {
            AccommodationService accommodationService = new AccommodationService();
            accommodationService.Delete(accommodationToDelete);
        }
    }
}
