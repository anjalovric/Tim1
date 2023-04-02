using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class Guest2Repository
    {
        private const string FilePath = "../../../Resources/Data/guests2.csv";

        private readonly Serializer<Guest2> _serializer;

        private List<Guest2> _guests;

        public Guest2Repository()
        {
            _serializer = new Serializer<Guest2>();
            _guests = _serializer.FromCSV(FilePath);
        }

        public List<Guest2> GetAll()
        {
            return _guests;
        }
        public Guest2 GetByUsername(string userName)
        {
            return _guests.Find(n => n.Username.Equals(userName));
        }
        public Guest2 GetById(int id)
        {
            return _guests.Find(n => n.Id==id);
        }
    }
}
