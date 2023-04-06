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
        public ChangeAccommodationReservationDateRequest Request { get; set; }
        public ChangeAccommodationReservationDateRequest.State State { get; set; }
        public string OwnersExplanation { get; set; }
       
        public CompletedAccommodationReschedulingRequest()
        {
            Request = new ChangeAccommodationReservationDateRequest();
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Request = new ChangeAccommodationReservationDateRequest();
            Request.Id= Convert.ToInt32(values[1]);
            if (values[2].Equals("Approved"))
                State = ChangeAccommodationReservationDateRequest.State.Approved;
            else if (values[2].Equals("Declined"))
                State = ChangeAccommodationReservationDateRequest.State.Declined;
            if (State == ChangeAccommodationReservationDateRequest.State.Declined)
                OwnersExplanation = values[3];
           
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Request.Id.ToString(), State.ToString(), OwnersExplanation};
            return csvValues;
        }
    }
}
