using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using InitialProject.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public TourInstance Save(TourInstance tour)
        {
            return tourInstancerepository.Save(tour);
        }

        public void Delete(TourInstance tour)
        {
            tourInstancerepository.Delete(tour);
        }

        public TourInstance Update(TourInstance tour)
        {
            return tourInstancerepository.Update(tour);
        }
        public List<TourInstance> GetByStart(Guide guide)
        {
            List<TourInstance> list = tourInstancerepository.GetByStart(guide);

            FillWithTours(list);
            return list;

        }

        public List<TourInstance> GetInstancesLaterThan48hFromNow(Guide guide)
        {       
            return tourInstancerepository.GetInstancesLaterThan48hFromNow(guide);

        }

        public List<TourInstance> FindCancelableTours(Guide guide)
        {
            List<TourInstance> cancelableInstances = GetInstancesLaterThan48hFromNow(guide);
            TourService tourService = new TourService();
            tourService.SetLocationToTour(cancelableInstances);
            return cancelableInstances;
        }

        public void CancelTourInstance(TourInstance currentTourInstance, ObservableCollection<TourInstance> tourInstances, User tourInstanceGuide)
        {
            foreach (TourInstance tourInstance in GetAll())
            {
                if (tourInstance.Id == currentTourInstance.Id)
                {
                    currentTourInstance = tourInstance;
                }
            }
            currentTourInstance.Finished = true;
            Update(currentTourInstance);
            tourInstances.Remove(currentTourInstance);
            VoucherService voucherService = new VoucherService();
            voucherService.SendVoucher(currentTourInstance.Id, tourInstanceGuide);
        }

        public List<TourInstance> GetFinishedInsatnces(ObservableCollection<TourInstance> Instances)
        {
            foreach (TourInstance instance in GetAll())
            {
                if (instance.Finished)
                {
                    Instances.Add(instance);
                    finishedtourInstances.Add(instance);
                }
            }
            return finishedtourInstances;
        }

        public List<TourInstance> GetFinishedInstancesForChoosenYear(int year)
        {
            
            foreach (TourInstance instance in GetAll())
            {
                if (instance.Finished && instance.StartDate.Year == year)
                {
                    finishedtourInsatncesForChosenYear.Add(instance);
                }
            }
            return finishedtourInsatncesForChosenYear;
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

        public void SetFinishedInstances(ObservableCollection<TourInstance> Instances)
        {
            TourService tourService = new TourService();
            tourService.SetLocationToTour(GetFinishedInsatnces(Instances));
        }

        private void FillWithTours(List<TourInstance> Instances)
        {
            TourService tourService=new TourService();
            foreach(TourInstance instance in Instances)
            {
                foreach(Tour tour in tourService.GetAll())
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
        public TourInstance GetByActive(Guide guide)
        {
            return tourInstancerepository.GetActive(guide);
        }

        public void FillTour(TourInstance instance)
        {
            TourService tourService = new TourService();
            foreach(Tour tour in tourService.GetAll())
            {
                if(tour.Id== instance.Tour.Id)
                {
                    instance.Tour= tour;
                }
            }
            FillWithLocation(instance);
        }

        private void FillWithLocation(TourInstance instance)
        {
            LocationService locationService = new LocationService();
            foreach(Location location in locationService.GetAll())
            {
                if(location.Id== instance.Tour.Location.Id)
                {
                    instance.Tour.Location= location;
                }
            }
        }
    }
}
