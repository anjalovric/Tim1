using InitialProject.Model;
using System;
using System.Collections.Generic;

namespace InitialProject.Service
{
    public class AvailableDatesForTour
    {
        public AvailableDatesForTour() 
        {
        }

        public List<TourInstance> ScheduledInstances(TourInstance newinstance,DateTime StartDate,DateTime EndDate,double duration)
        {
            List <TourInstance> instances = new List<TourInstance>();
            TourInstanceService tourInstanceService= new TourInstanceService();
            TourInstance actual = newinstance;
            DateTime newdays=actual.StartDate.AddHours(duration);
            if (newdays <= EndDate)
            {
                List<TourInstance> tours = tourInstanceService.GetAll();
                foreach (TourInstance instance in SetTours(tours))
                {
                    if (instance.Finished == false && instance.Canceled == false)
                    {
                        TourInstance copied = instance;
                        DateTime newDate = copied.StartDate.AddHours(instance.Tour.Duration);
                        if ((newDate >= newdays) && !(instance.StartDate >= newdays) && !(newinstance.StartDate > newDate))
                        {
                            instances.Add(copied);
                            return instances;
                        }
                        else if(!(instance.StartDate >= newdays) && !(newinstance.StartDate > newDate) && (newinstance.StartDate<newDate))
                            instances.Add(copied);
                    }
                }
            }
            else
                instances.Add(newinstance);
            return instances;
        }
        private List<TourInstance> SetTours(List<TourInstance> requests) 
        {
            TourService tourService = new TourService();
            foreach(TourInstance instance in requests)
            {
                foreach(Tour tour in tourService.GetAll())
                    if(tour.Id==instance.Tour.Id)
                        instance.Tour = tour;
            }
            return requests;
        }
    }
}
