using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace InitialProject.Repository
{
    public class SuperGuideRepository:ISuperGuideRepository
    {
        private const string FilePath = "../../../Resources/Data/superGuideTitle.csv";

        private readonly Serializer<SuperGuide> _serializer;

        private List<SuperGuide> superGuides;


        public SuperGuide GetById(int id)
        {
            superGuides = _serializer.FromCSV(FilePath);
            return superGuides.Find(x => x.Id == id);   
        }
        public SuperGuideRepository()
        {
            _serializer = new Serializer<SuperGuide>();
            superGuides = _serializer.FromCSV(FilePath);
        }

        public List<SuperGuide> GetAll()
        {
            return _serializer.FromCSV(FilePath); ;
        }
        public SuperGuide Save(SuperGuide superGuide)
        {
            superGuide.Id = NextId();
            superGuides = _serializer.FromCSV(FilePath);
            superGuides.Add(superGuide);
            _serializer.ToCSV(FilePath, superGuides);
            return superGuide;
        }
        public int NextId()
        {
            superGuides = _serializer.FromCSV(FilePath);
            if (superGuides.Count < 1)
            {
                return 1;
            }
            return superGuides.Max(c => c.Id) + 1;
        }

        public void Delete(SuperGuide superGuide)
        {
            superGuides = _serializer.FromCSV(FilePath);
            SuperGuide founded = superGuides.Find(c => c.Id == superGuide.Id);
            superGuides.Remove(founded);
            _serializer.ToCSV(FilePath, superGuides);
        }
    }
}
