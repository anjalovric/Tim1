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
        private AccommodationReservationService accommodationReservationService;
        private SuperGuestTitleService superGuestTitleService;
        private Guest1 guest1;
        private DateTime currentDate;
        private int currentCounter = 0;
        private List<AccommodationImage> images;
        private DateTime arrival;
        private DateTime departure;
        public Func<double, string> YAxisLabelFormatter => value => value.ToString("N1");
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

        public AccommodationDetailsViewModel(Accommodation currentAccommodation, Guest1 guest1, DateTime arrival = default, DateTime departure = default)
        {
            this.guest1 = guest1;
            SelectedAccommodation = currentAccommodation;
            currentDate = DateTime.Now;
            this.arrival = arrival;
            this.departure = departure;
            SetChartData();
            Initialize();
            MakeCommands();
        }
        private void Initialize()
        {
            SetFirstImage();
            SetRating();
            SetButtonEnableProperty();
            accommodationReservationService = new AccommodationReservationService();
            superGuestTitleService = new SuperGuestTitleService();
        }
        private void MakeCommands()
        {
            NextPhotoCommand = new RelayCommand(NextPhoto_Executed, CanExecute);
            PreviousPhotoCommand = new RelayCommand(PreviousPhoto_Executed, CanExecute);
            ReserveCommand = new RelayCommand(Reserve_Executed, CanExecute);
        }

        //methods for diagram
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
                columnSeries.Title = "Completed reservations";
            }
            SeriesCollection.Add(columnSeries);
        }

        //other methods
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
        private void ShowMessageBoxForSentReservation()
        {
            Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("Successfully done!", "/Resources/Images/done.png");
            messageBox.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
            messageBox.ShowDialog();
        }
       
        private void MakeNewReservation()
        {
            AccommodationReservation newReservation = new AccommodationReservation(guest1, SelectedAccommodation, arrival, departure);
            accommodationReservationService.Add(newReservation);
            superGuestTitleService.DecrementPoints(guest1);
            AnywhereAnytimeView view = new AnywhereAnytimeView(guest1);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = view;
            ShowMessageBoxForSentReservation();
        }
        public async Task<bool> ConfirmReservationMessageBox()
        {
            var result = new TaskCompletionSource<bool>();
            Guest1YesNoMessageBoxView messageBox = new Guest1YesNoMessageBoxView("Do you want to make a reservation for " + arrival + " - " + departure + "?", "/Resources/Images/qm.png", result);
            messageBox.Owner = Application.Current.Windows.OfType<DatesForAccommodationReservationView>().FirstOrDefault();
            messageBox.ShowDialog();
            var returnedResult = await result.Task;
            return returnedResult;
        }
        
        private void SetFirstImage()
        {
            AccommodationImageService accommodationImageService = new AccommodationImageService();
            images = new List<AccommodationImage>(accommodationImageService.GetAllByAccommodation(SelectedAccommodation));
            ImageSource = new BitmapImage(new Uri(images[0].Url, UriKind.Relative));
        }


        //execute commands
        private void PreviousPhoto_Executed(object sender)
        {
            currentCounter = currentCounter - 1;
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
        private async void Reserve_Executed(object sender)
        {
            if (arrival == DateTime.MinValue && departure == DateTime.MinValue)
            {
                AccommodationReservationFormView accommodationReservationForm = new AccommodationReservationFormView(SelectedAccommodation, guest1);
                accommodationReservationForm.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
                accommodationReservationForm.ShowDialog();
            }
            else
            {
                Task<bool> result = ConfirmReservationMessageBox();
                bool IsYesClicked = await result;
                if (IsYesClicked)
                    MakeNewReservation();
            }
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
