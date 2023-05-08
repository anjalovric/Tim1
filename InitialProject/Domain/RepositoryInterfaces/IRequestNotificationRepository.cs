using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IRequestNotificationRepository:IGenericRepository<RequestNotification>

    {
        public RequestNotification Save(RequestNotification request);
        public RequestNotification Update(RequestNotification request);
    }
}
