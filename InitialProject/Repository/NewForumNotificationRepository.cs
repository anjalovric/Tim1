using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Repository
{
    public class NewForumNotificationRepository : INewForumNotificationRepository
    {

        private const string FilePath = "../../../Resources/Data/newForumNotifications.csv";

        private readonly Serializer<NewForumNotification> _serializer;

        private List<NewForumNotification> _forumNotifications;

        public NewForumNotificationRepository()
        {
            _serializer = new Serializer<NewForumNotification>();
            _forumNotifications = _serializer.FromCSV(FilePath);
        }

        public void Add(NewForumNotification notification)
        {
            _forumNotifications.Add(notification);
            _serializer.ToCSV(FilePath, _forumNotifications);
        }

        public NewForumNotification GetByOwnerAndForum(Owner owner, Location location)
        {
            return _forumNotifications.Find(c => c.Owner.Id == owner.Id && c.Location.Id == location.Id);
        }
        public void Delete(NewForumNotification notification)
        {
            _forumNotifications = _serializer.FromCSV(FilePath);
            NewForumNotification founded = _forumNotifications.Find(c => c.Owner.Id == notification.Owner.Id && c.Location.Id == notification.Location.Id);
            _forumNotifications.Remove(founded);
            _serializer.ToCSV(FilePath, _forumNotifications);
        }

        public List<NewForumNotification> GetAll()
        {
            return _forumNotifications;
        }

        public NewForumNotification GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int NextId()
        {
            throw new NotImplementedException();
        }
    }
}
