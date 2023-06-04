using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Domain.Model
{
    public class CommentReport : ISerializable
    {
        public ForumComment ForumComment { get; set; }
        public Owner Owner { get; set; }
        public CommentReport()
        {

        }

        public void FromCSV(string[] values)
        {
            ForumComment = new ForumComment();
            ForumComment.Id = Convert.ToInt32(values[0]);
            Owner = new Owner();
            Owner.Id = Convert.ToInt32(values[1]);
        }
        public string[] ToCSV()
        {
            string[] csvValues = { ForumComment.Id.ToString(), Owner.Id.ToString() };
            return csvValues;
        }
    }
}
