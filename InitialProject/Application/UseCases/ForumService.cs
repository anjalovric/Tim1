using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain.Model;
using InitialProject.Domain;
using InitialProject.Model;
using InitialProject.Domain.RepositoryInterfaces;

namespace InitialProject.Service
{
    public class ForumService
    {
        private IForumRepository forumRepository = Injector.CreateInstance<IForumRepository>();
        LocationService locationService;
        Guest1Service guest1Service;
        public ForumService()
        {
            locationService = new LocationService();
            guest1Service = new Guest1Service();
        }
        /*private void SetGuests(List<Forum> forums)
        {
            List<Guest1> allGuest = guest1Service.GetAll();
            foreach (Forum forum in forums)
            {
                Guest1 guest = allGuest.Find(n => n.Id == forum.Guest1.Id);
                if (guest != null)
                    forum.Guest1 = guest;
            }
        }
        private void SetLocations(List<Forum> forums)
        {
            List<Location> allLocations = locationService.GetAll();
            foreach (Forum forum in forums)
            {
                Location location = allLocations.Find(n => n.Id == forum.Location.Id);
                if (location != null)
                    forum.Location = location;
            }
        }*/
        public List<Forum> GetAll()
        {
            return forumRepository.GetAll();
        }
        public void Add(Forum forum)
        {
            forumRepository.Add(forum);
        }

       public Forum GetById(int id)
       {
           return forumRepository.GetById(id);
       }


     
    
    }
}
