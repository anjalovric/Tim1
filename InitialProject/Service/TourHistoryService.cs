using InitialProject.Model;
using InitialProject.Repository;
using Org.BouncyCastle.Asn1.Sec;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class TourHistoryService
    {
        private TourService tourService;
        private TourInstanceService tourInstanceService;
        private LocationService locationService;
        private TourDetailsService tourDetailsService;
        private TourImageService tourImageService;

        private List<TourInstance> finishedtourInstances;
        private List<TourInstance> finishedtourInsatncesForChosenYear;

        public TourHistoryService() 
        {
            tourService = new TourService();
            tourInstanceService = new TourInstanceService();
            locationService = new LocationService();
            tourDetailsService = new TourDetailsService();
            tourImageService = new TourImageService();

            finishedtourInstances = new List<TourInstance>();
            finishedtourInsatncesForChosenYear=new List<TourInstance>();
        }

        public void SetFinishedInstances(ObservableCollection<TourInstance> Instances)
        {
            SetLocationToTour();
            SetTourToTourInstance();
            GetFinishedInsatnces(Instances);
        }
        public void GetFinishedInsatnces(ObservableCollection<TourInstance> Instances)
        {   
            foreach (TourInstance instance in tourInstanceService.GetAll())
            {
                if (instance.Finished)
                {
                    Instances.Add(instance);
                    finishedtourInstances.Add(instance);
                }
            }
        }

        public void GetFinishedInstancesForChoosenYear(int year)
        {
            foreach (TourInstance instance in tourInstanceService.GetAll())
            {
                if (instance.Finished && instance.StartDate.Year==year)
                {
                    finishedtourInsatncesForChosenYear.Add(instance);
                }
            }
        }
        public void SetLocationToTour()
        {

            foreach (Tour tour in tourService.GetAll())
            {
                foreach (Location location in locationService.GetAll())
                {
                    if (location.Id == tour.Location.Id)
                        tour.Location = location;
                }
            }
        }

        public void SetTourToTourInstance()
        {

            foreach (TourInstance instance in tourInstanceService.GetAll())
            {
                foreach (Tour tour in tourService.GetAll())
                {
                    if (tour.Id == instance.Tour.Id)
                        instance.Tour = tour;
                }
            }
        }

        public void SetAttendanceToFinishTours()
        {
            foreach(TourInstance tour in finishedtourInstances)
            {
                tour.Attendance = tourDetailsService.MakeAttendancePrecentage(tour.Id);
            }
            foreach (TourInstance tour in finishedtourInsatncesForChosenYear)
            {
                tour.Attendance = tourDetailsService.MakeAttendancePrecentage(tour.Id);
            }
        }

        public TourInstance FindMostVisited()
        {
            SetAttendanceToFinishTours();
            double minimum = 0;
            TourInstance tour = null;
            foreach(TourInstance instance in finishedtourInstances)
            {
                if (instance.Attendance >= minimum)
                {
                    minimum = instance.Attendance;
                    tour = instance;
                }
            }
            return tour;
        }

        public TourInstance FindMostVisitedForChosenYear(int year)
        {
            GetFinishedInstancesForChoosenYear(year);
            SetAttendanceToFinishTours();
            double maximum = 0;
            TourInstance tour = null;
            foreach (TourInstance instance in finishedtourInsatncesForChosenYear)
            {
                if (instance.Attendance >= maximum)
                {
                    maximum = instance.Attendance;
                    tour = instance;
                }
            }
            return tour;
        }

        public string FindFirstImageOfTour(int tourId)
        {
            List<TourImage> images= tourImageService.GetByTour(tourId);
            return images[0].Url;
        }

    }
}
