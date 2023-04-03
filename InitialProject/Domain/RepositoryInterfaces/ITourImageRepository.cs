using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ITourImageRepository:IGenericRepository<TourImage>
    {
        public TourImage Save(TourImage tourImage);
        public TourImage Delete(TourImage tourImage);

        public TourImage Update(TourImage tourImage);

    }
}
