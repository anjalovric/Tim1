using InitialProject.Domain.Model;
using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IOrdinaryTourRequestsRepository : IGenericRepository<OrdinaryTourRequests>
    {

        public void Delete(OrdinaryTourRequests request);

        public OrdinaryTourRequests Update(OrdinaryTourRequests request);
        
        public OrdinaryTourRequests Save(OrdinaryTourRequests request);
        public List<OrdinaryTourRequests> GetByGuestId(int id);

    }
}
