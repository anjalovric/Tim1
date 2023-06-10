using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class Guide: User, ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public Boolean Active { get; set; }
        public Boolean IsSuperGuide { get; set; }

        public Guide() { }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            LastName = values[2];
            Username = values[3];
            Email= values[4];
            Age = Convert.ToInt32(values[5]);
            Active = Convert.ToBoolean(values[6]);
            IsSuperGuide = Convert.ToBoolean(values[7]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, LastName, Username,Email,Age.ToString(),Active.ToString(), IsSuperGuide.ToString()};
            return csvValues;
        }
        public string ToString()
        {
            return Name + " " + LastName;
        }
    }
}
