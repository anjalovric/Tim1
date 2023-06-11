using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Domain.Model
{
    public class ForumComment : ISerializable
    {
        public int Id { get; set; }
        public Forum Forum { get; set; }
        public String Text { get; set; }
        public User User { get; set; }
        public DateTime CreatingDate { get; set; }
        public bool WasOnLocation { get; set; }
        public int ReportsNumber { get; set; }
        public bool IsOwnerComment { get; set; }
        public bool IsAlreadyReportedByThisOwner { get; set; }


        public ForumComment() { }
        public ForumComment(Forum forum, User user, DateTime creatingDate, string text)
        {
            Forum = forum;
            User = user;
            CreatingDate = creatingDate;
            Text = text;
            ReportsNumber = 0;
         }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Forum = new Forum();
            Forum.Id = Convert.ToInt32(values[1]);
            User = new User();
            User.Username = values[2];
            CreatingDate = Convert.ToDateTime(values[3]);
            Text = values[4];
            WasOnLocation = Convert.ToBoolean(values[5]);
            ReportsNumber = Convert.ToInt32(values[6]);
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Forum.Id.ToString(), User.Username, CreatingDate.ToString(), Text, WasOnLocation.ToString(), ReportsNumber.ToString() };
            return csvValues;
        }


    }
}
