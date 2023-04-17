using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IReschedulingAccommodationRequestRepository : IGenericRepository<ReschedulingAccommodationRequest>
    {
        public void Add(ReschedulingAccommodationRequest request);
        public ReschedulingAccommodationRequest Update(ReschedulingAccommodationRequest updatedRequest);
    }
}
