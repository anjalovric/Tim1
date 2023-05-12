using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Domain.Model;
using InitialProject.WPF.Views.Guest1Views;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using InitialProject.Service;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class AccommodationDetailsViewModel :INotifyPropertyChanged
    {
        public Func<double, string> YAxisLabelFormatter => value => value.ToString("N1");
        private Guest1 guest1;
        private DateTime currentDate;
        private int currentCounter = 0;
        private List<AccommodationImage> images;
        public Accommodation SelectedAccommodation { get; set; }
        public int AverageRating { get; set; }
        public int ReviewsNumber { get; set; }
        public List<string> Labels { get; set; }
        public SeriesCollection SeriesCollection { get; set; }

        private BitmapImage imageSource;
        public BitmapImage ImageSource
        {
            get { return imageSource; }
            set
            {
                if (value != imageSource)
                    imageSource = value;
                OnPropertyChanged("ImageSource");
            }

        }
        private bool isNextEnabled;
        public bool IsNextEnabled
        {
            get { return isNextEnabled; }
            set
            {
                if (value != isNextEnabled)
                    isNextEnabled = value;
                OnPropertyChanged("IsNextEnabled");
            }

        }
        public RelayCommand ReserveCommand { get; set; }

        public RelayCommand NextPhotoCommand { get; set; }
        public RelayCommand PreviousPhotoCommand { get; set; }
        public AccommodationDetailsViewModel(Accommodation currentAccommodation, Guest1 guest1)
        {
            this.guest1 = guest1;
            SelectedAccommodation = currentAccommodation;
            currentDate = DateTime.Now;
            SetChartData();
            SetFirstImage();
            MakeCommands();
            SetRating();
            SetButtonEnableProperty();
        }
        private void SetChartData()
        {
            LastYearAccommodationReservationsService lastYearAccommodationReservationsService = new LastYearAccommodationReservationsService();
            List<int> values = new List<int>();
            Labels = new List<string>();
            DateTime lastYear = currentDate.AddMonths(-13);
            while (lastYear <= currentDate)
            {
                values.Add(lastYearAccommodationReservationsService.GetReservationsNumberByMonthForAccommodation(lastYear, SelectedAccommodation, currentDate));
                Labels.Add(lastYear.ToString("MMM").ToUpper());
                lastYear = lastYear.AddMonths(1);
            }
            SeriesCollection = new SeriesCollection();
            ColumnSeries columnSeries = new ColumnSeries();
            columnSeries.Values = new ChartValues<int>();
            for (int i = 0; i < values.Count; i++)
            {
                ChartValues<int> columnValues = new ChartValues<int> { values[i] };
                columnSeries.Values.Add(values[i]);
                columnSeries.Title = "Completed reservations";
            }
            SeriesCollection.Add(columnSeries);
        }
        private void SetButtonEnableProperty()
        {
            if (images.Count >= 2)
                IsNextEnabled = true;
            else
                IsNextEnabled = false;
        }
        private void SetRating()
        {
            AccommodationAverageReviewService accommodationAverageReviewService = new AccommodationAverageReviewService();
            AverageRating = accommodationAverageReviewService.GetAverageRating(SelectedAccommodation);
            ReviewsNumber = accommodationAverageReviewService.GetReviewsNumberByAccommodation(SelectedAccommodation);
        }
        private void MakeCommands()
        {
            NextPhotoCommand = new RelayCommand(NextPhoto_Executed, CanExecute);
            PreviousPhotoCommand = new RelayCommand(PreviousPhoto_Executed, CanExecute);
            ReserveCommand = new RelayCommand(Reserve_Executed, CanExecute);
        }
        private void Reserve_Executed(object sender)
        {
            AccommodationReservationFormView accommodationReservationForm = new AccommodationReservationFormView(SelectedAccommodation, guest1);
            accommodationReservationForm.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
            accommodationReservationForm.ShowDialog();
        }
        private bool CanExecute(object sender)
        {
            return true;
        }

        private void SetFirstImage()
        {
            AccommodationImageService accommodationImageService = new AccommodationImageService();
            images = new List<AccommodationImage>(accommodationImageService.GetAllByAccommodation(SelectedAccommodation));
            ImageSource = new BitmapImage(new Uri(images[0].Url, UriKind.Relative));
        }
        private void PreviousPhoto_Executed(object sender)
        {
            currentCounter--;
            if (currentCounter < 0)
                currentCounter = images.Count - 1;
            ImageSource = new BitmapImage(new Uri(images[currentCounter].Url, UriKind.Relative));
        }
        private void NextPhoto_Executed(object sender)
        {
            currentCounter++;
            if (currentCounter >= images.Count)
                currentCounter = 0;
            ImageSource = new BitmapImage(new Uri(images[currentCounter].Url, UriKind.Relative));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
