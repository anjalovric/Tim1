using InitialProject.Serializer;
using System;

namespace InitialProject.Model
{
    public class Comment : ISerializable
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string Text { get; set; }
        public Guest1 Guest { get; set; }

        public Comment() { }

        public Comment(DateTime creationTime, string text, Guest1 guest)
        {
            CreationTime = creationTime;
            Text = text;
            Guest = guest;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), CreationTime.ToString(), Text, Guest.Id.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            CreationTime = Convert.ToDateTime(values[1]);
            Text = values[2];
            Guest = new Guest1() { Id = Convert.ToInt32(values[3]) };
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
