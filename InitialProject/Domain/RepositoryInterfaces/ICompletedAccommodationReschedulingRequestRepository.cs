using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ICompletedAccommodationReschedulingRequestRepository : IGenericRepository<CompletedAccommodationReschedulingRequest>
    {
        public void Add(CompletedAccommodationReschedulingRequest request);
    }
}
