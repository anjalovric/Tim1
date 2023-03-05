using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace InitialProject.Model
{
    public class CheckPoint : ISerializable
    {
        public int Id { get; set; } 
        public string Name { get; set; }

        public bool Checked { get; set; }

        public CheckPoint() { }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Checked = Convert.ToBoolean(values[2]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, Checked.ToString() };
            return csvValues;
        }
    }
}
