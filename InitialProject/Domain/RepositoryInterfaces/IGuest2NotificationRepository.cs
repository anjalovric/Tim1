using InitialProject.Domain.Model;
using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IGuest2NotificationRepository : IGenericRepository<Guest2Notification>
    {
        public Guest2Notification Save(Guest2Notification notification);
        public void Delete(Guest2Notification notification);
        public ObservableCollection<Guest2Notification> GetByGuestId(int id);
        public void Update(Guest2Notification notification);
    }
}
