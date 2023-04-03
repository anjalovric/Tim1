using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAlertGuest2Repository:IGenericRepository<AlertGuest2>
    {
        AlertGuest2 Save(AlertGuest2 alert);
        AlertGuest2 Update(AlertGuest2 alert);
        AlertGuest2 Delete(AlertGuest2 alert);
        public List<AlertGuest2> GetByInstanceIdAndGuestId(int instanceId, int guestId);

    }
}
