using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
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

        public TourInstanceService() 
        {
            tourInstancerepository = new TourInstanceRepository();
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
        public List<TourInstance> GetByStart()
        {
            List<TourInstance> list = new List<TourInstance>();
            foreach (TourInstance tour in tourInstancerepository.GetAll())

                if (tour.StartDate.Date == DateTime.Now.Date && tour.Finished == false)
                {
                    string h = tour.StartClock.Split(':')[0];
                    string m = tour.StartClock.Split(":")[1];
                    string s = tour.StartClock.Split(":")[2];


                    if (Convert.ToInt32(h) > DateTime.Now.Hour)
                    {
                        list.Add(tour);
                    }
                    else if (Convert.ToInt32(h) == DateTime.Now.Hour && Convert.ToInt32(m) > DateTime.Now.Minute)
                    {
                        list.Add(tour);
                    }
                    else if (Convert.ToInt32(h) == DateTime.Now.Hour && Convert.ToInt32(m) == DateTime.Now.Minute && Convert.ToInt32(s) > DateTime.Now.Second)
                    {
                        list.Add(tour);
                    }
                }

            return list;

        }



        public List<TourInstance> GetInstancesLaterThan48hFromNow()
        {
            List<TourInstance> list = new List<TourInstance>();
            foreach (TourInstance tour in tourInstancerepository.GetAll())
            {
                if (tour.Finished == false && tour.StartDate > DateTime.Now.Date)
                {
                    var prevDate = Convert.ToDateTime(tour.StartDate.ToString().Split(" ")[0] + " " + tour.StartClock);
                    var today = DateTime.Now;
                    var diffOfDates = today - prevDate;

                    if (diffOfDates.Days < -2)
                        list.Add(tour);
                    else if (diffOfDates.Days == -2 && diffOfDates.Hours < 0)
                        list.Add(tour);
                    else if (diffOfDates.Days == -2 && diffOfDates.Hours == 0 && diffOfDates.Minutes < 0)
                        list.Add(tour);
                }

            }
            return list;

        }

        public List<TourInstance> FindCancelableTours()
        {
            List<TourInstance> cancelableInstances = GetInstancesLaterThan48hFromNow();
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
    }
}
