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
        private TourReservationService tourReservationService;
        private GuideAndTourReviewRepository guideAndTourReviewRepository;
        private ObservableCollection<TourReservation> tourReservations;
        private Guest2 guest2;
        public Location Location { get; set; }

        public GuideAndTourReviewService(Guest2 guest2)
        {
            guideAndTourReviewRepository = new GuideAndTourReviewRepository();

            locationService = new LocationService();
            tourInstanceService = new TourInstanceService();
            tourService = new TourService();
            tourReservationService = new TourReservationService();
            tourReservations = new ObservableCollection<TourReservation>(tourReservationService.GetAll());
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
            int exist = 0;
            foreach (TourReservation tourReservation in tourReservations)
            {
                foreach (TourInstance tourInstance in tourInstances)
                {
                    if (tourReservation.TourInstanceId == tourInstance.Id && tourReservation.GuestId == guest2.Id && tourInstance.Finished == true)
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
