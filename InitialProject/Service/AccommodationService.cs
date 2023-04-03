using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.Service
{
    public class AccommodationService
    {
        private AccommodationRepository accommodationRepository;
        private List<Accommodation> accommodations;
        public AccommodationService()
        {
            accommodationRepository = new AccommodationRepository();
            MakeAccommodations();
        }

        private void MakeAccommodations()
        {
            accommodations = accommodationRepository.GetAll();
            AddOwners();
            AddLocations();
        }

        public List<Accommodation> GetAll()
        {
            return accommodations;
        }

        private void AddOwners()
        {
            OwnerService ownerService = new OwnerService();
            List<Owner> allOwners = ownerService.GetAll();
            foreach(Accommodation accommodation in accommodations)
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
    }
}
