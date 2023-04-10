using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAlertGuest2Repository:IGenericRepository<AlertGuest2>
    {
        public AlertGuest2 Save(AlertGuest2 alert);
        public AlertGuest2 Update(AlertGuest2 alert);
        public void Delete(AlertGuest2 alert);
        public List<AlertGuest2> GetByInstanceIdAndGuestId(int instanceId, int guestId);
        public void AddAlerts(int currentPointId, int _callId, TourInstance selected);

    }
}
