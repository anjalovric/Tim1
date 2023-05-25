using InitialProject.Domain.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface INumberOfVisitedToursRepository : IGenericRepository<NumberOfVisitedTours>
    {
        public NumberOfVisitedTours Save(NumberOfVisitedTours tour);
        public NumberOfVisitedTours Update(NumberOfVisitedTours tour);
        public void Delete(NumberOfVisitedTours tour);
        

    }
}
