using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for FinishedTourDetails.xaml
    /// </summary>
    public partial class FinishedTourDetails : Page
    {
        TourInstance selectedInstance;
        private TourDetailsService detailsService;
        public ObservableCollection<CheckPointInformation> CheckPointInformations { get;set; }

        public FinishedTourDetails(TourInstance selected)
        {
            InitializeComponent();
            DataContext = this;
            selectedInstance = selected;
            detailsService = new TourDetailsService();
            CheckPointInformations = new ObservableCollection<CheckPointInformation>();

            MakeHeader(selected);
            ComposeReport();
            WriteVoucherPrecentacge(selected.Id);
            WriteAgePrecentacge(selected.Id);
            WriteAttendancePrecentacge(selected.Id);

        }
        private void MakeHeader(TourInstance selected)
        {
            Label header = new Label();
            header.Content = selected.Tour.Name.ToString().ToUpper() + ", " + selected.StartDate.ToString().Split(" ")[0] + ", " + selected.StartClock.ToString();
            header.FontWeight = FontWeights.Bold;
            PointStack.Children.Add(header);
        }

        private void ComposeReport()
        {
            foreach (CheckPoint point in detailsService.GetInstancePoints(selectedInstance))
            {
                CheckPointInformation pointInformation = new CheckPointInformation();
                pointInformation.CheckPoint= point;
                pointInformation.countGuests = detailsService.CountGuestsOnPoint(point.Id, selectedInstance);
                //WriteGuestsCountOnPoint(detailsService.CountGuestsOnPoint(point.Id, selectedInstance), point.Id,pointInformation);
                ShowGuestsOnPoint(point.Id, pointInformation);
                CheckPointInformations.Add(pointInformation);
                
            }
        }


        private void WriteVoucherPrecentacge(int selectedId)
        {
           
            With.Content =detailsService.MakeWithVoucherPrecentage(selectedId) + " %";
           
            Without.Content = detailsService.MakeWithoutVoucherPrecentage(selectedId) + " %";

        }

        private void WriteAgePrecentacge(int selectedId)
        {


            Under.Content = detailsService.MakeUnder18Precentage(selectedId) + " %";
           
            Between.Content =detailsService.MakeBetween18And50Precentage(selectedId) + " %";
        
            Over.Content = detailsService.MakeOver50Precentage(selectedId) + " %";
      

        }
        private void WriteGuestsCountOnPoint(int count, int currentPointId)
        {
            Label pointOrder = new Label();
            pointOrder.Content = "Number of guests on point: " + detailsService.checkPointService.GetById(currentPointId).Order + ". " + detailsService.checkPointService.GetById(currentPointId).Name + ": " + count.ToString();
            PointStack.Children.Add(pointOrder);
        }

        private void ShowGuestsOnPoint(int currentPointId,CheckPointInformation pointInformation)
        {

            foreach (AlertGuest2 alert in detailsService.alertGuest2Service.GetAll())
            {
                if (alert.CheckPointId == currentPointId && alert.Availability && alert.InstanceId == selectedInstance.Id)
                {
                    Guest2 presentGuest = detailsService.guest2Service.GetById(alert.Guest2Id);
                    pointInformation.guest2s.Add(presentGuest);
                   // WriteGuestOnPoint(presentGuest);
                }
            }
        }


        private void WriteGuestOnPoint(Guest2 presentGuest)
        {
            Label presentGuestOnPoint = new Label();
            presentGuestOnPoint.Content = "       On point were guests from reservation of: " + presentGuest.Name + " " + presentGuest.LastName + ", username: " + presentGuest.Username;
            PointStack.Children.Add(presentGuestOnPoint);
        }


        private void WriteAttendancePrecentacge(int selectedId)
        {   
            Total.Content = detailsService.MakeAttendancePrecentage(selectedId) + " %";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = NavigationService.GoBack;
        }
    }
}
