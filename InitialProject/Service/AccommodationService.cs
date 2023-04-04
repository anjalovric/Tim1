using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.Service
{
    public class AccommodationService
    {
        private AccommodationRepository accommodationRepository;

        public AccommodationService()
        {
            accommodationRepository = new AccommodationRepository();
        }

        public List<Accommodation> GetAll()
        {
            return accommodationRepository.GetAll();
        }



        public void Add(Accommodation accommodation)
        {
            accommodationRepository.Add(accommodation);
        }

        public Accommodation GetById(int id)
        {
            return accommodationRepository.GetById(id);
        }
    }
}
