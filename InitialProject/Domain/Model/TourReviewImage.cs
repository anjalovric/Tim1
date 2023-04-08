using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Model
{
    public class TourReviewImage:ISerializable
    {
        public int Id { get; set; }
        public int GuideAndTourReviewId { get; set; }
        public string RelativeUri { get; set; }
        public TourReviewImage()
        {
        }
        public TourReviewImage(int guideAndTourReviewId, string relativeUri)
        {
            GuideAndTourReviewId = guideAndTourReviewId;
            RelativeUri= relativeUri;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuideAndTourReviewId= Convert.ToInt32(values[1]);
            RelativeUri= values[2];
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuideAndTourReviewId.ToString(), RelativeUri};
            return csvValues;
        }

    }
}
