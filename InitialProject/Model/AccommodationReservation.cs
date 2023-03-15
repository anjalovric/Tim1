using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using InitialProject.Serializer;

namespace InitialProject.Model
{
    public class AccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public Accommodation Accommodation { get; set; }


        public DateTime Arrival  { get; set; }
        public DateTime Departure { get; set; }


        public AccommodationReservation() {
            Accommodation = new Accommodation();
        }
        public AccommodationReservation(int guestId, Accommodation currentAccommodation, DateTime arrivalDate, DateTime departureDate)
        {
            GuestId = guestId;
            this.Accommodation = currentAccommodation;
            Arrival = arrivalDate;
            Departure = departureDate;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            Accommodation.Id = Convert.ToInt32(values[2]);
            Arrival = Convert.ToDateTime(values[3]);
            Departure = Convert.ToDateTime(values[4]);


        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuestId.ToString(), Accommodation.Id.ToString(), Arrival.ToString(), Departure.ToString()};
            return csvValues;
        }
    }
}
