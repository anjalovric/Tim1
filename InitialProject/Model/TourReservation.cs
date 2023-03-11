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
        public int TourId { get; set; }
        public int TourInstanceId { get; set; }
        public int CurrentGuestsNumber { get; set; }
        public TourReservation() { }

        public TourReservation(int tourId,int tourInstanceId, int currentGuestsNumber)
        {
            TourId = tourId;
            TourInstanceId = tourInstanceId;
            CurrentGuestsNumber = currentGuestsNumber;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourId = Convert.ToInt32(values[1]);
            TourInstanceId = Convert.ToInt32(values[2]);
            CurrentGuestsNumber = Convert.ToInt32(values[3]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TourId.ToString(),TourInstanceId.ToString(),CurrentGuestsNumber.ToString()};
            return csvValues;
        }
    }
}

