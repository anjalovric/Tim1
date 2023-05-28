using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class WinningAVoucher
    {
        private int Guest2Id;
        private List<AlertGuest2> AlertGuest2;
        private AlertGuest2Service alertGuestService;
        private List<TourInstance> TourInstances;
        private TourInstanceService tourInstanceService;
        private VoucherService voucherService;
        public WinningAVoucher(int guest2Id)
        {
            this.Guest2Id = guest2Id;
            alertGuestService = new AlertGuest2Service();
            AlertGuest2 = new List<AlertGuest2>(alertGuestService.GetAll());
            tourInstanceService = new TourInstanceService();
            TourInstances = new List<TourInstance>(tourInstanceService.GetAll());
            voucherService = new VoucherService();
        }
        public void CountOfTours()
        {
            List<TourInstance> tourInstances=new List<TourInstance>();
            int count = 0;
            foreach (AlertGuest2 alertGuest2 in AlertGuest2)
            {
                foreach(TourInstance tourInstance in TourInstances)
                {
                    if (alertGuest2.Guest2Id == Guest2Id && alertGuest2.Availability && alertGuest2.InstanceId == tourInstance.Id && tourInstance.Finished)
                    {
                        if (tourInstances.Count == 0)
                        {
                            tourInstances.Add(tourInstance);
                            count++;
                        }
                        else if (!tourInstances.Contains(tourInstance) && Check(tourInstances, tourInstance))
                        {
                            tourInstances.Add(tourInstance);
                            count++;
                        }
                    }
                }
            
            }
            if (count >= 5)
            {
                voucherService.SendVoucherForVisitedTours(Guest2Id);
            }
        }
        private Boolean Check(List<TourInstance> tourInstances, TourInstance tourInstance)
        {
            foreach(TourInstance instance in tourInstances)
            {
                if (instance.StartDate.Year == tourInstance.StartDate.Year)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
