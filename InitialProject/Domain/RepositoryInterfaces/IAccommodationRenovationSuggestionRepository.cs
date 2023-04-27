using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Model;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationRenovationSuggestionRepository : IGenericRepository<AccommodationRenovationSuggestion>
    {
        public void Add(AccommodationRenovationSuggestion suggestion);
        public void Delete(AccommodationRenovationSuggestion suggestion);
    }
}
