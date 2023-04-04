using System;
using System.Collections.Generic;
using InitialProject.Serializer;

namespace InitialProject.Model
{
    public class AccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public Guest1 Guest { get; set; }
        public Accommodation Accommodation { get; set; }
        public DateTime Arrival  { get; set; }
        public DateTime Departure { get; set; }

        

        public AccommodationReservation() {
            Guest = new Guest1();
            Accommodation = new Accommodation();
        }
        public AccommodationReservation(Guest1 guest, Accommodation currentAccommodation, DateTime arrival, DateTime departure)
        {
            Guest = guest;
            this.Accommodation = currentAccommodation;
            Arrival = arrival;
            Departure = departure;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Guest = new Guest1();
            Guest.Id = Convert.ToInt32(values[1]);
            Accommodation.Id = Convert.ToInt32(values[2]);
            
            Arrival = Convert.ToDateTime(values[3]);
            Departure = Convert.ToDateTime(values[4]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Guest.Id.ToString(), Accommodation.Id.ToString(), Arrival.ToString(), Departure.ToString()};
            return csvValues;
        }
    }
}
