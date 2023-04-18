using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace InitialProject.Service
{
    public class TourInstanceService
    {
        private ITourInstanceRepository tourInstancerepository=Injector.CreateInstance<ITourInstanceRepository>();
        private List<TourInstance> finishedtourInstances;
        private List<TourInstance> finishedtourInsatncesForChosenYear;
        public TourInstanceService() 
        {
            finishedtourInsatncesForChosenYear = new List<TourInstance>();
            finishedtourInstances = new List<TourInstance>();
        }
        public List<TourInstance> GetAll()
        {
            return tourInstancerepository.GetAll();
        }
        public TourInstance Update(TourInstance tour)
        {
            return tourInstancerepository.Update(tour);
        }
        public List<TourInstance> GetByStart(Guide guide)
        {
            List<TourInstance> list = tourInstancerepository.GetByStart(guide);
            TourInstanceTourLocationService tourInstanceTourLocation = new TourInstanceTourLocationService();
            tourInstanceTourLocation.FillWithTours(list);
            return list;
        }
        public List<TourInstance> GetInstancesLaterThan48hFromNow(Guide guide)
        {       
            return tourInstancerepository.GetInstancesLaterThan48hFromNow(guide);
        }
        public List<TourInstance> GetFinishedInstances(ObservableCollection<TourInstance> Instances,Guide guide)
        {
            foreach (TourInstance instance in GetAll())
            {
                if (instance.Finished && instance.Guide.Id==guide.Id)
                {
                    Instances.Add(instance);
                    finishedtourInstances.Add(instance);
                }
            }
            return finishedtourInstances;
        }
        public List<TourInstance> GetFinishedInstancesForChoosenYear(int year, Guide guide)
        {
            foreach (TourInstance instance in GetAll())
            { 
                if (instance.Finished && instance.StartDate.Year == year && instance.Guide.Id == guide.Id)
                    finishedtourInsatncesForChosenYear.Add(instance);
            }
            return finishedtourInsatncesForChosenYear;
        }
        public TourInstance FindMostVisitedForChosenYear(int year, Guide guide )
        {
            GetFinishedInstancesForChoosenYear(year,guide);
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
            TourInstanceTourLocationService tourInstanceTourLocation = new TourInstanceTourLocationService();
            if(tour!=null)
                tourInstanceTourLocation.FillTour(tour);
            return tour;
        }
        public void SetAttendanceToFinishTours()
        {
            TourDetailsService tourDetailsService = new TourDetailsService();
            foreach (TourInstance tour in finishedtourInstances)
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
            foreach (TourInstance instance in finishedtourInstances)
            {
                if (instance.Attendance >= minimum)
                {
                    minimum = instance.Attendance;
                    tour = instance;
                }
            }
            return tour;
        }
        public void SetFinishedInstances(ObservableCollection<TourInstance> Instances,Guide guide)
        {
            TourService tourService = new TourService();
            tourService.SetTourToTourInstance(GetFinishedInstances(Instances,guide));
        }
        public TourInstance GetByActive(Guide guide)
        {
            return tourInstancerepository.GetActive(guide);
        }
        public TourInstance SetFinishStatus(TourInstance selected)
        {
            TourDetailsService tourDetailsService = new TourDetailsService();
            selected.Finished = true;
            selected.Active = false;
            selected.Attendance = tourDetailsService.MakeAttendancePrecentage(selected.Id);
            tourInstancerepository.Update(selected);
            TourInstanceTourLocationService tourInstanceTourLocation = new TourInstanceTourLocationService();
            tourInstanceTourLocation.FillTour(selected);
            return selected;
        }
        public void ActivateTour(TourInstance selected)
        {
            tourInstancerepository.ActivateTour(selected);
        }
    }
}
