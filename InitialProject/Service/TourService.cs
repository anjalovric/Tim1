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
        private ITourRepository tourRepository = Injector.CreateInstance<ITourRepository>();
        private TourInstanceService tourInstanceService = new TourInstanceService();
        private List<Tour> tours;
        private List<TourInstance> tourInstances;
        public TourService()
        {
            tourInstances = tourInstanceService.GetAll();
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
        }
        public void SetLocationToTour(List<TourInstance> tourInstance)
        {
            LocationService locationService = new LocationService();
            foreach (Location location in locationService.GetAll())
            {
                foreach (Tour tour in tours)
                {
                    if (location.Id == tour.Location.Id)
                        tour.Location = location;
                }
            }
            SetTourToTourInstance(tourInstance);
        }
    }
}