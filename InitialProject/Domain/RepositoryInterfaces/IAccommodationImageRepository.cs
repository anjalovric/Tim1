using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationImageRepository : IGenericRepository<AccommodationImage>
    {
        public int Add(AccommodationImage image);
        public List<string> GetUrlByAccommodationId(int id);
    }
}
