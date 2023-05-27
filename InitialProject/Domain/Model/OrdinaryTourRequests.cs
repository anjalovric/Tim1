using InitialProject.Model;
using InitialProject.Serializer;
using System;

namespace InitialProject.Domain.Model
{
    public enum Status { ONWAITING,ACCEPTED, INVALID}
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
        public Status Status { get; set; }
        public string Start { get;set; }
        public string End { get; set; }  
        public int GuideId { get; set; }
        public DateTime CreateDate { get; set; }
        public int TourInstanceId { get; set; }
        public bool NewAccepted { get; set; } 
        public bool IsNew { get; set; }
        public int ComplexId { get; set; }
        public OrdinaryTourRequests()
        {

        }
        public OrdinaryTourRequests(string name,int guestId, int maxGuests, Location location, string description, string language, DateTime startDate, DateTime endDate, Status status, string start, string end, int guideId, DateTime createDate, bool newAccepted, int instanceId, int complexId)
        {
            Name = name;
            GuestId = guestId;
            MaxGuests = maxGuests;
            Location = location;
            Description = description;
            Language = language;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            Start = start;
            End = end;
            GuideId = guideId;
            CreateDate = createDate;
            NewAccepted = newAccepted;
            TourInstanceId = instanceId;
            IsNew = false;
            ComplexId = complexId;
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
            Status = (Status)Enum.Parse(typeof(Status), values[9]);
            Start = values[10];
            End = values[11];
            GuideId= Convert.ToInt32(values[12]);
            CreateDate = Convert.ToDateTime(values[13]);
            NewAccepted= Convert.ToBoolean(values[14]);
            TourInstanceId = Convert.ToInt32(values[15]);
            ComplexId = Convert.ToInt32(values[16]);
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(),Name,GuestId.ToString(), MaxGuests.ToString(), Location.Id.ToString(), Description, Language, StartDate.ToString(),EndDate.ToString(),Status.ToString(),Start,End,GuideId.ToString(),CreateDate.ToString(),NewAccepted.ToString(), TourInstanceId.ToString(),ComplexId.ToString()};
            return csvValues;
        }
    }
}
