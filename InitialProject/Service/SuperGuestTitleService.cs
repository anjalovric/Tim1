using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Domain;

namespace InitialProject.Service
{
    public class SuperGuestTitleService
    {
        private List<SuperGuestTitle> superGuests;
        private ISuperGuestTitleRepository superGuestTitleRepository = Injector.CreateInstance<ISuperGuestTitleRepository>();
        public SuperGuestTitleService()
        {
            MakeSuperGuests();
        }
        private void MakeSuperGuests()
        {
            superGuests = new List<SuperGuestTitle>(superGuestTitleRepository.GetAll());
            SetGuests();
        }
        public List<SuperGuestTitle> GetAll()
        {
            return superGuests;
        }
        public void Add(SuperGuestTitle superGuestTitle)
        {
            superGuestTitleRepository.Add(superGuestTitle);
        }
        public bool IsAlreadySuperGuest(Guest1 guest1)
        {
             return superGuestTitleRepository.GetAll().Find(n => n.Guest.Id == guest1.Id)!=null;
        }

        public void SetGuests()
        {
            Guest1Service guest1Service = new Guest1Service();
            List<Guest1> allGuest = guest1Service.GetAll();
            foreach (SuperGuestTitle superGuest in superGuests)
            {
                Guest1 guest = allGuest.Find(n => n.Id == superGuest.Guest.Id);
                if (guest != null)
                    superGuest.Guest = guest;
            }
        }

        public SuperGuestTitle MakeSuperGuest(Guest1 guest1)
        {
            AccommodationReservationService accommodationReservationService = new AccommodationReservationService();
            int completedReservationsNumber = accommodationReservationService.GetReservationsNumberByGuestInLastYear(guest1);
            if(completedReservationsNumber >= 10 && !IsAlreadySuperGuest(guest1))
            {
                SuperGuestTitle newSuperGuestTitle = new SuperGuestTitle(guest1, 5, DateTime.Now);
                Add(newSuperGuestTitle);
                return newSuperGuestTitle;
            }
            return superGuestTitleRepository.GetAll().Find(n => n.Guest.Id == guest1.Id);
        }

        public bool HasSuperGuestTitleExpired(Guest1 guest1)
        {

            return true;
        }
    }
}
