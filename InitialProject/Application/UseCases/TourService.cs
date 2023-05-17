using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using System.Collections.Generic;

namespace InitialProject.Service
{
    public class TourService
    {
        private ITourRepository tourRepository;
        public TourService()
        {
            tourRepository = Injector.CreateInstance<ITourRepository>();
        }
        public Tour Save(Tour tour)
        {          
            return tourRepository.Save(tour);
        }
        public List<Tour> GetAll()
        {
            return tourRepository.GetAll();
        }
        public void SetTourToTourInstance(List<TourInstance> tourInstances)
        {
            foreach (TourInstance tourInstance in tourInstances)
            {
                foreach (Tour tour in GetAll())
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
        public bool IsYearAvailable(int year)
        {
            TourInstanceService tourInstanceService = new TourInstanceService();
            if (tourInstanceService.GetAll().Count > 0)
                foreach (TourInstance tourInstance in tourInstanceService.GetAll())
                    if (tourInstance.StartDate.Year == year)
                        return true;
            return false;               
        }
        public void SetTour(TourInstance tourInstance)
        {
            foreach(Tour tour in GetAll())
                if(tour.Id == tourInstance.Tour.Id)
                    tourInstance.Tour=tour;
        }
    }
}