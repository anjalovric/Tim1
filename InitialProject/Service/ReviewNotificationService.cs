using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Serializer;
using InitialProject.Model;

namespace InitialProject.Service
{
    public class ReviewNotificationService
    {
        private IReviewNotificationRepository reviewNotificationRepository = Injector.CreateInstance<IReviewNotificationRepository>();

        public ReviewNotificationService()
        {
        }
            public List<ReviewNotification> GetAll()
            {
                return reviewNotificationRepository.GetAll();
            }

            public ReviewNotification Save(ReviewNotification review)
            {
                return reviewNotificationRepository.Save(review);
            }

            public ReviewNotification GetById(int id)
            {
                return reviewNotificationRepository.GetById(id);
            }
            public ReviewNotification Update(ReviewNotification voucher)
            {
           
                return reviewNotificationRepository.Update(voucher);
            }
             public int GetNewReviewCount(User user)
            {
                GuideService guideService = new GuideService();
                int guideId=guideService.GetByUsername(user.Username).Id;
                int count = 0;
                foreach(ReviewNotification reviewNotification in GetAll())
                    if(reviewNotification.GuideId == guideId && reviewNotification.Count==0)
                        count++;
                return count;
            }
            public void UpCount(User user)
            {
                GuideService guideService = new GuideService();
                int guideId = guideService.GetByUsername(user.Username).Id;
                int count = 0;
                foreach (ReviewNotification reviewNotification in GetAll())
                {
                    if(reviewNotification.GuideId == guideId)
                        reviewNotification.Count++;
                        Update(reviewNotification);
                }
            }

    }
}
