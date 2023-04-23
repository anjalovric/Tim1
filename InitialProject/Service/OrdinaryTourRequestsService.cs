using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Domain;
using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
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
    }
}
