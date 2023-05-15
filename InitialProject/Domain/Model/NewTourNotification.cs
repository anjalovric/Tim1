using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Serializer;
using NPOI.SS.Formula.PTG;

namespace InitialProject.Domain.Model
{
    public enum Guest2NotificationType { REQUEST_ACCEPTED, CONFIRM_PRESENCE }
    public class NewTourNotification:ISerializable
    {
        public int Id { get; set; }
        public Guest2 Guest2 { get; set; }
        public String Text { get; set; }
        public Guest2NotificationType Type { get; set; }
        public TourInstance TourInstance { get; set; }
        public int AlertGuest2Id { get; set; }
        public bool Deleted { get; set; }
        public NewTourNotification() { }
        public NewTourNotification(Guest2 guest2,string text, Guest2NotificationType type,TourInstance tourInstance,bool deleted, int alertId)
        {
      
            Guest2 = guest2;
            Text = text;
            Type = type;
            TourInstance = tourInstance;   
            Deleted = deleted;
            AlertGuest2Id = alertId;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Guest2 = new Guest2();
            Guest2.Id = Convert.ToInt32(values[1]);
            Text = values[2];
            Type = (Guest2NotificationType)Enum.Parse(typeof(Guest2NotificationType), values[3]);
            TourInstance = new TourInstance();
            TourInstance.Id = Convert.ToInt32(values[4]);
            Deleted = Convert.ToBoolean(values[5]);
            AlertGuest2Id = Convert.ToInt32(values[6]);
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Guest2.Id.ToString(),Text, Type.ToString(),TourInstance.Id.ToString(), Deleted.ToString(), AlertGuest2Id.ToString()};
            return csvValues;
        }
    }
}
