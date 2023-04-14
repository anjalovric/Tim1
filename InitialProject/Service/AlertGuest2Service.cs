using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{

    public class AlertGuest2Service
    {
        private IAlertGuest2Repository alertGuest2Repository=Injector.CreateInstance<IAlertGuest2Repository>();
        public AlertGuest2Service()  {}
        public List<AlertGuest2> GetAll()
        {
            return alertGuest2Repository.GetAll();
        }
        public AlertGuest2 Save(AlertGuest2 alert)
        {        
            return alertGuest2Repository.Save(alert);
        }
        public AlertGuest2 Update(AlertGuest2 alert)
        {
            return alertGuest2Repository.Update(alert);
        }
        public void Delete(AlertGuest2 alert)
        {
            alertGuest2Repository.Delete(alert);
        }
        public List<AlertGuest2> GetByInstanceIdAndGuestId(int instanceId, int guestId)
        {

            return alertGuest2Repository.GetByInstanceIdAndGuestId(instanceId,guestId);
        }
        public void AddAlerts(int currentPointId, int _callId, TourInstance selected)
        {
            alertGuest2Repository.AddAlerts(currentPointId, _callId, selected);
        }

        public int CountGuestsOnPoint(int currentPointId, TourInstance selectedInstance)
        {
            TourReservationService reservationService = new TourReservationService();
            int counter = 0;
            foreach (AlertGuest2 alert in GetAll())
                if (alert.CheckPointId == currentPointId && alert.Availability && alert.InstanceId == selectedInstance.Id)
                    counter += reservationService.GetTourReservationById(alert.ReservationId).Capacity;
            return counter;
        }

        public bool IsOnTour(int guest2Id, int instanceId)
        {
            foreach (AlertGuest2 alert in GetByInstanceIdAndGuestId(instanceId, guest2Id))
                if (alert.Availability)
                    return true;
            return false;
        }
    }
}
