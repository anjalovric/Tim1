using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Repository
{
    public class Guest1Repository : IGuest1Repository
    {
        private const string FilePath = "../../../Resources/Data/guests1.csv";

        private readonly Serializer<Guest1> _serializer;

        private List<Guest1> _guests;

        public Guest1Repository()
        {
            _serializer = new Serializer<Guest1>();
            _guests = _serializer.FromCSV(FilePath);
        }

        public List<Guest1> GetAll()
        {
            return _guests;
        }

        public Guest1 GetByUsername(string userName)
        {
            return _guests.Find(n => n.Username.Equals(userName));
        }

        

        public Guest1 GetById(int id)
        {
            return _guests.Find(n => n.Id == id);
        }

        public int NextId()
        {
            _guests = _serializer.FromCSV(FilePath);
            if (_guests.Count < 1)
            {
                return 1;
            }
            return _guests.Max(c => c.Id) + 1;
        }
        public void Update(Guest1 guest1)
        {
            _guests = _serializer.FromCSV(FilePath);
            Guest1 current = _guests.Find(c => c.Id == guest1.Id);
            int index = _guests.IndexOf(current);
            _guests.Remove(current);
            _guests.Insert(index, guest1);
            _serializer.ToCSV(FilePath, _guests);
        }
       

    }
}
