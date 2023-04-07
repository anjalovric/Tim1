using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class GuideAndTourReviewService
    {
        public ObservableCollection<TourInstance> CompletedTours { get; set; }
        private LocationService locationService;
        private TourInstanceService tourInstanceService;
        private TourService tourService;
        private GuideAndTourReviewRepository guideAndTourReviewRepository;
        private AlertGuest2Service alertGuest2Service;
        private Guest2 guest2;
        private ObservableCollection<AlertGuest2> alertGuest2List;
        public Location Location { get; set; }
        public GuideAndTourReviewService(Guest2 guest2)
        {
            guideAndTourReviewRepository = new GuideAndTourReviewRepository();
            locationService = new LocationService();
            tourInstanceService = new TourInstanceService();
            tourService = new TourService();
            alertGuest2Service = new AlertGuest2Service();
            alertGuest2List = new ObservableCollection<AlertGuest2>(alertGuest2Service.GetAll());
            this.guest2 = guest2;
            CompletedTours = new ObservableCollection<TourInstance>();
            SetTourInstances(CompletedTours);
            Location = new Location();
            SetLocations();
            SetTours(CompletedTours);
            
        }
        private void SetTourInstances(ObservableCollection<TourInstance> CompletedTours)
        {
            List<TourInstance> tourInstances;
            tourInstances = tourInstanceService.GetAll();
            foreach (AlertGuest2 alertGuest2 in alertGuest2List)
            {
                foreach (TourInstance tourInstance in tourInstances)
                {
                    if (alertGuest2.InstanceId == tourInstance.Id && alertGuest2.Guest2Id == guest2.Id && tourInstance.Finished == true && alertGuest2.Availability==true)
                    {
                        if (!CompletedTours.Contains(tourInstance))
                        {
                            CompletedTours.Add(tourInstance);
                        }
                    }
                }
            }
        }
        public void SetLocations()
        {
            List<Location> locations = locationService.GetAll();
            List<Tour> tours = tourService.GetAll();

            foreach (Location location in locations)
            {
                foreach (Tour tour in tours)
                {
                    if (location.Id == tour.Location.Id)
                        tour.Location = location;
                }
            }
        }
        public void SetTours(ObservableCollection<TourInstance> CompletedTours)
        {
            List<Tour> tours = tourService.GetAll();
            foreach (TourInstance tourInstance in CompletedTours)
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

    }
}
