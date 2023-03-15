using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Serializer;

namespace InitialProject.Model
{
    public class GuestReview : ISerializable
    {
        public int Id { get; set; }
        public Guest1 Guest { get; set; }
        public int Cleanliness { get; set; }
        public int RulesFollowing { get; set; }
        public string Comment { get; set; }
        public Owner Owner { get; set; }

        public GuestReview()
        {
            Guest = new Guest1();
            Owner = new Owner();
        }

        public GuestReview(int cleanliness, int rulesFollowing)
        {
            Cleanliness = cleanliness;
            RulesFollowing = rulesFollowing;
            Guest = new Guest1();
            Owner = new Owner();
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Guest.Id.ToString(), Cleanliness.ToString(), RulesFollowing.ToString(), Comment, Owner.Id.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Guest.Id = Convert.ToInt32(values[1]);
            Cleanliness = Convert.ToInt32(values[2]);
            RulesFollowing = Convert.ToInt32(values[3]);
            Comment = values[4];
            Owner = new Owner();
            Owner.Id = Convert.ToInt32(values[5]);
        }
    }
}
