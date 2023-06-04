using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.WPF.ViewModels.Guest2ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace InitialProject.Service
{
    public class TourInstanceService
    {
        private ITourInstanceRepository tourInstancerepository=Injector.CreateInstance<ITourInstanceRepository>();
        private List<TourInstance> finishedtourInstances;
        private List<TourInstance> finishedtourInsatncesForChosenYear;
        private TourService tourService;
        public TourInstanceService() 
        {
            finishedtourInsatncesForChosenYear = new List<TourInstance>();
            finishedtourInstances = new List<TourInstance>();
            tourService = new TourService();
        }
        public List<TourInstance> GetAll()
        {
            return tourInstancerepository.GetAll();
        }
        public void Delete(TourInstance tour)
        {
             tourInstancerepository.Delete(tour);   
        }
        public TourInstance Update(TourInstance tour)
        {
            return tourInstancerepository.Update(tour);
        }
        public TourInstance Save(TourInstance tour)
        {
            return tourInstancerepository.Save(tour);
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
            TourService tourService = new TourService();
            if (tourService.IsYearAvailable(year))
            {
                GetFinishedInstancesForChoosenYear(year, guide);
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
                if (tour != null)
                    tourInstanceTourLocation.FillTour(tour);
                return tour;
            }
            else
                return null;
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
        public List<TourInstance> SaveInstances(Tour savedTour, User loggedUser,ObservableCollection<TourInstance> FutureInstances,ObservableCollection<TourInstance> TodayInstances,ObservableCollection <TourInstance> Instances,List<TourImage> images)
        {
            GuideService guideService = new GuideService();
            TourImageService tourImageService = new TourImageService();
            List<TourInstance>saved= new List<TourInstance> ();
            foreach (TourInstance instance in Instances)
            {
                instance.Guide = guideService.GetByUsername(loggedUser.Username);
                instance.Tour = savedTour;
                instance.CoverImage = images[0].Url;
                instance.CoverBitmap = new BitmapImage(new Uri("/" + instance.CoverImage, UriKind.Relative));
                saved.Add(Save(instance));
                DisplayIfToday(instance,TodayInstances);
                DisplayIfCancelable(instance,FutureInstances);
            }
            return saved;
        }
        private void DisplayIfCancelable(TourInstance tour,ObservableCollection<TourInstance> FutureInstances)
        {
            if (tour.StartDate > DateTime.Now.Date)
            {
                var diffOfDates = DateTime.Now - tour.StartDate;

                if (diffOfDates.Days < -2)
                    FutureInstances.Add(tour);
                else if (diffOfDates.Days == -2 && diffOfDates.Hours < 0)
                    FutureInstances.Add(tour);
                else if (diffOfDates.Days == -2 && diffOfDates.Hours == 0 && diffOfDates.Minutes < 0)
                    FutureInstances.Add(tour);
            }
        }
        private void DisplayIfToday(TourInstance instance, ObservableCollection<TourInstance> TodayInstances)
        {
            if (instance.StartDate.Date == DateTime.Today.Date && instance.StartDate > DateTime.Now)
                TodayInstances.Add(instance);
        }
        public void SetLanguage(List<TourInstance> TourInstances)
        {
            
            foreach (TourInstance instance in TourInstances)
            {
                foreach (Tour tour in tourService.GetAll())
                {
                    if (tour.Id == instance.Tour.Id)
                    {
                        instance.Tour.Language = tour.Language;
                    }
                }
            }
        }

        public TourInstance GetById(int id)
        {
            return tourInstancerepository.GetById(id);
        }
    }
}
