using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Service
{
    public class TourService
    {
        private TourRepository tourRepository;

        public TourService() 
        {
            tourRepository = new TourRepository();
        }

        public List<Tour> GetAll()
        {
            return tourRepository.GetAll();
        }

        public Tour Save(Tour tour)
        {

            return tourRepository.Save(tour);
        }

        public void Delete(Tour tour)
        {
            tourRepository.Delete(tour);
        }

        public Tour Update(Tour tour)
        {
            return tourRepository.Update(tour);
        }
        
    }
}
