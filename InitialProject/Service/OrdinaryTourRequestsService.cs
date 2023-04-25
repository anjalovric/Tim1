using InitialProject.Domain;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Serializer;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

    }

}
