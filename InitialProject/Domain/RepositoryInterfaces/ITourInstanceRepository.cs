using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ITourInstanceRepository:IGenericRepository<TourInstance>
    {
        public List<TourInstance> GetByStart(Guide guide);

        public List<TourInstance> GetInstancesLaterThan48hFromNow(Guide guide);


        public TourInstance Save(TourInstance tour);


        public void Delete(TourInstance tour);

        public TourInstance Update(TourInstance tour);

        public TourInstance GetActive(Guide guide);

        public void ActivateTour(TourInstance selected);

    }
}
