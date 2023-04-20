using System.Collections.Generic;
using System.Windows.Documents;
using InitialProject.Domain;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;

namespace InitialProject.Service
{
    public class OwnerNotificationsService
    {
        private IOwnerNotificationRepository notificationRepository = Injector.CreateInstance<IOwnerNotificationRepository>();
        public OwnerNotificationsService()
        {
        }

        public void Add(OwnerNotificationType type)
        {
            notificationRepository.Add(type);
        }

        public void Delete(OwnerNotificationType type)
        {
            notificationRepository.Delete(type);
        }

        public bool IsAccommodationAdded()
        {
            List<OwnerNotification> notifications = notificationRepository.GetAll();
            return notifications.Find(n => n.Type.Equals(OwnerNotificationType.ACCOMMODATION_ADDED)) != null;
        }
        public bool IsAccommodationDeleted()
        {
            List<OwnerNotification> notifications = notificationRepository.GetAll();
            return notifications.Find(n => n.Type.Equals(OwnerNotificationType.ACCOMMODATION_DELETED)) != null;
        }
        public bool HasGuestToReview(Owner owner)
        {
            AccommodationReservationService accommodationReservationService = new AccommodationReservationService();
           return accommodationReservationService.GetAllForReviewByOwner(owner).Count != 0;
        }

        public bool HasReschedulingRequest(Owner owner)
        {
            RequestForReschedulingService requestService = new RequestForReschedulingService();
            return requestService.GetPendingRequests(owner).Count != 0;
        }
    }
}
