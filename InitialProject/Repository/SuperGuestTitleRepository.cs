using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Repository
{
    public class SuperGuestTitleRepository : ISuperGuestTitleRepository
    {
        private const string FilePath = "../../../Resources/Data/superGuests.csv";

        private readonly Serializer<SuperGuestTitle> _serializer;

        private List<SuperGuestTitle> _superGuests;

        public SuperGuestTitleRepository()
        {
            _serializer = new Serializer<SuperGuestTitle>();
            _superGuests = _serializer.FromCSV(FilePath);
        }
        public void Add(SuperGuestTitle superGuestTitle)
        {
            superGuestTitle.Id = NextId();
            _superGuests.Add(superGuestTitle);
            _serializer.ToCSV(FilePath, _superGuests);
        }
        public List<SuperGuestTitle> GetAll()
        {
            return _superGuests;
        }
        public SuperGuestTitle GetById(int id)
        {
            return _superGuests.Find(n => n.Id == id);
        }
        
        

        public int NextId()
        {
            _superGuests = _serializer.FromCSV(FilePath);
            if (_superGuests.Count < 1)
            {
                return 1;
            }
            return _superGuests.Max(c => c.Id) + 1;
        }
        public void Delete(SuperGuestTitle title)
        {
            _superGuests = _serializer.FromCSV(FilePath);
            SuperGuestTitle founded = _superGuests.Find(c => c.Id == title.Id);
            _superGuests.Remove(founded);
            _serializer.ToCSV(FilePath, _superGuests);
        }
        public void Update(SuperGuestTitle title)
        {
            _superGuests = _serializer.FromCSV(FilePath);
            SuperGuestTitle current = _superGuests.Find(c => c.Id == title.Id);
            int index = _superGuests.IndexOf(current);
            _superGuests.Remove(current);
            _superGuests.Insert(index, title);
            _serializer.ToCSV(FilePath, _superGuests);

        }

        public SuperGuestTitle GetByGuestId(int id)
        {
            return _superGuests.Find(n => n.Guest.Id == id);
        }

    }
}
