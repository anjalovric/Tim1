using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Serializer;

namespace InitialProject.Model
{
    public class OwnerReview : ISerializable
    {
        public int Id { get; set; }
        public AccommodationReservation Reservation { get; set; }
        public int Cleanliness { get; set; }
        public int Correctness { get; set; }
        public string Comment { get; set; }

        public OwnerReview()
        {
            Reservation = new AccommodationReservation();
        }

        public OwnerReview(AccommodationReservation reservation, int cleanliness, int correctness, string comment)
        {
            Reservation = reservation;
            Cleanliness = cleanliness;
            Correctness = correctness;
            Comment = comment;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Cleanliness.ToString(), Correctness.ToString(), Comment, Reservation.Id.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Cleanliness = Convert.ToInt32(values[1]);
            Correctness = Convert.ToInt32(values[2]);
            Comment = values[3];
            Reservation = new AccommodationReservation();
            Reservation.Id = Convert.ToInt32(values[4]);
        }

    }
}
