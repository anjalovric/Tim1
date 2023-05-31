using InitialProject.Domain.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace InitialProject.Model
{
    public enum VoucherType { CANCELED_TOUR, VISITED_TOUR, DISMISSAL_GUIDE }
    public class Voucher:ISerializable
    {
        public int Id { get; set; }
        public  int GuestId{ get; set; }
        public  int GuideId{ get; set; }
        public bool Used { get; set; }
        public DateTime CreateDate { get; set; }
        public VoucherType Type { get; set; }
        public Voucher() { }
        public Voucher(bool used,int guestid,int guideid,DateTime createDate, VoucherType type)
        {
            Used= used;
            GuestId= guestid;
            GuideId= guideid;
            CreateDate= createDate;
            Type= type;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuestId.ToString(),GuideId.ToString(),Used.ToString(),CreateDate.ToString(), Type.ToString() };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            GuideId = Convert.ToInt32(values[2]);
            Used = Convert.ToBoolean(values[3]);
            CreateDate = Convert.ToDateTime(values[4]);
            Type = (VoucherType)Enum.Parse(typeof(VoucherType), values[5]);
        }
    }
}
