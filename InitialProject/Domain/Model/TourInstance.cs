using InitialProject.Serializer;
using System;
using System.Windows.Media.Imaging;

namespace InitialProject.Model
{
    public class TourInstance:ISerializable
    {
        public int Id { get; set; }

        public Tour Tour { get; set; }

        public DateTime StartDate { get; set; }

        public bool Finished { get; set; }

        public Guide Guide{ get; set; }

        public double Attendance { get;set; }

        public string CoverImage { get; set; }

        public BitmapImage CoverBitmap { get; set; }

        public bool Active { get; set; }    

        public string Date { get;set; }

        public bool Canceled { get; set; }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Tour = new Tour() { Id = Convert.ToInt32(values[1]) };
            StartDate = Convert.ToDateTime(values[2]);
            Finished = Convert.ToBoolean(values[3]);
            Guide = new Guide() { Id=Convert.ToInt32(values[4]) };
            Attendance = Convert.ToDouble(values[5]);
            CoverImage = values[6];
            CoverBitmap= new BitmapImage(new Uri("/" + CoverImage, UriKind.Relative));
            Active= Convert.ToBoolean(values[8]);
            Date = values[9];
            Canceled = Convert.ToBoolean(values[10]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Tour.Id.ToString(), StartDate.ToString(),Finished.ToString(),Guide.Id.ToString(),Attendance.ToString(),CoverImage,CoverBitmap.ToString(),Active.ToString(),Date,Canceled.ToString()};
            return csvValues;
        }
        public TourInstance() 
        {
            Finished = false;
            Active = false;
            Canceled = false;        
        }
        public TourInstance( Tour tour, DateTime startDate,Guide guide,string coverImage)
        {
            Tour = tour;
            StartDate = startDate;
            Finished = false;
            Guide = guide;
            Attendance = 0;
            CoverImage = coverImage;
            CoverBitmap = new BitmapImage(new Uri("/" + CoverImage, UriKind.Relative));
            Active = false;
            Canceled= false;
        }
        public override string ToString()
        {
            return StartDate.ToString();
        }
    }
}
