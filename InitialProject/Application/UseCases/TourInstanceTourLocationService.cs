using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class TourInstanceTourLocationService
    {
        public void FillWithTours(List<TourInstance> Instances)
        {
            TourService tourService = new TourService();
            foreach (TourInstance instance in Instances)
            {
                foreach (Tour tour in tourService.GetAll())
                {
                    if (tour.Id == instance.Tour.Id)
                    {
                        instance.Tour = tour;
                    }
                }
            }
            FillWithLocation(Instances);
        }
        private void FillWithLocation(List<TourInstance> Instances)
        {
            LocationService locationService = new LocationService();
            foreach (TourInstance instance in Instances)
            {
                foreach (Location location in locationService.GetAll())
                {
                    if (location.Id == instance.Tour.Location.Id)
                    {
                        instance.Tour.Location = location;
                    }
                }
            }
        }
        public void FillTour(TourInstance instance)
        {
            TourService tourService = new TourService();
            foreach (Tour tour in tourService.GetAll())
            {
                if (tour.Id == instance.Tour.Id)
                {
                    instance.Tour = tour;
                }
            }
            FillLocation(instance);
        }
        private void FillLocation(TourInstance instance)
        {
            LocationService locationService = new LocationService();
            foreach (Location location in locationService.GetAll())
            {
                if (location.Id == instance.Tour.Location.Id)
                {
                    instance.Tour.Location = location;
                }
            }
        }
    }
}
