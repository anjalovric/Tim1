using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Serializer;
namespace InitialProject.Model
{
    public class TourReservation:ISerializable
    {
        public int Id { get; set; } 
        public int TourInstanceId { get; set; }
        public int CurrentGuestsNumber { get; set; }
        public int GuestId { get; set; }
        public double AverageGuestsAge { get; set; }
        public int Capacity { get; set; }  
        public bool WithVaucher { get; set; }
        public TourReservation() { }
        public TourReservation(int tourInstanceId, int currentGuestsNumber, int guestId, double averageGuestsAge, int capacity, bool withVaucher)
        {
            TourInstanceId = tourInstanceId;
            CurrentGuestsNumber = currentGuestsNumber;
            GuestId = guestId;
            AverageGuestsAge = averageGuestsAge;
            Capacity = capacity;
            WithVaucher = withVaucher;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourInstanceId = Convert.ToInt32(values[1]);
            CurrentGuestsNumber = Convert.ToInt32(values[2]);
            GuestId = Convert.ToInt32(values[3]);
            AverageGuestsAge = Convert.ToDouble(values[4]);
            Capacity = Convert.ToInt32(values[5]);
            WithVaucher = Convert.ToBoolean(values[6]);
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(),TourInstanceId.ToString(),CurrentGuestsNumber.ToString(),GuestId.ToString(),AverageGuestsAge.ToString(),Capacity.ToString(),WithVaucher.ToString()};
            return csvValues;
        }
    }
}

