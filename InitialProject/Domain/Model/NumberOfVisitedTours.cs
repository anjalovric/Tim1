using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.Model
{
    public class NumberOfVisitedTours:ISerializable
    {
        public int Id { get; set; }
        public Guest2 Guest2 { get; set; }
        public int Count { get; set; }
        public int Year { get; set; }
        public NumberOfVisitedTours() { }
        public NumberOfVisitedTours(Guest2 guest2,int count, int year)
        {
            Guest2 = guest2;
            Count = count;
            Year = year;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(),Guest2.Id.ToString(), Count.ToString(), Year.ToString() };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Guest2 = new Guest2() { Id = Convert.ToInt32(values[1]) };
            Count = Convert.ToInt32(values[2]);
            Year = Convert.ToInt32(values[3]);
        }
    }
}
