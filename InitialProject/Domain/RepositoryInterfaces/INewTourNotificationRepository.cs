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
    public interface INewTourNotificationRepository : IGenericRepository<NewTourNotification>
    {
        public NewTourNotification Save(NewTourNotification notification);
        public void Delete(NewTourNotification notification);
        public List<NewTourNotification> GetByGuestId(int id);
        public void Update(NewTourNotification notification);
    }
}
