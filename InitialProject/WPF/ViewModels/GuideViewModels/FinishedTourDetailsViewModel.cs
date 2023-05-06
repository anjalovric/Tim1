using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace InitialProject.WPF.ViewModels
{
    public class FinishedTourDetailsViewModel:INotifyPropertyChanged
    {
        public string Header { get; set; }
        private SeriesCollection totalAttendance;
        public SeriesCollection TotalAttendance
        {
            get { return totalAttendance; }
            set
            {
                if (value != totalAttendance)
                    totalAttendance = value;
                OnPropertyChanged();
            }
        }

        private SeriesCollection  attendanceVoucherPie;
        public SeriesCollection AttendanceVoucherPie
        {
            get { return attendanceVoucherPie; }
            set
            {
                if (value != attendanceVoucherPie)
                    attendanceVoucherPie = value;
                OnPropertyChanged();
            }
        }
        private SeriesCollection attendanceAgePie;
        public SeriesCollection AttendanceAgePie
        {
            get { return attendanceAgePie; }
            set
            {
                if (value != attendanceAgePie)
                    attendanceAgePie = value;
                OnPropertyChanged();
            }
        }


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
            Header = selected.Tour.Name.ToString().ToUpper() + ", " + selected.StartDate.ToString();          
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
        public RelayCommand OkToastCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void WriteVoucherPrecentacge(int selectedId)
        {
            TourDetailsService detailsService=new TourDetailsService();
          
            AttendanceVoucherPie = new SeriesCollection();
            AttendanceVoucherPie.Add(new PieSeries
            {
                Title = "WithVoucher",
                Values = new ChartValues<double> { detailsService.MakeWithVoucherPrecentage(selectedId) },
                DataLabels = true,
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#228B22"))
            });
              AttendanceVoucherPie.Add(new PieSeries
              {
                  Title = "WithoutVoucher",
                  Values = new ChartValues<double> { detailsService.MakeWithoutVoucherPrecentage(selectedId) },
                  DataLabels = true,
                  Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A8A8A8"))
              }
             ) ;
        }
        private void WriteAgePrecentacge(int selectedId)
        {
            TourDetailsService detailsService = new TourDetailsService();
            AttendanceAgePie = new SeriesCollection();
            AttendanceAgePie.Add(new PieSeries
            {
                Title = "Under 18",
                Values = new ChartValues<double> { detailsService.MakeUnder18Precentage(selectedId) },
                DataLabels = true,
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#228B22"))
            });
            AttendanceAgePie.Add(new PieSeries
            {
                Title = "Between 18 and 50",
                Values = new ChartValues<double> { detailsService.MakeBetween18And50Precentage(selectedId) },
                DataLabels = true,
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00A36C"))
            }
           );
            AttendanceAgePie.Add(new PieSeries
            {
                Title = "Over 50",
                Values = new ChartValues<double> { detailsService.MakeOver50Precentage(selectedId) },
                DataLabels = true,
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A8A8A8"))
            }
          );
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
            TotalAttendance = new SeriesCollection();
            TotalAttendance.Add(new PieSeries
            {
                Title = "Total attendance",
                Values = new ChartValues<double> { detailsService.MakeAttendancePrecentage(selectedId) },
                DataLabels = true,
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A8A8A8"))
            });
        }  
    }
}
