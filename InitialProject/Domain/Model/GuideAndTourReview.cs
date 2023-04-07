using InitialProject.Domain.Model;
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
        public TourInstance TourInstance { get; set; }
        public GuideAndTourReview()
        {
            TourInstance = new TourInstance();
        }
        public GuideAndTourReview(int guideId, int guestId,TourInstance tourInstance, int language, int interestingFacts, int knowledge, String comment)
        {
            GuideId = guideId;
            GuestId = guestId;
            Comment = comment;
            TourInstance = tourInstance;
            Language = language;
            InterestingFacts = interestingFacts;
            Knowledge = knowledge;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuideId.ToString(), GuestId.ToString(), Language.ToString(),InterestingFacts.ToString(),Knowledge.ToString(), Comment, TourInstance.Id.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuideId = Convert.ToInt32(values[1]);
            GuestId = Convert.ToInt32(values[2]);
            Language= Convert.ToInt32(values[3]);
            InterestingFacts = Convert.ToInt32(values[4]);
            Knowledge = Convert.ToInt32(values[5]);
            Comment = values[6];
            TourInstance = new TourInstance();
            TourInstance.Id = Convert.ToInt32(values[7]);
        }
    }
}
