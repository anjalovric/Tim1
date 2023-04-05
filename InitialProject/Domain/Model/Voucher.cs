using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace InitialProject.Model
{
    public class Voucher:ISerializable
    {
        public int Id { get; set; }
        public  int GuestId{ get; set; }
        public  int GuideId{ get; set; }
        public bool Used { get; set; }
        public DateTime CreateDate { get; set; }
        public Voucher() { }
        public Voucher(bool used,int guestid,int guideid,DateTime createDate)
        {
            Used= used;
            GuestId= guestid;
            GuideId= guideid;
            CreateDate= createDate;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuestId.ToString(),GuideId.ToString(),Used.ToString(),CreateDate.ToString() };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            GuideId = Convert.ToInt32(values[2]);
            Used = Convert.ToBoolean(values[3]);
            CreateDate = Convert.ToDateTime(values[4]);
        }
    }
}
