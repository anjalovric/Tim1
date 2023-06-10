using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using InitialProject.Model;
using LiveCharts;
using LiveCharts.Wpf;
using InitialProject.Service;


namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class SentAccommodationReservationRequestsViewModel : INotifyPropertyChanged
    {
        public Func<double, string> YAxisLabelFormatter => value => value.ToString("N1");
        private DateTime currentDate;
        private RequestForReschedulingService requestForReschedulingService;
        private ObservableCollection<ReschedulingAccommodationRequest> approvedRequests;
        private Guest1 guest1;
        //diagram
        public List<string> Labels { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public ObservableCollection<ReschedulingAccommodationRequest> ApprovedRequests
        {
            get { return approvedRequests; }
            set
            {
                if (value != approvedRequests)
                    approvedRequests = value;
                OnPropertyChanged("ApprovedRequests");
            }

        }
        private ObservableCollection<ReschedulingAccommodationRequest> pendingRequests;
        public ObservableCollection<ReschedulingAccommodationRequest> PendingRequests
        {
            get { return pendingRequests; }
            set
            {
                if (value != pendingRequests)
                    pendingRequests = value;
                OnPropertyChanged("PendingRequests");
            }

        }
        private ObservableCollection<ReschedulingAccommodationRequest> declinedRequests;
        public ObservableCollection<ReschedulingAccommodationRequest> DeclinedRequests
        {
            get { return declinedRequests; }
            set
            {
                if (value != declinedRequests)
                    declinedRequests = value;
                OnPropertyChanged("DeclinedRequests");
            }

        }
        public SentAccommodationReservationRequestsViewModel(Guest1 guest1)
        {
            this.guest1 = guest1;
            Initialize();
            SetChartData();
        }

        private void Initialize()
        {
            requestForReschedulingService = new RequestForReschedulingService();
            ApprovedRequests = new ObservableCollection<ReschedulingAccommodationRequest>(requestForReschedulingService.GetApprovedRequests(guest1));
            PendingRequests = new ObservableCollection<ReschedulingAccommodationRequest>(requestForReschedulingService.GetPendingRequests(guest1));
            DeclinedRequests = new ObservableCollection<ReschedulingAccommodationRequest>(requestForReschedulingService.GetDeclinedRequests(guest1));
            currentDate = DateTime.Now;
        }

        //methods for diagram
        private void SetChartData()
        {
            RequestForReschedulingService requestForReschedulingService = new RequestForReschedulingService();
            List<int> values = new List<int>();
            Labels = new List<string>();
            DateTime lastYear = currentDate.AddMonths(-13);
            while (lastYear <= currentDate)
            {
                values.Add(requestForReschedulingService.GetRequestsNumberByMonth(lastYear, guest1, currentDate));
                Labels.Add(lastYear.ToString("MMM").ToUpper());
                lastYear = lastYear.AddMonths(1);
            }
            SetSeriesCollection(values);
        }
        private void SetSeriesCollection(List<int> values)
        {
            SeriesCollection = new SeriesCollection();
            ColumnSeries columnSeries = new ColumnSeries();
            columnSeries.Values = new ChartValues<int>();
            for (int i = 0; i < values.Count; i++)
            {
                ChartValues<int> columnValues = new ChartValues<int> { values[i] };
                columnSeries.Values.Add(values[i]);
                columnSeries.Title = "Created requests";
            }
            SeriesCollection.Add(columnSeries);
        }

        public event PropertyChangedEventHandler PropertyChanged;  
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
    }
}
