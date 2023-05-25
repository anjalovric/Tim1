using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Serializer;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class NumberOfVisitedToursRepository:INumberOfVisitedToursRepository
    {
        private const string FilePath = "../../../Resources/Data/visitedTours.csv";

        private readonly Serializer<NumberOfVisitedTours> _serializer;


        private List<NumberOfVisitedTours> tours;

        public NumberOfVisitedToursRepository()
        {
            _serializer = new Serializer<NumberOfVisitedTours>();
            tours = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            tours = _serializer.FromCSV(FilePath);
            if (tours.Count < 1)
            {
                return 1;
            }
            return tours.Max(c => c.Id) + 1;
        }
        public List<NumberOfVisitedTours> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public NumberOfVisitedTours Save(NumberOfVisitedTours tour)
        {
            tour.Id = NextId();
            tours = _serializer.FromCSV(FilePath);
            tours.Add(tour);
            _serializer.ToCSV(FilePath, tours);
            return tour;
        }
        public NumberOfVisitedTours Update(NumberOfVisitedTours tour)
        {
            tours = _serializer.FromCSV(FilePath);
            NumberOfVisitedTours current = tours.Find(c => c.Id == tour.Id);
            int index = tours.IndexOf(current);
            tours.Remove(current);
            tours.Insert(index, tour);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, tours);
            return tour;
        }

        public void Delete(NumberOfVisitedTours tour)
        {
            tours = _serializer.FromCSV(FilePath);
            NumberOfVisitedTours founded = tours.Find(c => c.Id == tour.Id);
            tours.Remove(founded);
            _serializer.ToCSV(FilePath, tours);
        }
        public NumberOfVisitedTours GetById(int id)
        {
            return tours.Find(c => c.Id == id);
        }

    }
}
