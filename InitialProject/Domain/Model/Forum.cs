using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;

using InitialProject.Serializer;

namespace InitialProject.Domain.Model
{
    public class Forum : ISerializable
    {
        public int Id { get; set; }

        public Location Location { get; set; }

        public Guest1 Guest1 { get; set; }
        public bool Opened { get; set; }
        public int CommentsNumber { get; set; }
        public bool IsNewForOwner { get; set; }

        public bool IsVeryUseful { get; set; }
        public Forum() { }
        public Forum(Location location, Guest1 guest1)
        {
            Location = location;
            Guest1 = guest1;
            Opened = true;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Location = new Location();
            Location.Id = Convert.ToInt32(values[1]);
            Guest1 = new Guest1();
            Guest1.Id = Convert.ToInt32(values[2]);
            Opened = Convert.ToBoolean(values[3]);
            CommentsNumber = Convert.ToInt32(values[4]);
            IsNewForOwner = Convert.ToBoolean(values[5]);
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Location.Id.ToString(), Guest1.Id.ToString(), Opened.ToString(), CommentsNumber.ToString(), IsNewForOwner.ToString() };
            return csvValues;
        }
    }
}
