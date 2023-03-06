using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;

namespace InitialProject.Model
{
    public class Tour : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int MaxGuests { get; set; }

        public double Duration { get; set; }

        public Location Location { get; set; }

        public DateTime Start { get; set; }

        public bool Finished { get; set; }

        public string Description { get; set; }

        public string Language { get; set; }

        public Tour() 
        {
            Finished = false;
        }

        public Tour(string name, int maxGuests, double duration, Location location, DateTime start, bool finished, string description, string language)
        {
            Name=name;
            MaxGuests=maxGuests;
            Duration=duration;
            Location=location;
            Start=start;
            Finished=finished;
            Description=description;
            Language=language;

        }


        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            MaxGuests = Convert.ToInt32(values[2]);
            Duration = Convert.ToDouble(values[3]);
            Location = new Location() { Id = Convert.ToInt32(values[4]) };
            Start = Convert.ToDateTime(values[5]);
            Finished= Convert.ToBoolean(values[6]);
            Description = values[7];
            Language = values[8];

        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, MaxGuests.ToString(),Duration.ToString(), Location.Id.ToString(),Start.ToString(),Finished.ToString(),Description,Language };
            return csvValues;
        }
    }
}
