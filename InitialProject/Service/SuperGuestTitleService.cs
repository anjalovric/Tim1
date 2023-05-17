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
        private ISuperGuestTitleRepository superGuestTitleRepository = Injector.CreateInstance<ISuperGuestTitleRepository>();
        private Guest1Service guest1Service;
        private AccommodationReservationService accommodationReservationService;
        public SuperGuestTitleService()
        {
            guest1Service = new Guest1Service();
            accommodationReservationService = new AccommodationReservationService();
        }
        private List<SuperGuestTitle> GetAllSuperGuests()
        {
            List<SuperGuestTitle> superGuests = new List<SuperGuestTitle>(superGuestTitleRepository.GetAll());
            foreach (SuperGuestTitle superGuest in superGuests)
            {
                Guest1 guest = guest1Service.GetById(superGuest.Guest.Id);
                if (guest != null)
                    superGuest.Guest = guest;
            }
            return superGuests;
        }       
        public void Add(SuperGuestTitle superGuestTitle)
        {
            superGuestTitleRepository.Add(superGuestTitle);
        }
        public bool IsAlreadySuperGuest(Guest1 guest1)
        {
             return superGuestTitleRepository.GetByGuestId(guest1.Id)!=null;
        }
       
        public SuperGuestTitle MakeNewSuperGuest(Guest1 guest1)
        {
            DateTime activationDate = accommodationReservationService.GetNewSuperGuestActivationDateIfPossible(guest1);
            if(activationDate!=DateTime.MinValue && !IsAlreadySuperGuest(guest1))
            {
                SuperGuestTitle newSuperGuestTitle = new SuperGuestTitle(guest1, 5, activationDate);
                Add(newSuperGuestTitle); 
                return newSuperGuestTitle;  
            }
            return superGuestTitleRepository.GetByGuestId(guest1.Id);
        }
        public void Delete(SuperGuestTitle title)
        {
            superGuestTitleRepository.Delete(title);
        }
        public SuperGuestTitle ProlongSuperGuestTitle(Guest1 guest1)
        {
            SuperGuestTitle superGuest = GetAllSuperGuests().Find(n => n.Guest.Id == guest1.Id);
            DateTime newActivationDate = accommodationReservationService.GetProlongActivationDate(guest1, superGuest.ActivationDate);
            if (newActivationDate != DateTime.MinValue) //can prolog title
            {
                Delete(superGuest);     //delete previous and add new title
                Add(new SuperGuestTitle(guest1, 5, newActivationDate));
            } 
            else if(DateTime.Now > superGuest.ActivationDate.AddYears(1))  //1 year has passed
                Delete(superGuest);     //delete old title

            return GetAllSuperGuests().Find(n=>n.Guest.Id == guest1.Id);    //new superguest or null
        }        
        public void DeleteTitleIfNeeded(Guest1 guest1)
        {
            if(ShouldDelete(guest1))
            {
                Delete(superGuestTitleRepository.GetByGuestId(guest1.Id));

            }
                    
        }

        private bool ShouldDelete(Guest1 guest1)
        {
            SuperGuestTitle foundedSuperGuest = superGuestTitleRepository.GetByGuestId(guest1.Id);
            return foundedSuperGuest != null && DateTime.Now > foundedSuperGuest.ActivationDate.AddYears(2);
        }
        public void DecrementPoints(Guest1 guest1)
        {
            SuperGuestTitle title = superGuestTitleRepository.GetByGuestId(guest1.Id);
            if (title!=null)
            {
                if(title.AvailablePoints>0)
                    title.AvailablePoints -= 1;
                superGuestTitleRepository.Update(title);
            }            
        }
    }
}
