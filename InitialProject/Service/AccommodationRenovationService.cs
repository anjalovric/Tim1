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
        private List<AccommodationRenovation> renovations;
        public AccommodationRenovationService()
        {
            renovations = accommodationRenovationRepository.GetAll();
            SetAccommodations();
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
            return renovations.FindAll(n => n.Accommodation.Owner.Id == owner.Id);
        }

        public void Add(AccommodationRenovation renovation)
        {
            accommodationRenovationRepository.Add(renovation);
        }
    }
}
