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
    public class TourService
    {
        private ITourRepository tourRepository;
       // private TourInstanceService tourInstanceService = new TourInstanceService();
        private List<Tour> tours;
        private List<TourInstance> tourInstances;
        public TourService()
        {
            tourRepository = Injector.CreateInstance<ITourRepository>();
         //   tourInstances = tourInstanceService.GetAll();
            tours = GetAll();
        }

        public List<Tour> GetAll()
        {
            return tourRepository.GetAll();
        }
        public void SetTourToTourInstance(List<TourInstance> tourInstances)
        {
            foreach (TourInstance tourInstance in tourInstances)
            {
                foreach (Tour tour in tours)
                {
                    if (tour.Id == tourInstance.Tour.Id)
                    {
                        tourInstance.Tour = tour;
                    }
                }
            }
            SetLocationToTour(tourInstances);
        }
        public void SetLocationToTour(List<TourInstance> tourInstance)
        {
            LocationService locationService = new LocationService();
            foreach (Location location in locationService.GetAll())
            {
                foreach (TourInstance tour in tourInstance)
                {
                    if (location.Id == tour.Tour.Location.Id)
                        tour.Tour.Location = location;
                }
            }
           
        }
    }
}