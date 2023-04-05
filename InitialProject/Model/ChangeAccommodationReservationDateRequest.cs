using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Serializer;

namespace InitialProject.Model
{
    public class ChangeAccommodationReservationDateRequest : ISerializable
    {
        public int Id { get; set; }
        public AccommodationReservation Reservation { get; set; }   
        public DateTime NewArrivalDate { get; set; }
        public DateTime NewDepartureDate { get; set; }

        public String Reasons { get; set; }

        public ChangeAccommodationReservationDateRequest() {
            Reservation = new AccommodationReservation();
        }

        public ChangeAccommodationReservationDateRequest(AccommodationReservation Reservation, DateTime NewArrivalDate, DateTime NewDepartureDate, String ReasonsnDaysToCancel)
        {
            this.Reservation = Reservation;
            this.NewArrivalDate = NewArrivalDate;
            this.NewDepartureDate = NewDepartureDate;
            this.Reasons = Reasons;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Reservation = new AccommodationReservation();
            Reservation.Id = Convert.ToInt32(values[1]);
            NewArrivalDate = Convert.ToDateTime(values[2]);
            NewDepartureDate = Convert.ToDateTime(values[3]);
            Reasons = values[4];
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Reservation.Id.ToString(), NewArrivalDate.ToString(), NewDepartureDate.ToString(), Reasons};
            return csvValues;
        }
    }
}
