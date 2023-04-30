using System;
using System.Collections.Generic;
using System.Linq;
using InitialProject.Serializer;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;

namespace InitialProject.Domain.Model
{
    public class AccommodationRenovationSuggestion : ISerializable
    {
        public int Id { get; set; }
        public AccommodationReservation Reservation { get; set; }
        public int LevelOfUrgency { get; set; }
        public string ConditionsOfAccommodation { get; set; }

        public AccommodationRenovationSuggestion() 
        {
            Reservation = new AccommodationReservation();
        }

        public AccommodationRenovationSuggestion(AccommodationReservation reservation, int levelOfUrgency, string conditionsOfAccommodation)
        {
            Reservation = reservation;
            LevelOfUrgency = levelOfUrgency;
            ConditionsOfAccommodation = conditionsOfAccommodation;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Reservation = new AccommodationReservation();
            Reservation.Id = Convert.ToInt32(values[1]);
            ConditionsOfAccommodation = values[2];
            LevelOfUrgency = Convert.ToInt32(values[3]);
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Reservation.Id.ToString(), ConditionsOfAccommodation, LevelOfUrgency.ToString()};
            return csvValues;
        }
    }
}
