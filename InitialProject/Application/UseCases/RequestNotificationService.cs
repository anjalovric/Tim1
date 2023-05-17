using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain;
using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class RequestNotificationService
    {
        private IRequestNotificationRepository reguestNotificationRepository = Injector.CreateInstance<IRequestNotificationRepository>();

        public RequestNotificationService()
        {
        }
        public List<OrdinaryRequestNotification> GetAll()
        {
            return reguestNotificationRepository.GetAll();
        }

        public OrdinaryRequestNotification Save(OrdinaryRequestNotification request)
        {
            return reguestNotificationRepository.Save(request);
        }

        public OrdinaryRequestNotification GetById(int id)
        {
            return reguestNotificationRepository.GetById(id);
        }
        public OrdinaryRequestNotification Update(OrdinaryRequestNotification request)
        {

            return reguestNotificationRepository.Update(request);
        }
        public int GetNewRequestCount()
        {
            int count = 0;
            foreach (OrdinaryRequestNotification requestNotification in GetAll())
                if (requestNotification.Count == 0)
                    count++;
            return count;
        }
        public void UpCount()
        {
            int count = 0;
            foreach (OrdinaryRequestNotification requestNotification in GetAll())
            {
                requestNotification.Count++;
                Update(requestNotification);
            }
        }
    }
}
