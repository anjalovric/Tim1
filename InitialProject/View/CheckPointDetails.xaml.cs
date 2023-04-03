using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
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
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for CheckPointDetails.xaml
    /// </summary>
    public  partial class CheckPointDetails : Window
    {

        TourInstance selectedInstance;
        private TourDetailsService detailsService;

        public CheckPointDetails(TourInstance selected)
        {
            InitializeComponent();
            DataContext = this;
            selectedInstance = selected;
            detailsService = new TourDetailsService();

            MakeHeader(selected);
            ComposeReport();
            WriteVoucherPrecentacge(selected.Id);
            WriteAgePrecentacge(selected.Id);
            WriteAttendancePrecentacge(selected.Id);

        }
        private void MakeHeader(TourInstance selected)
        {
            Label header=new Label();
            header.Content=selected.Tour.Name.ToString().ToUpper()+", " + selected.StartDate.ToString().Split(" ")[0]+", "+ selected.StartClock.ToString();
            header.FontWeight= FontWeights.Bold;
            PointStack.Children.Add(header);
        }

        private void ComposeReport()
        {
            foreach (CheckPoint point in detailsService.GetInstancePoints(selectedInstance))
            {
                WriteGuestsCountOnPoint(detailsService.CountGuestsOnPoint(point.Id,selectedInstance),point.Id);
                ShowGuestsOnPoint(point.Id);
            }
        }
        

        private void WriteVoucherPrecentacge(int selectedId)
        {
            Label voucher = new Label();
            voucher.Content = "Voucher statistics";
            Label with=new Label();
            with.Content = "With vouchers: " + detailsService.MakeWithVoucherPrecentage(selectedId) + " %";
            Label without=new Label();
            without.Content = "Without vouchers: " + detailsService.MakeWithoutVoucherPrecentage(selectedId) + " %";
            StaticticsStack.Children.Add(voucher);
            StaticticsStack.Children.Add(with);
            StaticticsStack.Children.Add(without);
        }

        private void WriteAgePrecentacge(int selectedId)
        {
            Label age = new Label();
            age.Content = "Age statistics";
            Label under18 = new Label();
            under18.Content = "Under 18: " + detailsService.MakeUnder18Precentage(selectedId) + " %";
            Label between18and50 = new Label();
            between18and50.Content = "Between 18 and 50: " + detailsService.MakeBetween18And50Precentage(selectedId) + " %";
            Label over50 = new Label();
            over50.Content = "Over 50: " + detailsService.MakeOver50Precentage(selectedId) + " %";
            Label empty=new Label();

            StaticticsStack.Children.Add(age);
            StaticticsStack.Children.Add(under18);
            StaticticsStack.Children.Add(between18and50);
            StaticticsStack.Children.Add(over50);

        }
        private void WriteGuestsCountOnPoint(int count,int currentPointId)
        {
            Label pointOrder = new Label();
            pointOrder.Content = "Number of guests on point: " + detailsService.checkPointService.GetById(currentPointId).Order + ". " + detailsService.checkPointService.GetById(currentPointId).Name + ": " + count.ToString();
            PointStack.Children.Add(pointOrder);
        }

        private void ShowGuestsOnPoint(int currentPointId)
        {

            foreach (AlertGuest2 alert in detailsService.alertGuest2Service.GetAll())
            {
                if (alert.CheckPointId == currentPointId && alert.Availability && alert.InstanceId == selectedInstance.Id)
                {
                    Guest2 presentGuest = detailsService.guest2Service.GetById(alert.Guest2Id);
                    WriteGuestOnPoint(presentGuest);
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
            Label voucher = new Label();
            voucher.Content = "Total attendance: "+ detailsService.MakeAttendancePrecentage(selectedId)+" %";
            
            StaticticsStack.Children.Add(voucher);
        }
    }
}
