using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for CheckPointDetails.xaml
    /// </summary>
    public  partial class CheckPointDetails : Window
    {
        private AlertGuest2Repository alertGuest2Repository;
        private CheckPointRepository pointRepository;
        private Guest2Repository guest2Repository;
        TourInstance selectedInstance;
        int pointCounter = 0;
        public CheckPointDetails(TourInstance selected)
        {
            InitializeComponent();
            DataContext = this;
            selectedInstance = selected;
            alertGuest2Repository = new AlertGuest2Repository();
            pointRepository = new CheckPointRepository();
            guest2Repository = new Guest2Repository();

            foreach (CheckPoint point in GetInstancePoints())
            {
                CountGuestsOnPoint(point.Id);
                ShowGuestsOnPoint(point.Id);
            }
        }

        private List<CheckPoint> GetInstancePoints()
        {
            List <CheckPoint> tourPoints = new List<CheckPoint>();
            foreach(CheckPoint point in pointRepository.GetAll())
            {
                if (point.TourId == selectedInstance.Tour.Id)
                {
                    tourPoints.Add(point);
                }
            }
            return tourPoints;
        }
        private void CountGuestsOnPoint(int currentPointId)
        {
            int counter = 0;
            List<AlertGuest2> allAlerts = alertGuest2Repository.GetAll();

            foreach (AlertGuest2 alert in allAlerts)
            {
                if (alert.CheckPointId == currentPointId && alert.Availability && alert.InstanceId == selectedInstance.Id)
                {
                    counter++;

                }
            }
            
            Label pointOrder = new Label();
            pointOrder.Content = "Number of guests on point: " + pointRepository.GetAll().Find(n => n.Id == currentPointId).Order + ". " + pointRepository.GetAll().Find(n => n.Id == currentPointId).Name + ": " + counter.ToString();
            PointStack.Children.Add(pointOrder);
        }

        private void ShowGuestsOnPoint(int currentPointId)
        {
            List<AlertGuest2> allAlerts = alertGuest2Repository.GetAll();

            foreach (AlertGuest2 alert in allAlerts)
            {
                if (alert.CheckPointId == currentPointId && alert.Availability && alert.InstanceId == selectedInstance.Id)
                {
                    Guest2 presentGuest = guest2Repository.GetAll().Find(n => n.Id == alert.Guest2Id);
                    Label presentGuestOnPoint = new Label();
                    presentGuestOnPoint.Content = "       On point was : " + presentGuest.Name + " " + presentGuest.LastName +", username: " +presentGuest.Username;
                    PointStack.Children.Add(presentGuestOnPoint);
                }
            }
        }
    }
}
