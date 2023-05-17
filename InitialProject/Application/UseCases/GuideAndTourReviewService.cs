using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace InitialProject.Service
{
    public class GuideAndTourReviewService
    { 
        private LocationService locationService;
        private TourInstanceService tourInstanceService;
        private TourService tourService;
        private TourReservationService tourReservationService;
        private IGuideAndTourReviewsRepository guideAndTourReviewRepository = Injector.CreateInstance<IGuideAndTourReviewsRepository>();
        private ObservableCollection<TourReservation> tourReservations;
        private AlertGuest2Service alertGuest2Service;
        private ObservableCollection<AlertGuest2> alertGuest2List;
        public GuideAndTourReviewService()
        {
            locationService = new LocationService();
            tourInstanceService = new TourInstanceService();
            tourService = new TourService();
            tourReservationService = new TourReservationService();
            tourReservations = new ObservableCollection<TourReservation>(tourReservationService.GetAll());
            alertGuest2Service = new AlertGuest2Service();
            alertGuest2List = new ObservableCollection<AlertGuest2>(alertGuest2Service.GetAll());
        }
        public void SetTourInstances(ObservableCollection<TourInstance> CompletedTours,Guest2 guest2)
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
                            CompletedTours.Add(tourInstance);
                    }
                }
            }
            SetTours(CompletedTours);
        }
        public void SetTours(ObservableCollection<TourInstance> CompletedTours)
        {
            List<Tour> tours = tourService.GetAll();
            List<Location> locations = locationService.GetAll();
            foreach (TourInstance tourInstance in CompletedTours)
            {
                foreach (Location location in locations)
                {
                    foreach (Tour tour in tours)
                    {
                        if (location.Id == tour.Location.Id && tour.Id == tourInstance.Tour.Id)
                        {
                            tour.Location = location;
                            tourInstance.Tour = tour;
                        }
                    }
                }
            }
        }
        public List<GuideAndTourReview> GetReviewsByGuide(int guideId)
        {
            return guideAndTourReviewRepository.GetReviewsByGuide(guideId);
        }
        public List<GuideAndTourReview> FillWithGuests(List<GuideAndTourReview> guideAndTourReviews)
        {
            Guest2Service guest2Service = new Guest2Service();
            foreach(GuideAndTourReview review in guideAndTourReviews)
            {
                foreach(Guest2 guest2 in guest2Service.GetAll())
                {
                    if(guest2.Id==review.Guest2.Id)
                        review.Guest2 = guest2;
                }
            }
            return guideAndTourReviews;
        }
        public List<GuideAndTourReview> FillWithInstance(List<GuideAndTourReview> guideAndTourReviews)
        {
            TourInstanceService tourInstanceService = new TourInstanceService();
            foreach (GuideAndTourReview review in guideAndTourReviews)
            {
                foreach (TourInstance instance in tourInstanceService.GetAll())
                {
                    if (instance.Id == review.TourInstance.Id)
                        review.TourInstance = instance;
                }
            }
            return guideAndTourReviews;
        }
        public List<GuideAndTourReview> FillWithTour(List<GuideAndTourReview> guideAndTourReviews)
        {
            TourService tourService = new TourService();
            foreach (GuideAndTourReview review in guideAndTourReviews)
            {
                foreach (Tour tour in tourService.GetAll())
                {
                    if (tour.Id == review.TourInstance.Tour.Id)
                        review.TourInstance.Tour = tour;
                }
            }
            return guideAndTourReviews;
        }
        public List<GuideAndTourReview> FillWithLocation(List<GuideAndTourReview> guideAndTourReviews)
        {
            LocationService locationService = new LocationService();
            foreach (GuideAndTourReview review in guideAndTourReviews)
            {
                foreach (Location location in locationService.GetAll())
                {
                    if (location.Id == review.TourInstance.Tour.Location.Id)
                        review.TourInstance.Tour.Location = location;
                }
            }
            return guideAndTourReviews;
        }
        public GuideAndTourReview Update(GuideAndTourReview review)
        {
            return guideAndTourReviewRepository.Update(review);
        }
        public bool HasReview(TourInstance tourInstance,Guest2 guest2)
        {
            return guideAndTourReviewRepository.HasReview(tourInstance,guest2);
        }
        public GuideAndTourReview Save(GuideAndTourReview review)
        {
           return guideAndTourReviewRepository.Save(review);
        }
    }
}
