using InitialProject.Model;
using InitialProject.Serializer;
using System;

namespace InitialProject.Domain.Model
{
    public enum Type { ONWAITING, ACCEPTED, INVALID }
    public class ComplexTourRequests : ISerializable
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public Guest2 Guest2 { get; set; }
        public Type Status { get; set; }

        public ComplexTourRequests()
        {

        }
        public ComplexTourRequests(string name, Guest2 guest2, Type status)
        {
            Name = name;
            Guest2 = guest2;
            Status = status;
           
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Guest2 = new Guest2();
            Guest2.Id= Convert.ToInt32(values[2]);
            Status = (Type)Enum.Parse(typeof(Type), values[3]);
            
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, Guest2.Id.ToString(), Status.ToString() };
            return csvValues;
        }
    }
}
