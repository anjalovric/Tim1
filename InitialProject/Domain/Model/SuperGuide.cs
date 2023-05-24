﻿using InitialProject.Serializer;
using System;

namespace InitialProject.Domain.Model
{
    public class SuperGuide: ISerializable
    {
        public int Id { get; set; }    
        public string Language { get; set; }
        public int guideId { get; set; }

        public SuperGuide() { }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Language = values[1];
            guideId = Convert.ToInt32(values[2]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Language, guideId.ToString() };
            return csvValues;
        }

    }
}
