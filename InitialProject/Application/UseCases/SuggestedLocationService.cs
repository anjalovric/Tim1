using InitialProject.Domain.Model;
using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
namespace InitialProject.Service
{
    public class SuggestedLocationService
    {
        private LocationService locationService;
        private OrdinaryTourRequestsService ordinaryTourRequestsService;
        public SuggestedLocationService() 
        { 
            locationService = new LocationService();
            ordinaryTourRequestsService=new OrdinaryTourRequestsService();
        }
        private List<OrdinaryTourRequests> GetRequestsFromLastYear()
        {
            DateTime today = DateTime.Now;
            string yearago = today.Month + "/" + today.Day + "/" + (today.Year - 1) + " " + today.ToString().Split(" ")[1] + " " + today.ToString().Split(" ")[2];
            List<OrdinaryTourRequests> ordinaryTourRequests = new List<OrdinaryTourRequests>();
            if (ordinaryTourRequestsService.GetAll().Count > 0)
            {
                foreach (OrdinaryTourRequests request in ordinaryTourRequestsService.GetAll())
                    if (request.CreateDate >= Convert.ToDateTime(yearago))
                        if (request.CreateDate <= today)
                            ordinaryTourRequests.Add(request);
                SetLocation(ordinaryTourRequests);
            }          
            return ordinaryTourRequests;
        }
        private List<Location> GetRequestsLocationsFromLastYear()
        {
            List<Location> locations = new List<Location>();
            if (GetRequestsFromLastYear().Count > 0)
            {
                locations.Add(GetRequestsFromLastYear()[0].Location);
                foreach (OrdinaryTourRequests request in GetRequestsFromLastYear())
                    if (!locations.Contains(request.Location))
                        locations.Add(request.Location);
            }
            return locations;
        }
        private int CountRequestForLocation(Location location)
        {
            int count = 0;
            foreach (OrdinaryTourRequests request in GetRequestsFromLastYear())
                if (request.Location.Id == location.Id)
                    count++;
            return count;
        }
        private Dictionary<Location, int> SetRequestForLocationNumber()
        {
            Dictionary<Location, int> locationsRequests = new Dictionary<Location, int>();
            if (GetRequestsLocationsFromLastYear().Count > 0)
                foreach (Location location in GetRequestsLocationsFromLastYear())
                    locationsRequests.Add(location, CountRequestForLocation(location));
            return locationsRequests;
        }
        public Location GetMostWantedLocation()
        {
            Location location = null;
            if (SetRequestForLocationNumber().Count > 0)
            {
                int maximum = 0;
                Dictionary<Location, int> locations = SetRequestForLocationNumber();
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
            foreach (OrdinaryTourRequests request in requests)
                foreach (Location location in locationService.GetAll())
                    if (location.Id == request.Location.Id)
                        request.Location = location;
        }
    }
}
