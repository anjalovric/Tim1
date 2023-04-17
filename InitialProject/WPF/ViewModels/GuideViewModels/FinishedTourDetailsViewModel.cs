using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using System.Collections.ObjectModel;

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
        public ObservableCollection<CheckPointInformation> CheckPointInformations { get; set; }

        public FinishedTourDetailsViewModel(TourInstance selected)
        {
            selectedInstance = selected;
            
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
            AlertGuest2Service alertGuest2Service = new AlertGuest2Service();
            CheckPointService checkPointService = new CheckPointService();
            foreach (CheckPoint point in checkPointService.GetInstancePoints(selectedInstance))
            {
                CheckPointInformation pointInformation = new CheckPointInformation();
                pointInformation.CheckPoint = point;
                pointInformation.countGuests = alertGuest2Service.CountGuestsOnPoint(point.Id, selectedInstance);
                ShowGuestsOnPoint(point.Id, pointInformation);
                CheckPointInformations.Add(pointInformation);

            }
        }
        private void WriteVoucherPrecentacge(int selectedId)
        {
            TourDetailsService detailsService=new TourDetailsService();
            if (detailsService.MakeWithVoucherPrecentage(selectedId) > 0)
                With = detailsService.MakeWithVoucherPrecentage(selectedId).ToString("#.##") + " %";
            else
                With = 0 + "%";

            if(detailsService.MakeWithoutVoucherPrecentage(selectedId)>0)
                Without= detailsService.MakeWithoutVoucherPrecentage(selectedId).ToString("#.##") + " %";
            else
                Without=0 + "%";
        }

        private void WriteAgePrecentacge(int selectedId)
        {
            TourDetailsService detailsService = new TourDetailsService();
            if (detailsService.MakeUnder18Precentage(selectedId)>0)
                Under = detailsService.MakeUnder18Precentage(selectedId).ToString("#.##") + " %";
            else
                Under = 0 + "%";
            if(detailsService.MakeBetween18And50Precentage(selectedId) > 0)
                Between= detailsService.MakeBetween18And50Precentage(selectedId).ToString("#.##") + " %";
            else
                Between= 0 + "%";
            if(detailsService.MakeOver50Precentage(selectedId)>0)
                Over = detailsService.MakeOver50Precentage(selectedId).ToString("#.##") + " %";
            else
                Over = 0 + "%";
        }

        private void ShowGuestsOnPoint(int currentPointId, CheckPointInformation pointInformation)
        {
            TourDetailsService detailsService=new TourDetailsService();
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
            TourDetailsService detailsService = new TourDetailsService();
            Attendance = detailsService.MakeAttendancePrecentage(selectedId).ToString("#.##") + " %";
        }      
    }
}
