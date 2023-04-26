using InitialProject.Domain;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Serializer;
using MathNet.Numerics;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace InitialProject.Service
{
    public class OrdinaryTourRequestsService
    {
        private IOrdinaryTourRequestsRepository requestRepository;
        private List<OrdinaryTourRequests> requests;
        public OrdinaryTourRequestsService()
        {
            requestRepository = Injector.CreateInstance<IOrdinaryTourRequestsRepository>();
            requests = GetAll();
        }
        public OrdinaryTourRequests Save(OrdinaryTourRequests request)
        {
            return requestRepository.Save(request);
        }
        public List<OrdinaryTourRequests> GetAll()
        {
            return requestRepository.GetAll();
        }
        public ObservableCollection<OrdinaryTourRequests> GetByGuestId(int id)
        {
            return requestRepository.GetByGuestId(id);
        }
        public List<OrdinaryTourRequests> GetOnWaitingRequests()
        {
            List<OrdinaryTourRequests> requests= new List<OrdinaryTourRequests>();
            foreach(OrdinaryTourRequests request in GetAll()) 
            {
                if(request.Status.Equals("On waiting"))
                    requests.Add(request);
            }
            SetLocations(requests);
            return requests;
        }
        private void SetLocations(List<OrdinaryTourRequests> requests) 
        {
            LocationService locationService = new LocationService();
            foreach(OrdinaryTourRequests request in requests)
            {
                foreach(Location location in locationService.GetAll())
                {
                    if(location.Id==request.Location.Id)
                        request.Location = location;
                }
            }
        }
        public OrdinaryTourRequests Update(OrdinaryTourRequests request)
        {
            return requestRepository.Update(request);
        }

        public List<OrdinaryTourRequests> GetByLanguage(string language) 
        { 
            List<OrdinaryTourRequests> ordinaryTourRequests = new List<OrdinaryTourRequests>();
            foreach(OrdinaryTourRequests request in GetAll())
                if (request.Language.Equals(language))
                    ordinaryTourRequests.Add(request);
            return ordinaryTourRequests;
        }
        public List<OrdinaryTourRequests> GetByLocation(Location location)
        {
            List<OrdinaryTourRequests> ordinaryTourRequests = new List<OrdinaryTourRequests>();
            foreach (OrdinaryTourRequests request in GetAll())
                if (request.Location.Id==location.Id)
                    ordinaryTourRequests.Add(request);
            return ordinaryTourRequests;
        }
        public double AverageNumberOfPeopleInAcceptedRequests(List<OrdinaryTourRequests> ordinaryTours,Guest2 Guest2)
        {
            int counter = 0;
            double averageNumberOfPeople = 0;
            foreach (OrdinaryTourRequests request in ordinaryTours)
            {
                if (request.Status == "Accepted" && request.GuestId == Guest2.Id)
                {
                    averageNumberOfPeople += request.MaxGuests;
                    counter++;
                }
            }
            if (averageNumberOfPeople == 0)
                return averageNumberOfPeople;
            return (averageNumberOfPeople /= counter).Round(2);
        }
        public double ProcentOfInvalidRequest(List<OrdinaryTourRequests> ordinaryTours, Guest2 Guest2)
        {
            double invalidRequest = 0;
            foreach (OrdinaryTourRequests request in ordinaryTours)
            {
                if (request.Status == "Invalid" && request.GuestId == Guest2.Id)
                {
                    invalidRequest++;
                }
            }
            if (invalidRequest == 0)
                return 0;
            invalidRequest /= ordinaryTours.Count();
            return invalidRequest *= 100;
        }
        public double ProcentOfAcceptedRequest(List<OrdinaryTourRequests> ordinaryTours, Guest2 Guest2)
        {
            double acceptedRequest = 0;
            foreach (OrdinaryTourRequests request in ordinaryTours)
            {
                if (request.Status == "Accepted" && request.GuestId == Guest2.Id)
                {
                    acceptedRequest++;
                }
            }
            if (acceptedRequest == 0)
                return acceptedRequest;
            acceptedRequest /= ordinaryTours.Count();
            return acceptedRequest *= 100;
        }
    }
}
