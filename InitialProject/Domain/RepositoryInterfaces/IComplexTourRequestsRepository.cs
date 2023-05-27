using InitialProject.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IComplexTourRequestsRepository : IGenericRepository<ComplexTourRequests>
    {
        public void Delete(ComplexTourRequests request);

        public ComplexTourRequests Update(ComplexTourRequests request);

        public ComplexTourRequests Save(ComplexTourRequests request);
        public List<ComplexTourRequests> GetByGuestId(int id);
    }
}
