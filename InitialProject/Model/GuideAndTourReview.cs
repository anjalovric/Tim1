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
        public int Language { get; set; }
        public int InterestingFacts { get; set; }
        public int Knowledge { get; set; }
        public TourReservation Reservation { get; set; }
        public GuideAndTourReview()
        {
            Reservation = new TourReservation();
        }
        public GuideAndTourReview(int guideId, int guestId,TourReservation tourReservation, int language, int interestingFacts, int knowledge, String comment)
        {
            GuideId = guideId;
            GuestId = guestId;
            Comment = comment;
            Reservation = tourReservation;
            Language = language;
            InterestingFacts = interestingFacts;
            Knowledge = knowledge;
            //Reservation = new TourReservation();
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuideId.ToString(), GuestId.ToString(), Reservation.Id.ToString(),Language.ToString(),InterestingFacts.ToString(),Knowledge.ToString(), Comment };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuideId = Convert.ToInt32(values[1]);
            GuestId = Convert.ToInt32(values[2]);
            Reservation = new TourReservation();
            Reservation.Id = Convert.ToInt32(values[3]);
            Language= Convert.ToInt32(values[4]);
            InterestingFacts = Convert.ToInt32(values[5]);
            Knowledge = Convert.ToInt32(values[6]);
            Comment = values[7];
        }
    }
}
