using System;
using InitialProject.Serializer;

namespace InitialProject.Model
{
    public class Owner : User, ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public bool IsSuperOwner { get; set; }
        public String ImagePath { get; set; }

        public Owner() { }

        public Owner(string name, string lastName)
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
            Age = Convert.ToInt32(values[4]);
            Email = values[5];
            IsSuperOwner = Convert.ToBoolean(values[6]);
            ImagePath = values[7];
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, LastName, Username, Age.ToString(), Email, IsSuperOwner.ToString(), ImagePath };
            return csvValues;
        }
        public override string ToString()
        {
            return Name + " " + LastName;
        }
    }
}
