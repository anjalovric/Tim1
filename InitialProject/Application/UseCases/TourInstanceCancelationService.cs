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
    internal class TourInstanceCancelationService
    {
        public List<TourInstance> FindCancelableTours(Guide guide)
        {
            TourInstanceService service = new TourInstanceService();
            List<TourInstance> cancelableInstances = service.GetInstancesLaterThan48hFromNow(guide);
            TourService tourService = new TourService();
            tourService.SetTourToTourInstance(cancelableInstances);
            return cancelableInstances;
        }

        public void CancelTourInstance(TourInstance currentTourInstance, ObservableCollection<TourInstance> tourInstances, User tourInstanceGuide)
        {
            GuideService guideService = new GuideService();
            Guide loggedGuide = guideService.GetByUsername(tourInstanceGuide.Username);
            SetToCancelState(currentTourInstance, tourInstances, loggedGuide);
            VoucherService voucherService = new VoucherService();
            voucherService.SendVoucher(currentTourInstance.Id, tourInstanceGuide);
        }
        private void SetToCancelState(TourInstance currentTourInstance, ObservableCollection<TourInstance> tourInstances, Guide loggedGuide)
        {
            TourInstanceService tourInstanceService = new TourInstanceService();
            foreach (TourInstance tourInstance in tourInstanceService.GetAll())
                if (tourInstance.Id == currentTourInstance.Id)
                    currentTourInstance = tourInstance;
            currentTourInstance.Canceled = true;
            tourInstanceService.Update(currentTourInstance);
            tourInstances.Clear();
            foreach (TourInstance tour in FindCancelableTours(loggedGuide))
                tourInstances.Add(tour);
        }
    }
}
