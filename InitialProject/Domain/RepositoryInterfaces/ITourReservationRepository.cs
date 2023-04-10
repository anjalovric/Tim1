using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ITourReservationRepository:IGenericRepository<TourReservation>
    {
        public void Add(TourReservation tourReservation);

        public List<TourReservation> GetReservationsForTourInstance(int tourInstanceId);


        public void Delete(TourReservation tourReservation);

        public TourReservation Update(TourReservation tourReservation,int guestsNumber,Boolean withVoucher);

        public TourReservation Save(TourReservation tourReservation);

        public List<TourReservation> GetReservationsForTour(TourInstance selected);
    }
}
