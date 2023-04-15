using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Model;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ICancelledAccommodationReservationRepository : IGenericRepository<AccommodationReservation>
    {
        public void Add(AccommodationReservation reservation);
        public List<AccommodationReservation> GetAll();
        public int NextId();
        public AccommodationReservation GetById(int id);
        public int GetMaxId();
    }
}
