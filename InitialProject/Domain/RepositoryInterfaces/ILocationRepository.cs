using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ILocationRepository : IGenericRepository<Location>
    {
        public int Add(Location location);

        public ObservableCollection<string> GetCitiesByCountry(string country);

        public List<string> GetAllCountries();

        public Location GetByCityAndCountry(string country, string city);

        public Location Save(Location location);

        public Location Update(Location location);

        public void Delete(Location location);

    }
}
