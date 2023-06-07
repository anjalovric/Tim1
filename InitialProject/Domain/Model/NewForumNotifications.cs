using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Domain.Model
{
    public class NewForumNotification : ISerializable
    {
        public Owner Owner { get; set; }
        public Location Location { get; set; }
        public NewForumNotification()
        {

        }

        public string[] ToCSV()
        {
            string[] csvValues = {Owner.Id.ToString(), Location.Id.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Owner = new Owner();
            Owner.Id = Convert.ToInt32(values[0]);
            Location = new Location();
            Location.Id = Convert.ToInt32(values[1]);
        }
    }
}
