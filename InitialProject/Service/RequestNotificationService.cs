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
        private IRequestNotificationRepository reviewNotificationRepository = Injector.CreateInstance<IRequestNotificationRepository>();

        public RequestNotificationService()
        {
        }
        public List<RequestNotification> GetAll()
        {
            return reviewNotificationRepository.GetAll();
        }

        public RequestNotification Save(RequestNotification review)
        {
            return reviewNotificationRepository.Save(review);
        }

        public RequestNotification GetById(int id)
        {
            return reviewNotificationRepository.GetById(id);
        }
        public RequestNotification Update(RequestNotification request)
        {

            return reviewNotificationRepository.Update(request);
        }
        public int GetNewRequestCount()
        {
            int count = 0;
            foreach (RequestNotification reviewNotification in GetAll())
                if ( reviewNotification.Count == 0)
                    count++;
            return count;
        }
        public void UpCount()
        {
            int count = 0;
            foreach (RequestNotification reviewNotification in GetAll())
            {
                    reviewNotification.Count++;
                Update(reviewNotification);
            }
        }
    }
}
