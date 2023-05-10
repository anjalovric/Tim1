using InitialProject.Serializer;
using System;

namespace InitialProject.Domain.Model
{
    public class GuideAndTourReviewNotification: ISerializable
    {
        public int Id { get; set; }
        public int GuideAndTourReviewId { get; set; }
        public int GuideId { get; set; }
        public int Count { get;set; }

        public GuideAndTourReviewNotification(int reviewId, int guideId)
        {
            GuideAndTourReviewId = reviewId;
            GuideId = guideId;
            Count = 0;
        }
        public GuideAndTourReviewNotification() 
        {
        Count=0;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuideAndTourReviewId.ToString(), GuideId.ToString(), Count.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuideAndTourReviewId = Convert.ToInt32((string)values[1]);
            GuideId = Convert.ToInt32((string)values[2]);
            Count = Convert.ToInt32((string)values[3]);
        }
    }
}
