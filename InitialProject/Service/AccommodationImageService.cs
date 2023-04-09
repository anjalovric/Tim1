using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.Service
{
    public class AccommodationImageService
    {
        private AccommodationImageRepository accommodationImageRepository;
        public AccommodationImageService()
        {
            accommodationImageRepository = new AccommodationImageRepository();
        }

        public List<AccommodationImage> GetAll()
        {
            return accommodationImageRepository.GetAll();
        }
        public int Add(AccommodationImage image)
        {
            return accommodationImageRepository.Add(image);
        }

    }
}
