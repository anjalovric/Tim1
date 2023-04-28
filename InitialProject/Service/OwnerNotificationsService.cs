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

        public void Add(OwnerNotificationType type, Owner owner)
        {
            notificationRepository.Add(type, owner);
        }

        public void Delete(OwnerNotificationType type, Owner owner)
        {
            notificationRepository.Delete(type, owner);
        }

        public bool IsAccommodationAdded(Owner owner)
        {
            List<OwnerNotification> notifications = notificationRepository.GetAll();
            return notifications.Find(n => n.Type.Equals(OwnerNotificationType.ACCOMMODATION_ADDED) && n.Owner.Id == owner.Id) != null;
        }
        public bool IsAccommodationDeleted(Owner owner)
        {
            List<OwnerNotification> notifications = notificationRepository.GetAll();
            return notifications.Find(n => n.Type.Equals(OwnerNotificationType.ACCOMMODATION_DELETED) && n.Owner.Id == owner.Id) != null;
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

        public bool IsNewSuperOwner(Owner owner)
        {
            OwnerNotification notification = notificationRepository.GetAll().Find(n => n.Type == OwnerNotificationType.SUPEROWNER && n.Owner.Id == owner.Id);
            return notification != null;
        }

        public bool IsRenovationScheduled(Owner owner)
        {
            List<OwnerNotification> notifications = notificationRepository.GetAll();
            return notifications.Find(n => n.Owner.Id == owner.Id && n.Type.Equals(OwnerNotificationType.RENOVATION_SCHEDULED)) != null;
        }

        public bool IsRenovationCancelled(Owner owner)
        {
            List<OwnerNotification> notifications = notificationRepository.GetAll();
            return notifications.Find(n => n.Owner.Id == owner.Id && n.Type.Equals(OwnerNotificationType.RENOVATION_CANCELLED)) != null;
        }
    }
}
