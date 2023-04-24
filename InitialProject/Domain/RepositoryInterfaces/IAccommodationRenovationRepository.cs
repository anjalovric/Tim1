using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationRenovationRepository : IGenericRepository<AccommodationRenovation>
    {
        public void Add(AccommodationRenovation renovation);
        public void Delete(AccommodationRenovation renovation);
    }
}
