using System;
using InitialProject.Serializer;

namespace InitialProject.Model
{
    public enum State { Approved, Pending, Declined };
    public class ReschedulingAccommodationRequest : ISerializable
    {
        public int Id { get; set; }
        public AccommodationReservation Reservation { get; set; }
        public DateTime NewArrivalDate { get; set; }
        public DateTime NewDepartureDate { get; set; }
        public string Reasons { get; set; }
        public State state { get; set; }
        public string OwnerExplanationForDeclining { get; set; }
        public DateTime OldArrivalDate { get; set; }
        public DateTime OldDepartureDate { get; set; }

        public ReschedulingAccommodationRequest() {
            Reservation = new AccommodationReservation();
        }

        public ReschedulingAccommodationRequest(AccommodationReservation Reservation, DateTime NewArrivalDate, DateTime NewDepartureDate, string Reasons)
        {
            this.Reservation = Reservation;
            this.NewArrivalDate = NewArrivalDate;
            this.NewDepartureDate = NewDepartureDate;
            this.NewDepartureDate = this.NewDepartureDate.AddHours(23);
            this.NewDepartureDate = this.NewDepartureDate.AddMinutes(59);
            this.OldArrivalDate = Reservation.Arrival;
            this.OldDepartureDate = Reservation.Departure;
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
            string stateValue = values[5];
            if (stateValue == "Approved")
                state = State.Approved;
            else if (stateValue == "Pending")
                state = State.Pending;
            else
                state = State.Declined;
            OldArrivalDate = Convert.ToDateTime(values[6]);
            OldDepartureDate = Convert.ToDateTime(values[7]);

            if (state == State.Declined)
                OwnerExplanationForDeclining = values[8];
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Reservation.Id.ToString(), NewArrivalDate.ToString(), NewDepartureDate.ToString(), Reasons, state.ToString(),OldArrivalDate.ToString(), OldDepartureDate.ToString(), OwnerExplanationForDeclining};
            return csvValues;
        }
    }
}
