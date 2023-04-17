using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationRepository : IGenericRepository<Accommodation>
    {
        public void Add(Accommodation accommodation);
    }
}
