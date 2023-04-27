﻿using System;
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
        private List<AccommodationRenovation> renovations;
        public AccommodationRenovationService()
        {
            renovations = accommodationRenovationRepository.GetAll();
            SetAccommodations();
            SetCanBeCancelled();
            SetIsInProgress();
        }

        private void SetAccommodations()
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
            List<AccommodationRenovation> renovationsByOwner = renovations.FindAll(n => n.Accommodation.Owner.Id == owner.Id);
            renovationsByOwner = renovationsByOwner.OrderByDescending(r => r.StartDate.Date).ToList();
            return renovationsByOwner;
        }

        public void Add(AccommodationRenovation renovation)
        {
            accommodationRenovationRepository.Add(renovation);
        }

        public void AreRenovated(List<Accommodation> accommodations)
        {
            foreach(var accommodation in accommodations)
            {
                AccommodationRenovation renovation = renovations.Find(n => n.Accommodation.Id == accommodation.Id);
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
            return renovations.FindAll(n => n.Accommodation.Owner.Id == owner.Id && n.StartDate>DateTime.Now.Date && !n.IsInProgress).Count();
        }

        public int CountRenovatedObjects(Owner owner)
        {
            return renovations.FindAll(n => n.Accommodation.Owner.Id == owner.Id && n.EndDate.Date<DateTime.Now.Date).Count();
        }

        private void SetCanBeCancelled()
        {
            foreach(var renovation in renovations)
            {
                if (renovation.StartDate.Date > DateTime.Now.Date.AddDays(5))
                    renovation.CanBeCancelled = true;
                else
                    renovation.CanBeCancelled = false;
            }
        }

        private void SetIsInProgress()
        {
            foreach (var renovation in renovations)
            {
                if (renovation.StartDate.Date <= DateTime.Now.Date && renovation.EndDate >=DateTime.Now.Date)
                    renovation.IsInProgress = true;
                else
                    renovation.IsInProgress = false;
            }
        }
    }
}
