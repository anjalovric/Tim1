using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Serializer;
using NPOI.SS.Formula.Functions;

namespace InitialProject.Model
{
    public class ChangeAccommodationReservationDateRequest : ISerializable
    {
        public int Id { get; set; }
        public AccommodationReservation Reservation { get; set; }   
        public DateTime NewArrivalDate { get; set; }
        public DateTime NewDepartureDate { get; set; }

        public String Reasons { get; set; }

        public enum State { Approved, Pending, Declined};

        public State state { get; set; }

        public ChangeAccommodationReservationDateRequest() {
            Reservation = new AccommodationReservation();
        }

        public ChangeAccommodationReservationDateRequest(AccommodationReservation Reservation, DateTime NewArrivalDate, DateTime NewDepartureDate, String Reasons)
        {
            this.Reservation = Reservation;
            this.NewArrivalDate = NewArrivalDate;
            this.NewDepartureDate = NewDepartureDate;
            this.NewDepartureDate = this.NewDepartureDate.AddHours(23);
            this.NewDepartureDate = this.NewDepartureDate.AddMinutes(59);
            this.Reasons = Reasons;
            this.state = State.Pending;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Reservation = new AccommodationReservation();
            Reservation.Id = Convert.ToInt32(values[1]);
            NewArrivalDate = Convert.ToDateTime(values[2]);
            NewDepartureDate = Convert.ToDateTime(values[3]);
            Reasons = values[4];
            String stateValue = values[5];
            if (stateValue == "Approved")
                state = State.Approved;
            else if (stateValue == "Pending")
                state = State.Pending;
            else
                state = State.Declined;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Reservation.Id.ToString(), NewArrivalDate.ToString(), NewDepartureDate.ToString(), Reasons, state.ToString()};
            return csvValues;
        }
    }
}
