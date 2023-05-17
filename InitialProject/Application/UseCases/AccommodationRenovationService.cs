using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain;
using InitialProject.Domain.Model;
using InitialProject.Model;

namespace InitialProject.Service
{
    public class AccommodationRenovationService
    {
        private IAccommodationRenovationRepository accommodationRenovationRepository = Injector.CreateInstance<IAccommodationRenovationRepository>();
        private OwnerNotificationsService notificationService;
        public AccommodationRenovationService()
        {
            notificationService = new OwnerNotificationsService();
        }

        public bool IsDateAvailableForReservation(int accommodationId, DateTime date)
        {
            List<AccommodationRenovation> renovations = accommodationRenovationRepository.GetAll();
            foreach(AccommodationRenovation renovation in renovations)
            {
                if(renovation.Accommodation.Id == accommodationId && date.Date>=renovation.StartDate.Date && date.Date<=renovation.EndDate.Date)
                {
                    return false;
                }
            }
            return true;
        }

        private void SetAccommodations(List<AccommodationRenovation> renovations)
        {
            AccommodationService accommodationService = new AccommodationService();
            foreach(var renovation in renovations)
            {
                Accommodation accommodation = accommodationService.GetAll().Find(n => n.Id == renovation.Accommodation.Id);
                if(accommodation != null)
                {
                    renovation.Accommodation = accommodation;
                }
            }
        }

        public List<AccommodationRenovation> GetAllByOwner(Owner owner)
        {
            List<AccommodationRenovation> renovations = MakeRenovations();
            List<AccommodationRenovation> renovationsByOwner = renovations.FindAll(n => n.Accommodation.Owner.Id == owner.Id);
            renovationsByOwner = renovationsByOwner.OrderByDescending(r => r.StartDate.Date).ToList();
            return renovationsByOwner;
        }

        private List<AccommodationRenovation> MakeRenovations()
        {
            List<AccommodationRenovation> renovations = accommodationRenovationRepository.GetAll();
            SetAccommodations(renovations);
            SetIsInProgress(renovations);
            SetIsFinished(renovations);
            SetCanBeCancelled(renovations);
            return renovations;
        }

        public void Add(AccommodationRenovation renovation)
        {
            accommodationRenovationRepository.Add(renovation);
            notificationService.Add(OwnerNotificationType.RENOVATION_SCHEDULED, renovation.Accommodation.Owner);
        }

        public void AreRenovated(List<Accommodation> accommodations)
        {
            List<AccommodationRenovation> renovations = MakeRenovations();
            foreach (var accommodation in accommodations)
            {
                AccommodationRenovation renovation = renovations.Find(n => n.Accommodation.Id == accommodation.Id && n.EndDate.Date<DateTime.Now.Date);
                if (renovation != null && (DateTime.Now.Year - renovation.EndDate.Year) <= 1)
                    accommodation.IsRenovated = true;
                else
                    accommodation.IsRenovated = false;
            }
        }

        public void Delete(AccommodationRenovation renovation)
        {
            accommodationRenovationRepository.Delete(renovation);
        }

        public int CountUpcomingRenovations(Owner owner)
        {
            List<AccommodationRenovation> renovations = MakeRenovations();
            return renovations.FindAll(n => n.Accommodation.Owner.Id == owner.Id && n.StartDate>DateTime.Now.Date && !n.IsInProgress).Count();
        }

        public int CountRenovatedObjects(Owner owner)
        {
            List<AccommodationRenovation> renovations = MakeRenovations();
            return renovations.FindAll(n => n.Accommodation.Owner.Id == owner.Id && n.EndDate.Date<DateTime.Now.Date).Count();
        }

        private void SetCanBeCancelled(List<AccommodationRenovation> renovations)
        {
            foreach(var renovation in renovations)
            {
                if (renovation.StartDate.Date > DateTime.Now.Date.AddDays(5))
                    renovation.CanBeCancelled = true;
                else
                    renovation.CanBeCancelled = false;
            }
        }

        private void SetIsInProgress(List<AccommodationRenovation> renovations)
        {
            foreach (var renovation in renovations)
            {
                if (renovation.StartDate.Date <= DateTime.Now.Date && renovation.EndDate.Date >=DateTime.Now.Date)
                    renovation.IsInProgress = true;
                else
                    renovation.IsInProgress = false;
            }
        }

        public bool IsRenovationOnDate(Accommodation accommodation, DateTime date)
        {
            List<AccommodationRenovation> renovations = MakeRenovations();
            return renovations.Find(n => n.Accommodation.Id == accommodation.Id && n.StartDate <= date && n.EndDate >= date) != null;
        }

        private void SetIsFinished(List<AccommodationRenovation> renovations)
        {
            foreach (var renovation in renovations)
            {
                if (renovation.EndDate < DateTime.Now)
                    renovation.IsFinished = true;
                else
                    renovation.IsFinished = false;
            }
        }
    }
}
