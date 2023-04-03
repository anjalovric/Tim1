using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class GuideAndTourReview:ISerializable
    {
        public int Id { get; set; }
        public int GuideId { get; set; }
        public int GuestId { get; set; }
        public string Comment { get; set; }
        public TourReservation Reservation { get; set; }
        public GuideAndTourReview()
        {
            Reservation = new TourReservation();
        }
        public GuideAndTourReview(int guideId, int guestId, String comment,TourReservation tourReservation)
        {
            GuideId= guideId;
            GuestId= guestId;
            Comment= comment;
            Reservation = tourReservation;
            //Reservation = new TourReservation();
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuideId.ToString(), GuestId.ToString(), Comment, Reservation.Id.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuideId = Convert.ToInt32(values[1]);
            GuestId = Convert.ToInt32(values[2]);
            Comment = values[3];
            Reservation = new TourReservation();
            Reservation.Id = Convert.ToInt32(values[4]);
        }
    }
}
