using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.Service
{
    public class AccommodationTypeService
    {
        private IAccommodationTypeRepository accommodationTypeRepository = Injector.CreateInstance<IAccommodationTypeRepository>();

        public AccommodationTypeService()
        {
        }

        public List<AccommodationType> GetAll()
        {
            return accommodationTypeRepository.GetAll();
        }

        public AccommodationType GetById(int id)
        {
            return accommodationTypeRepository.GetById(id);
        }

    }
}
