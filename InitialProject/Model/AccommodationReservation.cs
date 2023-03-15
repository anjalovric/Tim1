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

        public DateTime ArrivalDate  { get; set; }
        public DateTime DepartureDate { get; set; }

        public AccommodationReservation() {
            Accommodation = new Accommodation();
        }
        public AccommodationReservation(int guestId, Accommodation currentAccommodation, DateTime arrivalDate, DateTime departureDate)
        {
            GuestId = guestId;
            this.Accommodation = currentAccommodation;
            ArrivalDate = arrivalDate;
            DepartureDate = departureDate;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            Accommodation.Id = Convert.ToInt32(values[2]);
            ArrivalDate = Convert.ToDateTime(values[3]);
            DepartureDate = Convert.ToDateTime(values[4]);


        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuestId.ToString(), Accommodation.Id.ToString(), ArrivalDate.ToString(), DepartureDate.ToString()};
            return csvValues;
        }
    }
}
