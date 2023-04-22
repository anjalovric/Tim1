using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Domain.Model
{
    public enum OwnerNotificationType { ACCOMMODATION_ADDED, ACCOMMODATION_DELETED, SUPEROWNER }
    public class OwnerNotification : ISerializable
    {
        public int Id { get; set; }
        
        public OwnerNotificationType Type { get; set; }
        public OwnerNotification() { }
        public OwnerNotification(int id, OwnerNotificationType type)
        {
            Id = id;
            Type = type;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Type = (OwnerNotificationType)Enum.Parse(typeof(OwnerNotificationType), values[1]);
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Type.ToString()};
            return csvValues;
        }
    }
}
