using InitialProject.Domain.Model;
using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
namespace InitialProject.Service
{
    public class SuggestedLocationService
    {
        public SuggestedLocationService() { }
        private List<OrdinaryTourRequests> GetRequestsInLanstYear()
        {
            DateTime today = DateTime.Now;
            string yearago = today.Month + "/" + today.Day + "/" + (today.Year - 1) + " " + today.ToString().Split(" ")[1] + " " + today.ToString().Split(" ")[2];
            List<OrdinaryTourRequests> ordinaryTourRequests = new List<OrdinaryTourRequests>();
            OrdinaryTourRequestsService ordinaryTourRequestsService = new OrdinaryTourRequestsService();
            if (ordinaryTourRequestsService.GetAll().Count > 0)
            {
                foreach (OrdinaryTourRequests request in ordinaryTourRequestsService.GetAll())
                    if (request.StartDate >= Convert.ToDateTime(yearago))
                        if (request.StartDate <= today)
                            ordinaryTourRequests.Add(request);
                SetLocation(ordinaryTourRequests);
            }          
            return ordinaryTourRequests;
        }
        private List<Location> GetLocations()
        {
            List<Location> locations = new List<Location>();
            if (GetRequestsInLanstYear().Count > 0)
            {
                locations.Add(GetRequestsInLanstYear()[0].Location);
                foreach (OrdinaryTourRequests request in GetRequestsInLanstYear())
                    if (!locations.Contains(request.Location))
                        locations.Add(request.Location);
            }
            return locations;
        }
        private int CountRequestForLocation(Location location)
        {
            int count = 0;
            foreach (OrdinaryTourRequests request in GetRequestsInLanstYear())
                if (request.Location.Id == location.Id)
                    count++;
            return count;
        }
        private Dictionary<Location, int> GetRequestNumber()
        {
            Dictionary<Location, int> locationsRequests = new Dictionary<Location, int>();
            if (GetLocations().Count > 0)
                foreach (Location location in GetLocations())
                    locationsRequests.Add(location, CountRequestForLocation(location));
            return locationsRequests;
        }
        public Location GetMostWantedLocation()
        {
            Location location = null;
            if (GetRequestNumber().Count > 0)
            {
                int maximum = 0;
                Dictionary<Location, int> locations = GetRequestNumber();
                for (int index = 0; index < locations.Count; index++)
                {
                    var item = locations.ElementAt(index);
                    if (item.Value > maximum)
                    {
                        maximum = item.Value;
                        location = item.Key;
                    }
                }
            }
            return location;
        }
        private void SetLocation(List<OrdinaryTourRequests> requests)
        {
            LocationService locationService = new LocationService();
            foreach (OrdinaryTourRequests request in requests)
                foreach (Location location in locationService.GetAll())
                    if (location.Id == request.Location.Id)
                        request.Location = location;
        }
    }
}
