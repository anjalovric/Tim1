using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface IAccommodationReservationRepository : IGenericRepository<AccommodationReservation>
    {
        public void Add(AccommodationReservation accommodationReservation);
        public void Delete(AccommodationReservation accommodationReservation);
        public AccommodationReservation Update(AccommodationReservation newReservation);
    }
}
