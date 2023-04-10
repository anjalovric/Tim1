using InitialProject.Domain.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace InitialProject.Model
{
    public class GuideAndTourReview:ISerializable
    {
        public int Id { get; set; }
        public int GuideId { get; set; }
        public Guest2 Guest2 { get; set; }
        public string Comment { get; set; }
        public int Language { get; set; }
        public int InterestingFacts { get; set; }
        public int Knowledge { get; set; }
        public TourInstance TourInstance { get; set; }

        public bool Valid { get; set; }

        public string ValidationUri { get; set; }   
        public BitmapImage ValidationImage { get; set; }
        public GuideAndTourReview()
        {
            TourInstance = new TourInstance();
            Valid = true;
            ValidationUri = "Resources/Images/greenCorect.png";
            ValidationImage = new BitmapImage(new Uri("/" + ValidationUri, UriKind.Relative));
        }
        public GuideAndTourReview(int guideId, Guest2 guest,TourInstance tourInstance, int language, int interestingFacts, int knowledge, String comment)
        {
            GuideId = guideId;
            Guest2 = guest;
            Comment = comment;
            TourInstance = tourInstance;
            Language = language;
            InterestingFacts = interestingFacts;
            Knowledge = knowledge;
            Comment = comment;
            Valid = true;
            ValidationUri = "Resources/Images/greenCorect.png";
            ValidationImage = new BitmapImage(new Uri("/" + ValidationUri, UriKind.Relative));
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuideId.ToString(), Guest2.Id.ToString(), Language.ToString(),InterestingFacts.ToString(),Knowledge.ToString(), Comment, TourInstance.Id.ToString(),Valid.ToString(),ValidationUri};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuideId = Convert.ToInt32(values[1]);
            Guest2 = new Guest2() { Id = Convert.ToInt32(values[2]) };
            Language = Convert.ToInt32(values[3]);
            InterestingFacts = Convert.ToInt32(values[4]);
            Knowledge = Convert.ToInt32(values[5]);
            Comment = values[6];
            TourInstance = new TourInstance();
            TourInstance.Id = Convert.ToInt32(values[7]);
            Valid = Convert.ToBoolean(values[8]);
            ValidationUri = values[9];
            ValidationImage =  new BitmapImage(new Uri("/" + ValidationUri, UriKind.Relative));
        }
    }
}
