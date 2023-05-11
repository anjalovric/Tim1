using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IReviewNotificationRepository:IGenericRepository<GuideAndTourReviewNotification>
    {
        public GuideAndTourReviewNotification Save(GuideAndTourReviewNotification review);
        public GuideAndTourReviewNotification Update(GuideAndTourReviewNotification voucher);
       
    }
}
