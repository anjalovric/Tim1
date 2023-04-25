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
            return tourInstances;
        }
        public List<TourInstance> TourInstances;
        private TourInstanceService tourInstanceService;

        public List<TourInstance> ScheduledInstances(TourInstance newinstance,DateTime StartDate,DateTime EndDate,double duration)
        {
            List <TourInstance> instances = new List<TourInstance>();
 
            TourInstance actual = newinstance;
            DateTime newdays=actual.StartDate.AddHours(duration);
            if (newdays < EndDate)
            {
                foreach (TourInstance instance in GetInstancesStartingLaterThanStartDate(newinstance.StartDate))
                {
                    TourInstance copied = instance;
                    DateTime newDate = copied.StartDate.AddHours(instance.Tour.Duration);
                    if (newDate <= copied.StartDate)
                        instances.Add(copied);
                }
            }
            else
                instances.Add(newinstance);
            return instances;
        }
    }
}
