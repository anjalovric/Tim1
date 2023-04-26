using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Domain.Model
{
    public class SuperGuestTitle : ISerializable
    {
        public int Id { get; set; }
        public Guest1 Guest { get; set; }
        public int AvailablePoints {get;set;}
        public DateTime ActivationDate { get; set; }

        public SuperGuestTitle()
        {
            Guest = new Guest1();
        }
        public SuperGuestTitle(Guest1 guest, int availablePoints, DateTime activationDate)
        {
            Guest = guest;
            AvailablePoints = availablePoints;
            ActivationDate = activationDate;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Guest = new Guest1();
            Guest.Id = Convert.ToInt32(values[1]);
            AvailablePoints = Convert.ToInt32(values[2]);
            ActivationDate = Convert.ToDateTime(values[3]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Guest.Id.ToString(),AvailablePoints.ToString(), ActivationDate.ToString()};
            return csvValues;
        }
    }
}
