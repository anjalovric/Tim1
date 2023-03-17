using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Model
{
    public class Guide:ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }


        public Guide() { }
        public Guide(string name, string lastName,string username)
        {
            Name = name;
            LastName = lastName;
            Username = username;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            LastName = values[2];
            Username = values[3];
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, LastName, Username };
            return csvValues;
        }
    }
}
