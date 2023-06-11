using System;
using System.Collections.Generic;
using System.Linq;
using InitialProject.Domain.Model;
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

        public List<LeastPopularLocation> GetLeastPopularAccommodations(Owner owner)
        {
            Dictionary<int, int> locationReservationNumbers = new Dictionary<int, int>();
            Dictionary<int, double> locationBusinest = new Dictionary<int, double>();
            List<Accommodation> accommodations = new List<Accommodation>(accommodationService.GetAllByOwner(owner));
            foreach (Accommodation accommodation in accommodations)
            {
                CalculateReservationNumber(locationReservationNumbers, accommodation);
                CalculateBusinest(locationBusinest, accommodation);
            }

            return GetLeastPopularLocationList(locationReservationNumbers, locationBusinest, owner);
        }
        public List<MostPopularLocation> GetMostPopularLocations(Owner owner)
        {
            Dictionary<int, int> locationReservationNumbers = new Dictionary<int, int>();
            Dictionary<int, double> locationBusinest = new Dictionary<int, double>();
            List<Accommodation> accommodations = new List<Accommodation>(accommodationService.GetAllByOwner(owner));
            foreach (Accommodation accommodation in accommodations)
            {
                CalculateReservationNumber(locationReservationNumbers, accommodation);
                CalculateBusinest(locationBusinest, accommodation);
            }

            return GetMostPopularLocationList(locationReservationNumbers, locationBusinest);
        }

        private List<MostPopularLocation> GetMostPopularLocationList(Dictionary<int, int> locationReservationNumbers, Dictionary<int, double> locationBusinest)
        {
            List<MostPopularLocation> mostPopularLocations = new List<MostPopularLocation>();
            List<KeyValuePair<int, int>> locationListByReservationNumber = locationReservationNumbers.OrderByDescending(x => x.Value).ToList();
            List<KeyValuePair<int, double>> locationListByBusinest = locationBusinest.OrderByDescending(x => x.Value).ToList();

            if (locationListByReservationNumber[0].Key == locationListByBusinest[0].Key && locationListByBusinest[0].Value!=0)
            {
                Location location = locationService.GetById(locationListByReservationNumber[0].Key);
                MostPopularLocation mostPopularLocation = new MostPopularLocation(location, false, false, true);
                mostPopularLocations.Add(mostPopularLocation);
                return mostPopularLocations;
            }
            if(locationListByReservationNumber[0].Value!=0 || locationListByBusinest[0].Value != 0)
                MakeMostPopularList(mostPopularLocations, locationListByReservationNumber, locationListByBusinest);

            return mostPopularLocations;
        }

        private void MakeMostPopularList(List<MostPopularLocation> mostPopularLocations, List<KeyValuePair<int, int>> locationListByReservationNumber, List<KeyValuePair<int, double>> locationListByBusinest)
        {
            Location locationByReservationNumber = locationService.GetById(locationListByReservationNumber[0].Key);
            MostPopularLocation mostPopularLocation1 = new MostPopularLocation(locationByReservationNumber, true, false, false);
            mostPopularLocations.Add(mostPopularLocation1);

            Location locationByBusinest = locationService.GetById(locationListByBusinest[0].Key);
            MostPopularLocation mostPopularLocation2 = new MostPopularLocation(locationByBusinest, false, true, false);
            mostPopularLocations.Add(mostPopularLocation2);
        }

        private List<LeastPopularLocation> GetLeastPopularLocationList(Dictionary<int, int> locationReservationNumbers, Dictionary<int, double> locationBusinest, Owner owner)
        {
            List<LeastPopularLocation> leastPopularLocations = new List<LeastPopularLocation>();
            int numberOfLocations = locationReservationNumbers.Count;
            List<KeyValuePair<int, int>> locationListByReservationNumber = locationReservationNumbers.OrderByDescending(x => x.Value).ToList();
            List<KeyValuePair<int, double>> locationListByBusinest = locationBusinest.OrderByDescending(x => x.Value).ToList();

            if (locationListByReservationNumber[numberOfLocations-1].Key == locationListByBusinest[numberOfLocations-1].Key)
            {
                return GetLeastPopularLocation(owner, leastPopularLocations, locationListByReservationNumber);
            }
            MakeLeastPopularList(leastPopularLocations, locationListByReservationNumber, locationListByBusinest, owner);

            return leastPopularLocations;
        }

        private List<LeastPopularLocation> GetLeastPopularLocation(Owner owner, List<LeastPopularLocation> leastPopularLocations, List<KeyValuePair<int, int>> locationListByReservationNumber)
        {
            foreach (var accommodation in GetAccommodationsByLocationId(locationListByReservationNumber[locationListByReservationNumber.Count-1].Key, owner))
            {
                LeastPopularLocation leastPopularLocation = new LeastPopularLocation(accommodation, false, false, true);
                leastPopularLocations.Add(leastPopularLocation);
            }
            return leastPopularLocations;
        }

        private void MakeLeastPopularList(List<LeastPopularLocation> leastPopularLocations, List<KeyValuePair<int, int>> locationListByReservationNumber, List<KeyValuePair<int, double>> locationListByBusinest, Owner owner)
        {
            int numberOfLocations = locationListByReservationNumber.Count;
            Location locationByReservationNumber = locationService.GetById(locationListByReservationNumber[numberOfLocations-1].Key);
            foreach (var accommodation in GetAccommodationsByLocationId(locationListByReservationNumber[numberOfLocations-1].Key, owner))
            {
                LeastPopularLocation leastPopularLocation = new LeastPopularLocation(accommodation, true, false, false);
                leastPopularLocations.Add(leastPopularLocation);
            }

            Location locationByBusinest = locationService.GetById(locationListByBusinest[numberOfLocations-1].Key);
            foreach (var accommodation in GetAccommodationsByLocationId(locationListByBusinest[numberOfLocations-1].Key, owner))
            {
                LeastPopularLocation leastPopularLocation = new LeastPopularLocation(accommodation, false, true, false);
                leastPopularLocations.Add(leastPopularLocation);
            }
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

        private void CalculateBusinest(Dictionary<int, double> locations, Accommodation accommodation)
        {
            int busyDays = 0;
            int totalNumberOfDays = 0;
            List<int> years = ownerYearStatisticsService.GetAllYears(accommodation);
            if (years.Count == 0)
            {
                locations[accommodation.Location.Id] = 0;
                return;
            }
            foreach (int year in years)
            {
                busyDays += ownerYearStatisticsService.GetBusyDaysNumberByYear(accommodation, year);
                totalNumberOfDays += DateTime.IsLeapYear(year) ? 366 : 365;
            }

            if (!locations.ContainsKey(accommodation.Location.Id))
            {
                locations.Add(accommodation.Location.Id, (double)busyDays/totalNumberOfDays);
            }
            else
            {
                locations[accommodation.Location.Id] += (double)busyDays / totalNumberOfDays;
            }
        }

        private List<Accommodation> GetAccommodationsByLocationId(int id, Owner owner)
        {
            return accommodationService.GetAll().FindAll(n => n.Location.Id == id && n.Owner.Id == owner.Id);
        }

    }
}
