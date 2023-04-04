using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class CancelTourService
    {

        private LocationService locationService;
        private TourInstanceService tourInstanceService;
        private TourService tourService;
        private VoucherService voucherService;
        private GuideService guideService;
        private TourReservationService tourReservationService;
        //private Guest2 guest;
        public CancelTourService() 
        {
            locationService = new LocationService();
            tourInstanceService = new TourInstanceService();
            tourService = new TourService();
            voucherService = new VoucherService();
            guideService = new GuideService();
            tourReservationService = new TourReservationService();
        }

        public void SetLocationToTour()
        {
            List<Location> locations = locationService.GetAll();
            List<Tour> tours = tourService.GetAll();
            foreach (Location location in locations)
            {
                foreach (Tour tour in tours)
                {
                    if (location.Id == tour.Location.Id)
                        tour.Location = location;
                }
            }
        }

        public void SetTourToTourInstance()
        {
            List<TourInstance> tourInstances = tourInstanceService.GetAll();
            List<Tour> tours = tourService.GetAll();
            foreach (TourInstance tourInstance in tourInstances)
            {
                foreach (Tour tour in tours)
                {
                    if (tour.Id == tourInstance.Tour.Id)
                    {
                        tourInstance.Tour = tour;
                    }
                }
            }
        }
        public void SendVoucher(int tourInstanceId,User tourInstanceGuide)
        {
            foreach (TourReservation reservation in tourReservationService.GetReservationsForTourInstance(tourInstanceId))
            {
                Voucher voucher = new Voucher();
                voucher.Used = false;
                voucher.GuestId = reservation.GuestId;
                voucher.GuideId = guideService.GetByUsername(tourInstanceGuide.Username).Id;
                voucher.CreateDate = DateTime.Now;
                Voucher savedVoucher = voucherService.Save(voucher);

            }

        }

        public List<TourInstance> FindCancelableTours()
        {
            return tourInstanceService.GetInstancesLaterThan48hFromNow();
        }


        public void CancelTourInstance(TourInstance currentTourInstance, ObservableCollection<TourInstance> tourInstances,User tourInstanceGuide)
        {
            foreach (TourInstance tourInstance in tourInstanceService.GetAll())
            {
                if (tourInstance.Id == currentTourInstance.Id)
                {
                    currentTourInstance = tourInstance;
                }
            }
            currentTourInstance.Finished = true;
            tourInstanceService.Update(currentTourInstance);
            tourInstances.Remove(currentTourInstance);
            SendVoucher(currentTourInstance.Id,tourInstanceGuide);
        }
    }
}
