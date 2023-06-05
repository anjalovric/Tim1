using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Service;

namespace InitialProject.APPLICATION.UseCases
{
    public class LocationSuggestionsService
    {
        private OwnerYearStatisticsService ownerYearStatisticsService;
        private AccommodationService accommodationService;
        private LocationService locationService;
        public LocationSuggestionsService()
        {
            ownerYearStatisticsService = new OwnerYearStatisticsService();
            accommodationService = new AccommodationService();
            locationService = new LocationService();
        }

        public List<Location> GetMostPopularLocations(Owner owner)
        {
            Dictionary<int, int> locations = SortLocations(owner);

            List<Location> mostPopularLocations = MakeBestLocationsList(locations, 3);
            return mostPopularLocations;
        }

        public List<Accommodation> GetLeastPopularAccommodations(Owner owner)
        {
            Dictionary<int, int> locations = SortLocations(owner);

            List<Location> leastPopularLocations = MakeWorstLocationsList(locations, 3);
            List<Accommodation> leastPopularAccommodations = GetAccommodationsByLocationList(leastPopularLocations, owner);
            return leastPopularAccommodations;
        }
        private Dictionary<int, int> SortLocations(Owner owner)
        {
            Dictionary<int, int> locations = new Dictionary<int, int>();
            List<Accommodation> accommodations = new List<Accommodation>(accommodationService.GetAllByOwner(owner));
            foreach (Accommodation accommodation in accommodations)
            {
                CalculateReservationNumber(locations, accommodation);
            }

            return locations;
        }

        private List<Location> MakeBestLocationsList(Dictionary<int, int> locations, int numberOfLocations)
        {
            List<Location> mostPopularLocations = new List<Location>();
            List<KeyValuePair<int, int>> locationList = locations.OrderByDescending(x => x.Value).ToList();
            for (var i = 0; i < numberOfLocations; i++)
            {
                Location location = locationService.GetById(locationList[i].Key);
                if(mostPopularLocations.Find(n => n.Id == location.Id) == null)
                    mostPopularLocations.Add(location);
            }

            return mostPopularLocations;
        }

        private List<Location> MakeWorstLocationsList(Dictionary<int, int> locations, int numberOfLocations)
        {
            List<Location> leastPopularLocations = new List<Location>();
            List<KeyValuePair<int, int>> locationList = locations.OrderBy(x => x.Value).ToList();
            for (var i = 0; i < numberOfLocations; i++)
            {
                Location location = locationService.GetById(locationList[i].Key);
                if (leastPopularLocations.Find(n => n.Id == location.Id) == null)
                    leastPopularLocations.Add(location);
            }

            return leastPopularLocations;
        }

        private void CalculateReservationNumber(Dictionary<int, int> locations, Accommodation accommodation)
        {
            int reservationNumber = 0;
            List<int> years = ownerYearStatisticsService.GetAllYears(accommodation);
            foreach (int year in years)
            {
                reservationNumber += ownerYearStatisticsService.GetReservationNumberByYear(accommodation, year);
            }
            if (!locations.ContainsKey(accommodation.Location.Id))
            {
                locations.Add(accommodation.Location.Id, reservationNumber);
            }
            else
            {
                locations[accommodation.Location.Id] += reservationNumber;
            }
        }

        private List<Accommodation> GetAccommodationsByLocationList(List<Location> locations, Owner owner)
        {
            List<Accommodation> accommodations = new List<Accommodation>();
            foreach(Location location in locations)
            {
                if (accommodationService.HasAccommodationOnLocation(owner.Username, location))
                    foreach(var accommodation in accommodationService.GetAllByOwner(owner))
                    {
                        if (accommodations.Find(n => n.Id == accommodation.Id) == null && accommodation.Location.Id == location.Id)
                            accommodations.Add(accommodation);
                    }
            }
            return accommodations;
        }

    }
}
