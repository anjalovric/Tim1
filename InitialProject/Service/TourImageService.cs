using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class TourImageService
    {
        private ITourImageRepository repository = Injector.CreateInstance<ITourImageRepository>();
        public TourImageService() { }
        public List<TourImage> GetAll()
        {
            return repository.GetAll();
        }
        public TourImage Save(TourImage tourImage)
        {
            return repository.Save(tourImage);
        }
        public void Delete(TourImage tourImage)
        {
            repository.Delete(tourImage);
        }
        public TourImage Update(TourImage tourImage)
        {
            return repository.Update(tourImage);

        }
        public TourImage GetById(int id)
        {
            return repository.GetById(id);
        }
        public List<TourImage> GetByTour(int touId)
        {
            return repository.GetByTour(touId);
        }
    }

}

