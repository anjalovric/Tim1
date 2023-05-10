using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Domain;
using InitialProject.Repository;

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
        public SuperGuestTitle MakeNewSuperGuest(Guest1 guest1)
        {
            AccommodationReservationService accommodationReservationService = new AccommodationReservationService();
            DateTime activationDate = accommodationReservationService.GetNewSuperGuestActivationDateIfPossible(guest1);
            if(activationDate!=DateTime.MinValue && !IsAlreadySuperGuest(guest1))
            {
                SuperGuestTitle newSuperGuestTitle = new SuperGuestTitle(guest1, 5, activationDate);
                Add(newSuperGuestTitle);
                MakeSuperGuests(); 
                return newSuperGuestTitle;
            }
            MakeSuperGuests();
            return superGuestTitleRepository.GetAll().Find(n => n.Guest.Id == guest1.Id);
        }
        public void Delete(SuperGuestTitle title)
        {
            superGuestTitleRepository.Delete(title);
        }
        public SuperGuestTitle ProlongSuperGuestTitle(Guest1 guest1)
        {
            SuperGuestTitle superGuest = superGuests.Find(n => n.Guest.Id == guest1.Id);
            AccommodationReservationService accommodationReservationService=new AccommodationReservationService();
            DateTime newActivationDate = accommodationReservationService.GetProlongActivationDate(guest1, superGuest.ActivationDate);
            if (newActivationDate != DateTime.MinValue) //can prolog title
            {
                Delete(superGuest);     //delete previous and add new title
                Add(new SuperGuestTitle(guest1, 5, newActivationDate));
            } 
            else if(DateTime.Now > superGuest.ActivationDate.AddYears(1))  //1 year has passed
                Delete(superGuest);     //delete old title
            MakeSuperGuests();
            return superGuests.Find(n=>n.Guest.Id == guest1.Id);    //new superguest or null
        }        
        public void DeleteTitleIfManyYearsPassed(Guest1 guest1)
        {
            if(superGuests.Find(n => n.Guest.Id == guest1.Id)!=null)
                if (DateTime.Now > superGuests.Find(n => n.Guest.Id == guest1.Id).ActivationDate.AddYears(2))
                {
                    Delete(superGuests.Find(n => n.Guest.Id == guest1.Id));
                    MakeSuperGuests();
                }
        }
        public void DecrementPoints(Guest1 guest1)
        {
            SuperGuestTitle title = superGuests.Find(n => n.Guest.Id == guest1.Id);
            if (title!=null)
            {
                if(title.AvailablePoints>0)
                    title.AvailablePoints -= 1;
                superGuestTitleRepository.Update(title);
            }            
        }
    }
}
