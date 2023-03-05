using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace InitialProject.Model
{
    public class Tour : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int MaxGuests { get; set; }

        public double Duration { get; set; }

        public Location Location { get; set; }

        public int LocationId { get; set; }

        public List<DateTime> StartDates { get; set; }

        public bool Started { get; set; }

        public bool Finished { get; set; }

        public string Description { get; set; }

        public List<string> Languages { get; set; }

        public List<int> CheckPointIds { get; set; }

        public Tour()
        {
            CheckPointIds = new List<int>();
            StartDates = new List<DateTime>();
            Languages= new List<string>();

        }



       

        public void FromCSV(string[] values)
        {
            throw new NotImplementedException();
        }

        public string[] ToCSV()
        {
            throw new NotImplementedException();
        }
    }
}
