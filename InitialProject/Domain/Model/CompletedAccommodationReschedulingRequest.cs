using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Domain.Model
{
    public class CompletedAccommodationReschedulingRequest : ISerializable
    {
        public int Id { get; set; }
        public ReschedulingAccommodationRequest Request { get; set; }
       
        public CompletedAccommodationReschedulingRequest()
        {
            Request = new ReschedulingAccommodationRequest();
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Request = new ReschedulingAccommodationRequest();
            Request.Id= Convert.ToInt32(values[1]);
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Request.Id.ToString()};
            return csvValues;
        }
    }
}
