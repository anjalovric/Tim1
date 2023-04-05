﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Serializer;

namespace InitialProject.Model
{
        public class Guest1 : ISerializable
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public string Username { get; set; }

            public Guest1() { }
            public Guest1(string name, string lastName)
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
            }

            public string[] ToCSV()
            {
                string[] csvValues = { Id.ToString(), Name, LastName, Username };
                return csvValues;
            }

        public override string ToString()
        {
            return Name + " " + LastName;
        }
    }
    
}