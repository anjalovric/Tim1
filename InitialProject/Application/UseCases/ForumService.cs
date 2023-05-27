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
using InitialProject.WPF.ViewModels.OwnerViewModels;
using InitialProject.APPLICATION.UseCases;

namespace InitialProject.Service
{
    public class ForumService
    {
        private IForumRepository forumRepository = Injector.CreateInstance<IForumRepository>();
        LocationService locationService;
        Guest1Service guest1Service;
        ForumCommentService forumCommentService;
        OwnerService ownerService;
        public ForumService()
        {
            locationService = new LocationService();
            guest1Service = new Guest1Service();
            forumCommentService = new ForumCommentService();
            ownerService = new OwnerService();
        }
        private void SetGuests(List<Forum> forums)
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
        }
        public List<Forum> GetAll()
        {
            List<Forum> forums = forumRepository.GetAll();
            SetLocations(forums);
            SetGuests(forums);
            return forums;
        }
        public void Add(Forum forum)
        {
            forum.IsNewForOwner = true;
            forumRepository.Add(forum);
        }

       public Forum GetById(int id)
       {
           return forumRepository.GetById(id);
       }

        public void Close(Forum forum)
        {
            forum.Opened = false;
            forumRepository.Update(forum);
        }
        public Forum Open(Forum forum)
        {
            forum.Opened = true;
            forumRepository.Update(forum);
            return forum;
        }
        public void IncrementCommentsNumber(Forum forum)
        {
            forum.CommentsNumber += 1;
            forumRepository.Update(forum);
        }

        public bool ExistsOnLocation(string locationCountry, string locationCity)
        {
            Location location = locationService.GetByCityAndCountry(locationCountry, locationCity);
            return forumRepository.GetAll().Find(n => n.Location.Id == location.Id) != null; 
        }
        

        public Forum GetByLocation(string locationCountry, string locationCity)
        {
            Location location = locationService.GetByCityAndCountry(locationCountry, locationCity);
            return forumRepository.GetAll().Find(n => n.Location.Id == location.Id);
        }
     
        public List<OneForumViewModel> getAllForOwnerDisplay(Owner owner)
        {
            List<Forum> forums = GetAll();
            List<OneForumViewModel> result = new List<OneForumViewModel>();
            foreach(Forum forum in forums)
            {
                OneForumViewModel forumViewModel = new OneForumViewModel();
                forumViewModel.Forum = forum;
                forumViewModel.GuestComments = forumCommentService.GetNumberOfGuestComments(forum);
                forumViewModel.OwnerComments = forumCommentService.GetNumberOfOwnerComments(forum);
                forumViewModel.OwnerHasLocation = ownerService.HasAccommodationOnLocation(owner, forum.Location);
                result.Add(forumViewModel);
            }
            return result;
        }
    
        public List<OneForumViewModel> getNewForOwnerDisplay(Owner owner)
        {
            return getAllForOwnerDisplay(owner).FindAll(n => n.Forum.IsNewForOwner == true);
        }

        public void MakeForumsOld(List<OneForumViewModel> forumViewModels)
        {
            foreach(var forum in forumViewModels)
            {
                forum.Forum.IsNewForOwner = false;
                forumRepository.Update(forum.Forum);
            }
        }
    }
}
