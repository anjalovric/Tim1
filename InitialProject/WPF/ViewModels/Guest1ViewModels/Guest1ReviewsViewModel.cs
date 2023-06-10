using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using InitialProject.Model;
using InitialProject.ReportPatterns;
using InitialProject.Service;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.Guest1Views;
using LiveCharts;
using LiveCharts.Wpf;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class Guest1ReviewsViewModel :INotifyPropertyChanged
    {
        public Func<double, string> YAxisLabelFormatter => value => value.ToString("N1");
        private Guest1 guest1;
        private DateTime currentDate;
        GuestReviewService guestReviewService;
        public List<string> Labels { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public double AverageCleanliness { get; set; }
        public int ReviewsNumber { get; set; }
        public double AverageFollowingRules { get; set; }
        public int AverageRating { get; set; }

        private ObservableCollection<GuestReview> guest1Reviews;
        public ObservableCollection<GuestReview> Guest1Reviews
        {
            get { return guest1Reviews; }
            set
            {
                if (value != guest1Reviews)
                    guest1Reviews = value;
                OnPropertyChanged("Ruest1Reviews");
            }

        }
        public GuestReview SelectedReview { get; set; }
        public RelayCommand ShowReviewDetailsCommand { get; set; }
        public RelayCommand GenerateReportCommand { get; set; }
        public Guest1ReviewsViewModel(Guest1 guest1)
        {
            this.guest1 = guest1;
            Initialize();
            SetChartData();
            SetRatings();
            MakeCommands();   
        }

        private void Initialize()
        {
            guestReviewService = new GuestReviewService();
            Guest1Reviews = new ObservableCollection<GuestReview>(guestReviewService.GetAllToDisplay(guest1));
            currentDate = DateTime.Now;
        }
        private void MakeCommands()
        {
            ShowReviewDetailsCommand = new RelayCommand(ShowReviewDetails_Executed, CanExecute);
            GenerateReportCommand = new RelayCommand(GenerateReport_Executed, CanExecute);
        }
        //methods for diagram
        private void SetChartData()
        {
            GuestAverageReviewService guestAverageReviewService = new GuestAverageReviewService();
            List<double> values = new List<double>();
            Labels = new List<string>();
            DateTime lastYear = currentDate.AddMonths(-13);
            while (lastYear <= currentDate)
            {
                values.Add(guestAverageReviewService.GetAverageRatingByMonth(lastYear, guest1, currentDate));
                Labels.Add(lastYear.ToString("MMM").ToUpper());
                lastYear = lastYear.AddMonths(1);
            }
            SetSeriesCollection(values);
        }

        private void SetSeriesCollection(List<double> values)
        {
            SeriesCollection = new SeriesCollection();
            ColumnSeries columnSeries = new ColumnSeries();
            columnSeries.Values = new ChartValues<double>();
            for (int i = 0; i < values.Count; i++)
            {
                ChartValues<double> columnValues = new ChartValues<double> { values[i] };
                columnSeries.Values.Add(values[i]);
                columnSeries.Title = "Average rating";
            }
            SeriesCollection.Add(columnSeries);
        }

        //other methods
        private void SetRatings()
        {
            GuestAverageReviewService guestAverageReviewService = new GuestAverageReviewService();
            AverageCleanliness = guestAverageReviewService.GetAverageCleanlinessReview(guest1);
            AverageFollowingRules = guestAverageReviewService.GetAverageFollowingRulesReview(guest1);
            ReviewsNumber = guestAverageReviewService.GetReviewsNumberByGuest(guest1);
            AverageRating = guestAverageReviewService.GetAverageRating(guest1);
            AverageCleanliness = Math.Round(AverageCleanliness, 1);
            AverageFollowingRules = Math.Round(AverageFollowingRules, 1);
        }
        //execute commands
        private void GenerateReport_Executed(object sender)
        {
            ReportGenerator generator = new Guest1ReportPattern(guest1);
            generator.GenerateReport();
            PdfPreviewView previewView = new PdfPreviewView();
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = previewView;
        }
        private void ShowReviewDetails_Executed(object sender)
        {
            Guest1ReviewDetailsView view = new Guest1ReviewDetailsView(guest1, SelectedReview);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = view;

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
