using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Serializer;

namespace InitialProject.Model
{
    public class CancelledAccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public Owner owner { get; set; }
        public AccommodationReservation reservation { get; set; }

        public CancelledAccommodationReservation()
        {
            owner = new Owner();
            reservation = new AccommodationReservation();
        }
        public CancelledAccommodationReservation(AccommodationReservation reservation)
        {
            this.owner = reservation.Accommodation.Owner;
            this.reservation= reservation;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            reservation.Id = Convert.ToInt32(values[1]);
            reservation.Accommodation.Id = Convert.ToInt32(values[2]);
            owner =new Owner();
            owner.Id = Convert.ToInt32(values[3]);
            reservation.Guest.Id = Convert.ToInt32(values[4]);

            reservation.Arrival = Convert.ToDateTime(values[5]);
            reservation.Departure = Convert.ToDateTime(values[6]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString() ,reservation.Id.ToString(), reservation.Accommodation.Id.ToString(), owner.Id.ToString(), reservation.Guest.Id.ToString(), reservation.Arrival.ToString(), reservation.Departure.ToString() };
            return csvValues;
        }

    }
}
