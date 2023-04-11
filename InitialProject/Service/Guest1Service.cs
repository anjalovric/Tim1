using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.Service
{
    public class Guest1Service
    {
        private IGuest1Repository guest1Repository = Injector.CreateInstance<IGuest1Repository>();

        public Guest1Service()
        {
        }

        public List<Guest1> GetAll()
        {
            return guest1Repository.GetAll();
        }

        public Guest1 GetByUsername(String username)
        {
            Guest1 guest1 =  guest1Repository.GetByUsername(username);
            LocationService locationService = new LocationService();
            guest1.Location = locationService.GetAll().Find(n => n.Id == guest1.Location.Id);
            return guest1;
            
        }
    }

}
