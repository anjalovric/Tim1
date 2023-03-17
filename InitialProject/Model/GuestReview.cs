using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Serializer;

namespace InitialProject.Model
{
    public class GuestReview : ISerializable
    {
        public int Id { get; set; }
        public int Cleanliness { get; set; }
        public int RulesFollowing { get; set; }
        public string Comment { get; set; }
        public AccommodationReservation Reservation { get; set; }
        public GuestReview()
        {
            Reservation = new AccommodationReservation();
        }

        public GuestReview(int cleanliness, int rulesFollowing)
        {
            Cleanliness = cleanliness;
            RulesFollowing = rulesFollowing;
            Reservation = new AccommodationReservation();
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Cleanliness.ToString(), RulesFollowing.ToString(), Comment, Reservation.Id.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Cleanliness = Convert.ToInt32(values[1]);
            RulesFollowing = Convert.ToInt32(values[2]);
            Comment = values[3];
            Reservation = new AccommodationReservation();
            Reservation.Id = Convert.ToInt32(values[4]);
        }
    }
}
