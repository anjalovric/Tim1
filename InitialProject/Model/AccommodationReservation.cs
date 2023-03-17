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
        public DateTime ComingDate  { get; set; }
        public DateTime LeavingDate { get; set; }

        public AccommodationReservation() {
            Guest = new Guest1();
            Accommodation = new Accommodation();
        }
        public AccommodationReservation(Guest1 guest, Accommodation currentAccommodation, DateTime comingDate, DateTime leavingDate)
        {
            Guest = guest;
            this.Accommodation = currentAccommodation;
            ComingDate = comingDate;
            LeavingDate = leavingDate;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Guest = new Guest1();
            Guest.Id = Convert.ToInt32(values[1]);
            Accommodation.Id = Convert.ToInt32(values[2]);
            ComingDate = Convert.ToDateTime(values[3]);
            LeavingDate = Convert.ToDateTime(values[4]);


        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Guest.Id.ToString(), Accommodation.Id.ToString(), ComingDate.ToString(), LeavingDate.ToString()};
            return csvValues;
        }
    }
}
