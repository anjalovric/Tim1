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
        public TourInstance TourInstance { get; set; }
        public string RelativeUri { get; set; }
        public TourReviewImage()
        {
            //TourInstance = new TourInstance();
        }
        public TourReviewImage(TourInstance tourInstance, string relativeUri)
        {
            TourInstance = tourInstance;
            RelativeUri= relativeUri;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourInstance = new TourInstance();
            TourInstance.Id= Convert.ToInt32(values[1]);
            RelativeUri= values[2];
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TourInstance.Id.ToString(), RelativeUri};
            return csvValues;
        }

    }
}
