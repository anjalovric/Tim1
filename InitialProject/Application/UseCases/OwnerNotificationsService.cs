using System.Collections.Generic;
using System.Windows.Documents;
using InitialProject.APPLICATION.UseCases;
using InitialProject.Domain;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;

namespace InitialProject.Service
{
    public class OwnerNotificationsService
    {
        private OwnerService ownerService;
        private LocationSuggestionsService suggestionsService;
        private NewForumNotificationService forumNotificationService;
        private IOwnerNotificationRepository notificationRepository = Injector.CreateInstance<IOwnerNotificationRepository>();
        public OwnerNotificationsService()
        {
            ownerService = new OwnerService();
            suggestionsService = new LocationSuggestionsService();
            forumNotificationService = new NewForumNotificationService();
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

        public bool IsRequestAccepted(Owner owner)
        {
            return notificationRepository.GetAll().Find(n => n.Type.Equals(OwnerNotificationType.REQUEST_ACCEPPTED) && n.Owner.Id == owner.Id) != null;
        }

        public bool IsRequestDeclined(Owner owner)
        {
            return notificationRepository.GetAll().Find(n => n.Type.Equals(OwnerNotificationType.REQUEST_DECLINED) && n.Owner.Id == owner.Id) != null;
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

        public void AddNewForumNotification(Location location)
        {
            List<Owner> owners = ownerService.GetAllByLocation(location);
            foreach(Owner owner in owners)
            {
                notificationRepository.Add(OwnerNotificationType.FORUM_ADDED, owner);
                forumNotificationService.Add(owner, location);
            }
        }

        public bool IsGuestReviewed(Owner owner)
        {
            return notificationRepository.GetAll().Find(n => n.Type.Equals(OwnerNotificationType.GUEST_REVIEWED) && n.Owner.Id == owner.Id) != null;
        }

        public bool HasLocationSuggestion(Owner owner)
        {
            return suggestionsService.GetMostPopularLocations(owner).Count !=0 || suggestionsService.GetLeastPopularAccommodations(owner).Count != 0;
        }

        public bool HasNewForum(Owner owner)
        {
            return notificationRepository.GetAll().Find(n => n.Owner.Id == owner.Id && n.Type.Equals(OwnerNotificationType.FORUM_ADDED)) != null;
        }

        public bool IsCommentAdded(Owner owner)
        {
            return notificationRepository.GetAll().Find(n => n.Owner.Id == owner.Id && n.Type.Equals(OwnerNotificationType.COMMENT_ADDED)) != null;
        }

        public bool IsCommentReported(Owner owner)
        {
            return notificationRepository.GetAll().Find(n => n.Owner.Id == owner.Id && n.Type.Equals(OwnerNotificationType.COMMENT_REPORTED)) != null;
        }

        public bool IsNewForumForOwner(Owner owner, Location location)
        {
            return forumNotificationService.GetByOwnerAndLocation(owner, location) != null;
        }
    }
}
