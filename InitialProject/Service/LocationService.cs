using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class LocationService
    {
        private LocationRepository locationRepository;

        public LocationService() 
        { 
            locationRepository = new LocationRepository();
        }
        public List<Location> GetAll()
        {
            return locationRepository.GetAll();
        }

        public Location Save(Location location)
        {
            return locationRepository.Save(location);
        }


        public void Delete(Location location)
        {
            locationRepository.Delete(location);
        }

        public Location Update(Location location)
        {
            return locationRepository.Update(location);
        }

        public int Add(Location location)
        {
            return locationRepository.Add(location);
        }

        public ObservableCollection<string> GetCitiesByCountry(string country)
        {
            ObservableCollection<string> cities = new ObservableCollection<string>();
            foreach (Location location in locationRepository.GetAll())
            {
                if (location.Country.Equals(country))
                {
                    if (!cities.Contains(location.City))
                    {
                        cities.Add(location.City);
                    }
                }
            }
            return cities;
        }

        public List<string> GetAllCountries()
        {
            List<string> countries = new List<string>();
            foreach (Location location in locationRepository.GetAll())
            {
                if (!countries.Contains(location.Country))
                {
                    countries.Add(location.Country);
                }
            }
            return countries;
        }

        public Location GetByCityAndCountry(string country, string city)
        {
            return locationRepository.GetAll().Find(n => n.City.Equals(city) && n.Country.Equals(country));
        }

        public Location GetById(int id)
        {
            return locationRepository.GetById(id);
        }
    }
}
