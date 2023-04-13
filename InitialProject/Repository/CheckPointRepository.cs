using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Serializer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
namespace InitialProject.Repository
{
    public class CheckPointRepository:ICheckPointRepository
    {
        private const string FilePath = "../../../Resources/Data/checkPoints.csv";
        private readonly Serializer<CheckPoint> _serializer;
        private List<CheckPoint> _checkPoints;
        public CheckPointRepository()
        {
            _serializer = new Serializer<CheckPoint>();
            _checkPoints = _serializer.FromCSV(FilePath);
        }
        public List<CheckPoint> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public CheckPoint Save(CheckPoint checkPoint)
        {
            checkPoint.Id = NextId();
            _checkPoints = _serializer.FromCSV(FilePath);
            _checkPoints.Add(checkPoint);
            _serializer.ToCSV(FilePath, _checkPoints);
            return checkPoint;
        }
        public int NextId()
        {
            _checkPoints = _serializer.FromCSV(FilePath);
            if (_checkPoints.Count < 1)
            {
                return 1;
            }
            return _checkPoints.Max(c => c.Id) + 1;
        }
        public void Delete(CheckPoint checkPoint)
        {
            _checkPoints = _serializer.FromCSV(FilePath);
            CheckPoint founded = _checkPoints.Find(c => c.Id == checkPoint.Id);
            _checkPoints.Remove(founded);
            _serializer.ToCSV(FilePath, _checkPoints);
        }
        public CheckPoint Update(CheckPoint checkPoint)
        {
            _checkPoints = _serializer.FromCSV(FilePath);
            CheckPoint current = _checkPoints.Find(c => c.Id == checkPoint.Id);
            int index = _checkPoints.IndexOf(current);
            _checkPoints.Remove(current);
            _checkPoints.Insert(index, checkPoint);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _checkPoints);
            return checkPoint;
        }

        public CheckPoint GetById(int id)
        {
            return _checkPoints.Find(n => n.Id == id);
        }

        public List<CheckPoint> GetByInstance(int tourId)
        {
            _checkPoints = _serializer.FromCSV(FilePath);
            List<CheckPoint> checkPoints = new List<CheckPoint>();
            foreach (CheckPoint checkPoint in _checkPoints)
            {
                if(checkPoint.TourId ==tourId )
                    checkPoints.Add(checkPoint);
            }
            return checkPoints;
        }
        public void FindPointsForSelectedInstance(TourInstance selectedInstance,ObservableCollection<CheckPoint> AllPoints)
        {
            _checkPoints = _serializer.FromCSV(FilePath);
            if (selectedInstance != null)
            {
                foreach (CheckPoint point in _checkPoints)
                {
                    if (point.TourId == selectedInstance.Tour.Id)
                        AllPoints.Add(point);
                }
            }
        }
        public void CheckFirstPoint(ObservableCollection<CheckPoint> AllPoints)
        {
            foreach (CheckPoint point in AllPoints)
            {
                if (point.Order == 1)
                {
                    point.Checked = true;
                    Update(point);
                }
            }
        }

    }
}
