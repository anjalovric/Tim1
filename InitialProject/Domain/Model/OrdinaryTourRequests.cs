using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using System.Windows;

namespace InitialProject.Domain.Model
{
    public class OrdinaryTourRequests:ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GuestId { get; set; }
        public int MaxGuests { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Boolean Informed { get; set; }
        public string Status { get; set; }
        public OrdinaryTourRequests()
        {

        }
        public OrdinaryTourRequests(string name,int guestId,int maxGuests, Location location, string description, string language,DateTime startDate, DateTime endDate, bool informed,string status)
        {
            Name = name;
            GuestId = guestId;
            MaxGuests = maxGuests;
            Location = location;
            Description = description;
            Language = language;
            StartDate = startDate;
            EndDate = endDate;
            Informed = informed;
            Status = status;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            GuestId = Convert.ToInt32(values[2]);
            MaxGuests = Convert.ToInt32(values[3]);
            Location = new Location() { Id = Convert.ToInt32(values[4]) };
            Description = values[5];
            Language = values[6];
            StartDate = Convert.ToDateTime(values[7]);
            EndDate = Convert.ToDateTime(values[8]);
            Informed = Convert.ToBoolean(values[9]);
            Status= values[10];
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(),Name,GuestId.ToString(), MaxGuests.ToString(), Location.Id.ToString(), Description, Language, StartDate.ToString(),EndDate.ToString(),Informed.ToString(),Status};
            return csvValues;
        }
    }
}
