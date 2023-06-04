using InitialProject.Model;
using System;
using System.Collections.Generic;

namespace InitialProject.Service
{
    public class AvailableDatesForTour
    {
        private TourInstanceService tourInstanceService;
        private TourService tourService;
        public AvailableDatesForTour() 
        {
            tourInstanceService=new TourInstanceService();
            tourService=new TourService();
        }
        public List<TourInstance> ScheduleTourInstances(TourInstance newinstance,DateTime StartDate,DateTime EndDate,double duration, int guideId)
        {
            List <TourInstance> overlayTourInstances = new List<TourInstance>();
            TourInstance newlTourInstance = newinstance;
            if (newlTourInstance.StartDate.AddHours(duration) <= EndDate)
            {
                List<TourInstance> tours = tourInstanceService.GetAll();
                foreach (TourInstance tourInstance in SetTours(tours))
                {
                    if (tourInstance.Finished == false && tourInstance.Canceled == false && tourInstance.Guide.Id==guideId)
                    {
                        TourInstance temporaryTourInstance = tourInstance;
                        if ((temporaryTourInstance.StartDate.AddHours(tourInstance.Tour.Duration) >= newlTourInstance.StartDate.AddHours(duration)) && !(tourInstance.StartDate >= newlTourInstance.StartDate.AddHours(duration)) && !(newinstance.StartDate > temporaryTourInstance.StartDate.AddHours(tourInstance.Tour.Duration)))
                        {
                            AddTourInstanceToOverlayList(overlayTourInstances, newinstance);
                            return overlayTourInstances;
                        }
                        else if (!(tourInstance.StartDate >= newlTourInstance.StartDate.AddHours(duration)) && !(newinstance.StartDate > temporaryTourInstance.StartDate.AddHours(tourInstance.Tour.Duration)) && (newinstance.StartDate < temporaryTourInstance.StartDate.AddHours(tourInstance.Tour.Duration)))
                            AddTourInstanceToOverlayList(overlayTourInstances, newinstance);
                    }
                }
            }
            else
                AddTourInstanceToOverlayList(overlayTourInstances, newinstance);
            return overlayTourInstances;
        }
        private void AddTourInstanceToOverlayList(List<TourInstance> overlayTourInstances,TourInstance tourInstance)
        {
            overlayTourInstances.Add(tourInstance); 
        }
        private List<TourInstance> SetTours(List<TourInstance> requests) 
        {
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
