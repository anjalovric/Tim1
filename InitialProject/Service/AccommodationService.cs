using System.Collections.Generic;
using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;

namespace InitialProject.Service
{
    public class AccommodationService
    {
        private IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
        private List<Accommodation> accommodations;
        public AccommodationService()
        {
            MakeAccommodations();
        }
        public List<Accommodation> GetAll()
        {
            return accommodations;
        }
        public void Add(Accommodation accommodation)
        {
            accommodationRepository.Add(accommodation);
        }
        public Accommodation GetById(int id)
        {
            return accommodations.Find(n => n.Id == id);
        }
        private void MakeAccommodations()
        {
            accommodations = accommodationRepository.GetAll();
            AddOwners();
            AddLocations();
            AddTypes();
        }
        private void AddOwners()
        {
            OwnerService ownerService = new OwnerService();
            List<Owner> allOwners = ownerService.GetAll();
            foreach (Accommodation accommodation in accommodations)
            {
                Owner accommodationOwner = allOwners.Find(n => n.Id == accommodation.Owner.Id);
                if (accommodationOwner != null)
                {
                    accommodation.Owner = accommodationOwner;
                }
            }
        }
        private void AddLocations()
        {
            LocationService locationService = new LocationService();
            List<Location> allLocations = locationService.GetAll();
            foreach (Accommodation accommodation in accommodations)
            {
                Location accommodationLocation = allLocations.Find(n => n.Id == accommodation.Location.Id);
                if (accommodationLocation != null)
                {
                   accommodation.Location = accommodationLocation;
                }
            }
        }

        private void AddTypes()
        {
            AccommodationTypeService accommodationTypeService = new AccommodationTypeService();
            List<AccommodationType> allTypes = accommodationTypeService.GetAll();
            foreach (Accommodation accommodation in accommodations)
            {
                AccommodationType accommodationType = allTypes.Find(n => n.Id == accommodation.Type.Id);
                if (accommodationType != null)
                {
                    accommodation.Type = accommodationType;
                }
            }
        }

        public List<Accommodation> GetAllByOwner(Owner owner)
        {
            AccommodationImageService imageService = new AccommodationImageService();
            List<Accommodation> accommodationsByOwner = new List<Accommodation>();
            foreach (Accommodation accommodation in accommodations)
            {
                if (accommodation.Owner.Id == owner.Id)
                {
                    accommodation.CoverImage = imageService.GetCoverImage(accommodation);
                    accommodationsByOwner.Add(accommodation);
                }
            }
            return accommodationsByOwner;
        }
        
    }
}
