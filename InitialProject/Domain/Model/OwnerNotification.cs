using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Domain.Model
{
    public enum OwnerNotificationType { ACCOMMODATION_ADDED, ACCOMMODATION_DELETED, SUPEROWNER, RENOVATION_SCHEDULED, RENOVATION_CANCELLED, FORUM_ADDED, COMMENT_ADDED, COMMENT_REPORTED, REQUEST_ACCEPPTED, REQUEST_DECLINED, GUEST_REVIEWED }
    public class OwnerNotification : ISerializable
    {
        public int Id { get; set; }
        public Owner Owner { get; set; }
        public OwnerNotificationType Type { get; set; }
        public OwnerNotification() { }
        public OwnerNotification(int id, Owner owner, OwnerNotificationType type)
        {
            Id = id;
            Owner = owner;
            Type = type;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Owner = new Owner();
            Owner.Id = Convert.ToInt32(values[1]);
            Type = (OwnerNotificationType)Enum.Parse(typeof(OwnerNotificationType), values[2]);
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Owner.Id.ToString(), Type.ToString()};
            return csvValues;
        }
    }
}
