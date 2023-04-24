using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.Service
{
    internal class OwnerService
    {
        private IOwnerRepository ownerRepository = Injector.CreateInstance<IOwnerRepository>();
        private List<Owner> owners;
        public OwnerService()
        {
            owners = ownerRepository.GetAll();
        }

        public List<Owner> GetAll()
        {
            return owners;
        }

        public Owner GetByUsername(String username)
        {
            return ownerRepository.GetByUsername(username);
        }

        public bool IsSuperOwner(Owner owner)
        {
            OwnerReviewService ownerReviewService = new OwnerReviewService();
            double averageRate = ownerReviewService.CalculateAverageRateByOwner(owner);
            int numberOfReviews = ownerReviewService.GetNumberOfReviewsByOwner(owner);
            bool isSuperOwner = averageRate >= 4.5 && numberOfReviews >= 2;
            MakeNewSuperOwnerNotification(owner, isSuperOwner);
            return isSuperOwner;
        }

        public Owner GetById(int id)
        {
            return ownerRepository.GetById(id);
        }

        private void MakeNewSuperOwnerNotification(Owner owner, bool isSuperOwner)
        {
            if(owner.IsSuperOwner != isSuperOwner)
            {
                owner.IsSuperOwner = isSuperOwner;
                ownerRepository.Update(owner);
                if(isSuperOwner)
                {
                    OwnerNotificationsService notificationsService = new OwnerNotificationsService();
                    notificationsService.Add(Domain.Model.OwnerNotificationType.SUPEROWNER, owner);
                }
            }
        }
    }
}
