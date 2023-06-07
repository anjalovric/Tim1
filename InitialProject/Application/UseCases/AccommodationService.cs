using System;
using System.Collections.Generic;
using InitialProject.Domain;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;

namespace InitialProject.Service
{
    public class AccommodationService
    {
        private IAccommodationRepository accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
        private List<Accommodation> accommodations;
        private List<Accommodation> Accommodations;//for search
        private LocationService locationService;
        public AccommodationService()
        {
            MakeAccommodations();
            Accommodations = new List<Accommodation>(accommodations);
            locationService = new LocationService();
        }
        public List<Accommodation> GetAll()
        {
            return accommodations;
        }
        public void Add(Accommodation accommodation)
        {
            accommodation.Location = locationService.GetByCityAndCountry(accommodation.Location.Country, accommodation.Location.City);
            accommodationRepository.Add(accommodation);
        }
        public Accommodation GetById(int id)
        {
            return accommodations.Find(n => n.Id == id);
        }
        private void MakeAccommodations()
        {
            accommodations = accommodationRepository.GetAll();
            SetOwners();
            SetLocations();
            SetTypes();
            SetAccommodationCoverImages();
        }
        private void SetAccommodationCoverImages()
        {
            AccommodationImageService accommodationImageService = new AccommodationImageService();
            foreach (Accommodation accommodation in accommodations)
                accommodation.CoverImage = accommodationImageService.GetCoverImage(accommodation);
        }
        private void SetOwners()
        {
            OwnerService ownerService = new OwnerService();
            List<Owner> allOwners = ownerService.GetAll();
            foreach (Accommodation accommodation in accommodations)
            {
                Owner accommodationOwner = allOwners.Find(n => n.Id == accommodation.Owner.Id);
                if (accommodationOwner != null)
                    accommodation.Owner = accommodationOwner;
            }
        }
        private void SetLocations()
        {
            LocationService locationService = new LocationService();
            List<Location> allLocations = locationService.GetAll();
            foreach (Accommodation accommodation in accommodations)
            {
                Location accommodationLocation = allLocations.Find(n => n.Id == accommodation.Location.Id);
                if (accommodationLocation != null)
                   accommodation.Location = accommodationLocation;
            }
        }
        private void SetTypes()
        {
            AccommodationTypeService accommodationTypeService = new AccommodationTypeService();
            List<AccommodationType> allTypes = accommodationTypeService.GetAll();
            foreach (Accommodation accommodation in accommodations)
            {
                AccommodationType accommodationType = allTypes.Find(n => n.Id == accommodation.Type.Id);
                if (accommodationType != null)
                    accommodation.Type = accommodationType;
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
        public void Delete(Accommodation accommodation)
        {
            accommodationRepository.Delete(accommodation);
        }
        public List<Accommodation> SearchName(Accommodation accommodation, string Name)
        {
            if (!accommodation.Name.ToLower().Contains(Name.ToLower()))
                Accommodations.Remove(accommodation);
            return Accommodations;
        }
        public List<Accommodation> SearchCity(Accommodation accommodation, string LocationCity)
        {
            if (LocationCity != null && !accommodation.Location.City.ToLower().Equals(LocationCity.ToLower()))
                Accommodations.Remove(accommodation);
            return Accommodations;
        }
        public List<Accommodation> SearchCountry(Accommodation accommodation, string LocationCountry)
        {
            if (LocationCountry != null && !accommodation.Location.Country.ToLower().Equals(LocationCountry.ToLower()))
                Accommodations.Remove(accommodation);
            return Accommodations;
        }
        public List<Accommodation> SearchType(Accommodation accommodation, bool ApartmentChecked, bool HouseChecked, bool CottageChecked)
        {
            if (ApartmentChecked == true || HouseChecked == true || CottageChecked == true)
            {
                if (ApartmentChecked == false)
                    RemoveIfApartment(accommodation);

                if (HouseChecked == false)
                    RemoveIfHouse(accommodation);

                if (CottageChecked == false)
                    RemoveIfCottage(accommodation);
            }
            return Accommodations;
        }
        private void RemoveIfApartment(Accommodation accommodation)
        {
            if (accommodation.Type.Name.ToLower() == "apartment")
                Accommodations.Remove(accommodation);
        }
        private void RemoveIfHouse(Accommodation accommodation)
        {
            if (accommodation.Type.Name.ToLower() == "house")
                Accommodations.Remove(accommodation);
        }
        private void RemoveIfCottage(Accommodation accommodation)
        {
            if (accommodation.Type.Name.ToLower() == "cottage")
                Accommodations.Remove(accommodation);
        }
        public List<Accommodation> SearchNumberOfGuests(Accommodation accommodation, string NumberOfGuests)
        {
            if (!(NumberOfGuests == "") && Convert.ToInt32(NumberOfGuests) > accommodation.Capacity)
                Accommodations.Remove(accommodation);
            return Accommodations;
        }
        public List<Accommodation> SearchNumberOfDays(Accommodation accommodation, string NumberOfDays)
        {
            if (!(NumberOfDays == "") && Convert.ToInt32(NumberOfDays) < accommodation.MinDaysForReservation)
                Accommodations.Remove(accommodation);
            return Accommodations;
        }
        public bool HasAccommodationOnLocation(String username, Location location)
        {
            return accommodations.Find(r => r.Location.Id == location.Id && r.Owner.Username == username) != null;

        }
    }
}
