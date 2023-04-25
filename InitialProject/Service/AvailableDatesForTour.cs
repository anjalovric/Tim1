using InitialProject.Domain.Model;
using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class AvailableDatesForTour
    {
        public AvailableDatesForTour() 
        {
            tourInstanceService = new TourInstanceService();
            TourInstances= new List<TourInstance>();
        }

        public List<TourInstance> GetInstancesStartingLaterThanStartDate(DateTime StartDate)
        {
            List<TourInstance> tourInstances = new List<TourInstance>();
            foreach(TourInstance instance in tourInstanceService.GetAll())
            {
                if(instance.StartDate >= StartDate)
                    tourInstances.Add(instance);
            }
            SetTours(tourInstances);
            return tourInstances;
        }
        public List<TourInstance> TourInstances;
        private TourInstanceService tourInstanceService;

        public List<TourInstance> ScheduledInstances(TourInstance newinstance,DateTime StartDate,DateTime EndDate,double duration)
        {
            List <TourInstance> instances = new List<TourInstance>();
            TourInstanceService tourInstanceService= new TourInstanceService();
            TourInstance actual = newinstance;
            DateTime newdays=actual.StartDate.AddHours(duration);
            if (newdays < EndDate)
            {
                foreach (TourInstance instance in tourInstanceService.GetAll())
                {
                    TourInstance copied = instance;
                    DateTime newDate = copied.StartDate.AddHours(instance.Tour.Duration);
                    if (newDate <= newdays && !(instance.StartDate>= newdays) && !(newinstance.StartDate>newDate))
                        instances.Add(copied);
                }
            }
            else
                instances.Add(newinstance);
            return instances;
        }
        private void SetTours(List<TourInstance> requests) 
        {
            TourService tourService = new TourService();
            foreach(TourInstance instance in requests)
            {
                foreach(Tour tour in tourService.GetAll())
                    if(tour.Id==instance.Tour.Id)
                        instance.Tour = tour;
            }
        }
    }
}
