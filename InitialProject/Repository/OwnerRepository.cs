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
    public class OwnerRepository : IOwnerRepository
    {
        private const string FilePath = "../../../Resources/Data/owners.csv";

        private readonly Serializer<Owner> _serializer;

        private List<Owner> _owners;

        public OwnerRepository()
        {
            _serializer = new Serializer<Owner>();
            _owners = _serializer.FromCSV(FilePath);
        }

        public List<Owner> GetAll()
        {
            return _owners;
        }

        public Owner GetByUsername(string username)
        {
            return _owners.Find(n => n.Username.Equals(username));
        }

        public Owner GetById(int id)
        {
            return _owners.Find(n => n.Id == id);
        }

        public int NextId()
        {
            _owners = _serializer.FromCSV(FilePath);
            if (_owners.Count < 1)
            {
                return 1;
            }
            return _owners.Max(c => c.Id) + 1;
        }

        public void Update(Owner owner)
        {
            _owners = _serializer.FromCSV(FilePath);
            Owner current = _owners.Find(c => c.Id == owner.Id);
            int index = _owners.IndexOf(current);
            _owners.Remove(current);
            _owners.Insert(index, owner); 
            _serializer.ToCSV(FilePath, _owners);
        }
        public void Add(Owner owner)
        {
            owner.Id = NextId();
            _owners.Add(owner);
            _serializer.ToCSV(FilePath, _owners);
        }
    }
}
