using InitialProject.Domain;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace InitialProject.Service
{
    public class CheckPointService
    {
        private ICheckPointRepository repository=Injector.CreateInstance<ICheckPointRepository>();
        public CheckPointService() {  }
        public List<CheckPoint> GetAll()
        {
            return repository.GetAll();
        }
        public CheckPoint Save(CheckPoint checkPoint)
        {
            return repository.Save(checkPoint);
        }
        public List<CheckPoint> GetByInstance(int tourId)
        {
            return repository.GetByInstance(tourId);
        }
        public void UpdateAllPointsListToNextPoint(ObservableCollection<CheckPoint> AllPoints, int orderCounter)
        {
            int pointSize = AllPoints.Count;
            List<CheckPoint> points = AllPoints.ToList();
            AllPoints.Clear();
            for (int i = 0; i < orderCounter; i++)
            {
                points[i].Checked = true;
                repository.Update(points[i]);
                AllPoints.Add(points[i]);
            }
            for (int i = orderCounter; i < pointSize; i++)
            {
                points[i].Checked = false;
                repository.Update(points[i]);
                AllPoints.Add(points[i]);
            }
        }
        public void FindPointsForSelectedInstance(TourInstance selectedInstance, ObservableCollection<CheckPoint> AllPoints)
        {
            repository.FindPointsForSelectedInstance(selectedInstance,AllPoints);
        }
        public void CheckFirstPoint(ObservableCollection<CheckPoint> AllPoints)
        {
           repository.CheckFirstPoint(AllPoints);
        }
        public List<CheckPoint> GetInstancePoints(TourInstance selectedInstance)
        {
            List<CheckPoint> tourPoints = new List<CheckPoint>();
            foreach (CheckPoint point in GetAll())
                if (point.TourId == selectedInstance.Tour.Id)
                    tourPoints.Add(point);
            return tourPoints;
        }
        public List<string> GetPointsForGuest(int guest2Id, TourInstance instance)
        {
            AlertGuest2Service alertGuest2Service = new AlertGuest2Service();
            List<string> pointsName = new List<string>();
            foreach (CheckPoint checkPoint in GetByInstance(instance.Tour.Id))
            {
                foreach (AlertGuest2 alert in alertGuest2Service.GetByInstanceIdAndGuestId(instance.Id, guest2Id))
                {
                    if (alert.Availability && alert.CheckPointId == checkPoint.Id)
                        pointsName.Add(checkPoint.Name);
                }
            }
            return pointsName;
        }
    }
}
