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
        private IOrdinaryTourRequestsRepository ordinaryTourRequestsRepository;
        private LocationService locationService;

        public OrdinaryTourRequestsService()
        {
            ordinaryTourRequestsRepository = Injector.CreateInstance<IOrdinaryTourRequestsRepository>();
            locationService = new LocationService();
        }
        public OrdinaryTourRequests Save(OrdinaryTourRequests request)
        {
            return ordinaryTourRequestsRepository.Save(request);
        }
        public List<OrdinaryTourRequests> GetAll()
        {
            return ordinaryTourRequestsRepository.GetAll();
        }
        public List<OrdinaryTourRequests> GetByGuestId(int id)
        {
            return ordinaryTourRequestsRepository.GetByGuestId(id);
        }
        public List<OrdinaryTourRequests> GetOnlyOrdinaryRequestsByGuestId(int id)
        {
            List<OrdinaryTourRequests> requests = new List<OrdinaryTourRequests>();
            foreach(OrdinaryTourRequests request in GetByGuestId(id))
            {
                if (request.ComplexId == -1)
                {
                    requests.Add(request);
                }
            }
            return requests;
        }
        public List<OrdinaryTourRequests> GetOnWaitingRequests()
        {
            List<OrdinaryTourRequests> requests= new List<OrdinaryTourRequests>();
            foreach(OrdinaryTourRequests request in GetAll()) 
            {
                if(request.Status==Status.ONWAITING)
                    requests.Add(request);
            }
            SetLocations(requests);
            return requests;
        }
        private void SetLocations(List<OrdinaryTourRequests> requests)
        { 
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
            return ordinaryTourRequestsRepository.Update(request);
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
        public List<OrdinaryTourRequests> GetInvalidOrWaitingRequests(List<OrdinaryTourRequests> OrdinaryTourRequests, Guest2 guest2)
        {
            List<OrdinaryTourRequests> ordinaryTourRequests = new List<OrdinaryTourRequests>();
            foreach (OrdinaryTourRequests request in OrdinaryTourRequests)
            {
                if ((request.Status == Status.ONWAITING || request.Status == Status.INVALID) && guest2.Id == request.GuestId)
                {
                    ordinaryTourRequests.Add(request);
                }
            }
            SetLocations(ordinaryTourRequests);
            return ordinaryTourRequests;
        }
        public List<OrdinaryTourRequests> GetAcceptedRequests(List<OrdinaryTourRequests> OrdinaryTourRequests)
        {
            List<OrdinaryTourRequests> ordinaryTourRequests = new List<OrdinaryTourRequests>();
            foreach (OrdinaryTourRequests request in OrdinaryTourRequests)
            {
                if (request.Status == Status.ACCEPTED)
                {
                    ordinaryTourRequests.Add(request);
                }
            }
            SetLocations(ordinaryTourRequests);
            return ordinaryTourRequests;
        }
    }
}
