using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.GuideViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Windows;

namespace InitialProject.WPF.ViewModels
{
    public class FinishedTourDetailsViewModel
    {
        public string With { get; set; }
        public string Without { get; set; } 

        public string Attendance { get; set; }

        public string Under { get; set; }

        public string Over { get;set; }

        public string Between { get; set; }

        public string Header { get; set; }
        TourInstance selectedInstance;
        private TourDetailsService detailsService;
        public ObservableCollection<CheckPointInformation> CheckPointInformations { get; set; }

        public FinishedTourDetailsViewModel(TourInstance selected)
        {
            selectedInstance = selected;
            detailsService = new TourDetailsService();
            CheckPointInformations = new ObservableCollection<CheckPointInformation>();

            UnitReport(selected);

        }
        public void UnitReport(TourInstance selected)
        {
            MakeHeader(selected);
            ComposeReport();
            WriteVoucherPrecentacge(selected.Id);
            WriteAgePrecentacge(selected.Id);
            WriteAttendancePrecentacge(selected.Id);
        }
        private void MakeHeader(TourInstance selected)
        {
            Header = selected.Tour.Name.ToString().ToUpper() + ", " + selected.StartDate.ToString().Split(" ")[0] + ", " + selected.StartClock.ToString();          
        }

        private void ComposeReport()
        {
            foreach (CheckPoint point in detailsService.GetInstancePoints(selectedInstance))
            {
                CheckPointInformation pointInformation = new CheckPointInformation();
                pointInformation.CheckPoint = point;
                pointInformation.countGuests = detailsService.CountGuestsOnPoint(point.Id, selectedInstance);
                ShowGuestsOnPoint(point.Id, pointInformation);
                CheckPointInformations.Add(pointInformation);

            }
        }


        private void WriteVoucherPrecentacge(int selectedId)
        {

            With = detailsService.MakeWithVoucherPrecentage(selectedId) + " %";

            Without= detailsService.MakeWithoutVoucherPrecentage(selectedId) + " %";

        }

        private void WriteAgePrecentacge(int selectedId)
        {
            Under = detailsService.MakeUnder18Precentage(selectedId) + " %";

            Between= detailsService.MakeBetween18And50Precentage(selectedId) + " %";

            Over = detailsService.MakeOver50Precentage(selectedId) + " %";
        }

        private void ShowGuestsOnPoint(int currentPointId, CheckPointInformation pointInformation)
        {

            foreach (AlertGuest2 alert in detailsService.alertGuest2Service.GetAll())
            {
                if (alert.CheckPointId == currentPointId && alert.Availability && alert.InstanceId == selectedInstance.Id)
                {
                    Guest2 presentGuest = detailsService.guest2Service.GetById(alert.Guest2Id);
                    pointInformation.guest2s.Add(presentGuest);
                }
            }
        }



        private void WriteAttendancePrecentacge(int selectedId)
        {
            Attendance = detailsService.MakeAttendancePrecentage(selectedId) + " %";
        }

       
    }
}
