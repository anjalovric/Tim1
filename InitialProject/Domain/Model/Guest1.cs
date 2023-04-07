using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Serializer;

namespace InitialProject.Model
{
   public class Guest1 : ISerializable
   {
       public int Id { get; set; }
       public string Name { get; set; }
       public string LastName { get; set; }
       public string Username { get; set; }

       public string PhoneNumber { get; set; }
       public Location Location { get; set; }
       public string Email { get; set; }
       public int ReviewsNumber { get; set; }
       public bool IsSuperGuest { get; set; }

       public string ImagePath { get; set; }


        public Guest1() { }
       public Guest1(string name, string lastName)
       {
           Name = name;
           LastName = lastName;
       }
       public void FromCSV(string[] values)
       {
           Id = Convert.ToInt32(values[0]);
           Name = values[1];
           LastName = values[2];
           Username = values[3];
           PhoneNumber = values[4];
           Location = new Location();
           Location.Id = Convert.ToInt32(values[5]);
           Email = values[6];
           ReviewsNumber = Convert.ToInt32(values[7]);
           IsSuperGuest = Convert.ToBoolean(values[8]);
           ImagePath = values[9];

       }

       public string[] ToCSV()
       {
           string[] csvValues = { Id.ToString(), Name, LastName, Username, PhoneNumber, Location.Id.ToString(), Email, ReviewsNumber.ToString(), IsSuperGuest.ToString(), ImagePath };
           return csvValues;
       }

        public override string ToString()
        {
            return Name + " " + LastName;
        }
    }
    
}
