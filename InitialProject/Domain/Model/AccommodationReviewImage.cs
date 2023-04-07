using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Domain.Model
{
    public class AccommodationReviewImage : ISerializable
    {
        public int Id { get; set; }
        public AccommodationReservation Reservation { get; set; }
        public string RelativeUri { get; set; }
        public AccommodationReviewImage()
        {
            Reservation = new AccommodationReservation();
        }
        public AccommodationReviewImage(AccommodationReservation reservation, string relativeUri)
        {
            Reservation = reservation;
            RelativeUri = relativeUri;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Reservation = new AccommodationReservation();
            Reservation.Id = Convert.ToInt32(values[1]);
            RelativeUri = values[2];
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Reservation.Id.ToString(), RelativeUri };
            return csvValues;
        }

    }
}
