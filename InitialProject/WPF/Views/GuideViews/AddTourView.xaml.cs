using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// Interaction logic for AddTourView.xaml
    /// </summary>
    public partial class AddTourView : Page
    {

        private readonly GuideRepository guideRepository;
        private Guide currentGuide;
        private TourRepository tourRepository;
        private LocationRepository locationRepository;
        private CheckPointRepository checkPointRepository;
        private TourImageRepository tourImageRepository;
        private TourInstanceRepository tourInstanceRepository;
        private TourInstance newInstance;
        public int pointCounter = 0;
        public ObservableCollection<CheckPoint> TourPoints { get; set; }
        public ObservableCollection<TourImage> TourImages { get; set; }
        public ObservableCollection<TourInstance> Instances { get; set; }
        public ObservableCollection<TourInstance> TodayInstances { get; set; }
        public ObservableCollection<TourInstance> FutureInstances { get; set; }
        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> CitiesByCountry { get; set; }

        private string imageUrl;
        public Tour saved;
        private int tourId;
        private string startTime;

        private TourInstance selectedInstance {  get; set; }
        private CheckPoint SelectedCheckPoint { get; set; }
        public string InstanceStartHour
        {
            get => startTime;
            set
            {
                if (value != startTime)
                {
                    startTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ImageUrl
        {
            get => imageUrl;
            set
            {
                if (!value.Equals( imageUrl))
                {
                    imageUrl = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime startDate;
        public DateTime InstanceStartDate
        {
            get => startDate;
            set
            {
                if (value != startDate)
                {
                    startDate = value;
                    OnPropertyChanged();
                }
            }
        }
        private string hour;
        private User loggedInUser;
        public string Hours
        {
            get => hour;
            set
            {
                if (value != hour)
                {
                    hour = value;
                    OnPropertyChanged();
                }
            }
        }
        private string namet;
        public string NameTU
        {
            get => namet;
            set
            {
                if (value != namet)
                {
                    namet = value;
                    OnPropertyChanged();
                }
            }
        }
        private string name;
        public string NameT
        {
            get => name;
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }
        private string city;
        public string City
        {
            get => city;
            set
            {
                if (value != city)
                {
                    city = value;
                    OnPropertyChanged();
                }
            }
        }
        private string country;
        public string Country
        {
            get => country;
            set
            {
                if (value != country)
                {
                    country = value;
                    OnPropertyChanged();
                }
            }
        }
        private string description;
        public string Description
        {
            get => description;
            set
            {
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged();
                }
            }
        }
        private string language;
        public string LanguageT
        {
            get => language;
            set
            {
                if (value != language)
                {
                    language = value;
                    OnPropertyChanged();
                }
            }
        }
        private string duration;

        public string Duration
        {
            get => duration;
            set
            {
                if (value != duration)
                {
                    duration = value;
                    OnPropertyChanged();
                }
            }
        }

        private string maxGuests;

        public string MaxGuests
        {
            get => maxGuests;
            set
            {
                if (value != maxGuests)
                {
                    maxGuests = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime start;
        public DateTime Start
        {
            get => start;
            set
            {
                if (value != start)
                {
                    start = value;
                    OnPropertyChanged();
                }
            }
        }
        public AddTourView(ObservableCollection<TourInstance> todayInstances,User user, ObservableCollection<TourInstance> futureInstances)
        {
            InitializeComponent();
            DataContext = this;
            tourRepository = new TourRepository();
            locationRepository = new LocationRepository();
            checkPointRepository = new CheckPointRepository();
            tourImageRepository = new TourImageRepository();
            tourInstanceRepository = new TourInstanceRepository();
            guideRepository = new GuideRepository();
            TourPoints = new ObservableCollection<CheckPoint>();
            TourImages = new ObservableCollection<TourImage>();
            Instances = new ObservableCollection<TourInstance>();
            TodayInstances = todayInstances;
            FutureInstances = futureInstances;
            Countries = new ObservableCollection<string>(locationRepository.GetAllCountries());
            CitiesByCountry = new ObservableCollection<string>();
            ComboBoxCity.IsEnabled = false;
            loggedInUser = user;
           
            currentGuide = guideRepository.GetByUsername(user.Username);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        private void AddTour_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {

                Location newLocation = locationRepository.GetByCityAndCountry(Country, City);

                Tour newTour = new Tour(namet, Convert.ToInt32(maxGuests), Convert.ToDouble(duration), newLocation, description, LanguageT);
                Tour savedTour = tourRepository.Save(newTour);
                tourId = savedTour.Id;
                saved = savedTour;

                UpdateCheckPoints();
                AddImages();
                SaveInstances(savedTour);
               
            }

        }
        private bool IsValid()
        {
            return IsNameValid() & IsMaximuGuestsNumberValid() & IsDurationValid() & IsCountryValid() & IsCityValid() & IsDescriptionValid()
                    & IsLanguageValid() & IsDateTimeValid()
                    & IsCheckPointsValid() & IsImagesValid();

        }
        private bool IsNameValid()
        {
            var content = TourNameTB.Text;
            var regex = @"[A-Za-z]+(\\ [A-Za-z]+)*$";
            Match match = Regex.Match(content, regex, RegexOptions.IgnoreCase);
            bool valid = false;
            if (TourNameTB.Text.Trim().Equals(""))
            {

                TourNameTB.BorderBrush = Brushes.Red;
                TourNameTB.BorderThickness = new Thickness(1);
                NameLabel.Content = "This field can't be empty";
            }
            else if (!match.Success)
            {

                TourNameTB.BorderBrush = Brushes.Red;
                TourNameTB.BorderThickness = new Thickness(1);
                NameLabel.Content = "Invalid name";
            }
            else
            {
                valid = true;
                TourNameTB.BorderBrush = Brushes.Green;
                NameLabel.Content = string.Empty;
            }
            return valid;
        }
        private bool IsMaximuGuestsNumberValid()
        {
            var content = MaxGuestsTB.Text;
            var regex = @"^[1-9]\d*$";
            Match match = Regex.Match(content, regex, RegexOptions.IgnoreCase);
            bool valid = false;
            if (MaxGuestsTB.Text.Trim().Equals(""))
            {

                MaxGuestsTB.BorderBrush = Brushes.Red;
                MaxGuestsTB.BorderThickness = new Thickness(1);
                MaxGuestLabel.Content = "This field can't be empty";
            }
            else if (!match.Success)
            {
                MaxGuestLabel.Content = "This field should be positive number";
                MaxGuestsTB.BorderBrush = Brushes.Red;
                MaxGuestsTB.BorderThickness = new Thickness(1);
            }
            else
            {
                valid = true;
                MaxGuestsTB.BorderBrush = Brushes.Green;
                MaxGuestLabel.Content = string.Empty;
            }
            return valid;
        }

        private bool IsDurationValid()
        {
            var content = DurationTB.Text;
            var regex = "(([1-9][0-9]*)(\\.[0-9]+)?)|(0\\.[0-9]+)$";
            var regexMinus = "-";

            Match match = Regex.Match(content, regex, RegexOptions.IgnoreCase);
            Match matchMinus = Regex.Match(content, regexMinus, RegexOptions.IgnoreCase);

            if (DurationTB.Text.Trim().Equals(""))
            {
                DurationTB.BorderBrush = Brushes.Red;
                DurationTB.BorderThickness = new Thickness(1);
                DurationLabel.Content = "This field can't be empty";
                return false;
            }
            else if (!match.Success || Convert.ToDouble(match.ToString()) == 0.0 || Convert.ToString(match).Equals('0') || matchMinus.Success)
            {
                DurationTB.BorderBrush = Brushes.Red;
                DurationLabel.Content = "Invalid number";
                DurationTB.BorderThickness = new Thickness(1);
                return false;
            }
            else if (match.Success)
            {
                DurationTB.BorderBrush = Brushes.Green;
                DurationLabel.Content = string.Empty;
                return true;
            }
            return false;
        }



        private bool IsCityValid()
        {
            if (ComboBoxCity.SelectedItem == null)
            {
                ComboBoxCity.BorderThickness = new Thickness(1);
                ComboBoxCity.BorderBrush = Brushes.Red;
                CityLabel.Content = "Can't be empty";
                return false;
            }
            else
            {
                ComboBoxCity.BorderThickness = new Thickness(1);
                ComboBoxCity.BorderBrush = Brushes.Green;
                CityLabel.Content = string.Empty;
                return true;
            }
        }


        private bool IsCountryValid()
        {
            if (ComboBoxCountry.SelectedItem == null)
            {
                ComboBoxCountry.BorderThickness = new Thickness(1);
                ComboBoxCountry.BorderBrush = Brushes.Red;
                CountryLabel.Content = "Can't be empty";
                return false;
            }
            else
            {
                ComboBoxCountry.BorderThickness = new Thickness(1);
                ComboBoxCountry.BorderBrush = Brushes.Green;
                CountryLabel.Content = string.Empty;
                return true;
            }
        }


        private bool IsLanguageValid()
        {
            var content = LanguageTB.Text;
            var regex = @"[A-Za-z]+(\\ [A-Za-z]+)*$";
            Match match = Regex.Match(content, regex, RegexOptions.IgnoreCase);
            bool valid = false;
            if (LanguageTB.Text.Trim().Equals(""))
            {

                LanguageTB.BorderBrush = Brushes.Red;
                LanguageTB.BorderThickness = new Thickness(1);
                LanguageLabel.Content = "This field can't be empty";
            }
            else if (!match.Success)
            {

                LanguageTB.BorderBrush = Brushes.Red;
                LanguageTB.BorderThickness = new Thickness(1);
                LanguageLabel.Content = "Only can contains letters and one space between words";
            }
            else
            {
                valid = true;
                LanguageTB.BorderBrush = Brushes.Green;
                LanguageLabel.Content = string.Empty;
            }
            return valid;
        }


        private bool IsDescriptionValid()
        {
            var content = DescriptionTB.Text;
            var regex = @"[A-Za-z]([A-Za-z0-9]|.)*(\\ [A-Za-z0-9]+)*$";
            Match match = Regex.Match(content, regex, RegexOptions.IgnoreCase);
            bool valid = false;
            if (DescriptionTB.Text.Trim().Equals(""))
            {

                DescriptionTB.BorderBrush = Brushes.Red;
                DescriptionTB.BorderThickness = new Thickness(1);
                DescriptionLabel.Content = "This field can't be empty";
            }
            else if (!match.Success)
            {

                DescriptionTB.BorderBrush = Brushes.Red;
                DescriptionTB.BorderThickness = new Thickness(1);
                DescriptionLabel.Content = "Invalid description";
            }
            else
            {
                valid = true;
                DescriptionTB.BorderBrush = Brushes.Green;
                DescriptionLabel.Content = string.Empty;
            }
            return valid;

        }

        private bool IsCheckPointsValid()
        {
            if (TourPoints.Count >= 2)
            {
                PointsGrid.BorderBrush = Brushes.Green;
                PointLabel.Content = string.Empty;
                return true;
            }
            else
            {
                PointsGrid.BorderBrush = Brushes.Red;
                PointsGrid.BorderThickness = new Thickness(1);
                PointLabel.Content = "There must be at least 2 checkpoints";
                return false;
            }
        }

        private bool IsImagesValid()
        {
            if (TourImages.Count >= 1)
            {
                ImagesGrid.BorderBrush = Brushes.Green;
                ImageLabel.Content = string.Empty;
                return true;
            }
            else
            {
                ImagesGrid.BorderBrush = Brushes.Red;
                ImageLabel.Content = "There should be at least 1 tour image";
                return false;
            }
        }


        private bool IsDateTimeValid()
        {
            if (Instances.Count == 0)
            {
                DateTimeBox.BorderBrush = Brushes.Red;
                DateTimeLabel.Content = "This field can't be empty";
                return false;
            }
            else
            {
                DateTimeBox.BorderBrush = Brushes.Green;
                DateTimeLabel.Content = string.Empty;
                return true;
            }
        }
        private void SaveInstances(Tour savedTour)
        {
            TourInstanceRepository tourInstanceRepository = new TourInstanceRepository();
            foreach (TourInstance instance in Instances)
            {
                instance.Tour = savedTour;
                instance.CoverImage = tourImageRepository.GetByTour(savedTour.Id)[0].Url;
                tourInstanceRepository.Save(instance);
                DisplayIfToday(instance);
                DisplayIfCancelable(instance);
            }
        }

        private void DisplayIfCancelable(TourInstance tour)
        {
            if (tour.Finished == false && tour.StartDate > DateTime.Now.Date)
            {
                var prevDate = Convert.ToDateTime(tour.StartDate.ToString().Split(" ")[0] + " " + tour.StartClock);
                var today = DateTime.Now;
                var diffOfDates = today - prevDate;

                if (diffOfDates.Days < -2)
                    FutureInstances.Add(tour);
                else if (diffOfDates.Days == -2 && diffOfDates.Hours < 0)
                    FutureInstances.Add(tour);
                else if (diffOfDates.Days == -2 && diffOfDates.Hours == 0 && diffOfDates.Minutes < 0)
                    FutureInstances.Add(tour);
            }
        }
        private void DisplayIfToday(TourInstance instance)
        {
            if (instance.StartDate.Equals(DateTime.Today))
            {
                if (instance.Finished == false)
                {
                    string hour = instance.StartClock.Split(':')[0];
                    string min = instance.StartClock.Split(":")[1];
                    string sec = instance.StartClock.Split(":")[2];

                    if (IsTimeFromFuture(hour, min, sec))
                    {
                        TodayInstances.Add(instance);
                    }

                }
            }
        }

        private bool IsTimeFromFuture(string hour, string min, string sec)
        {
            return ((Convert.ToInt32(hour) > DateTime.Now.Hour) || (Convert.ToInt32(hour) == DateTime.Now.Hour && Convert.ToInt32(min) > DateTime.Now.Minute) || (Convert.ToInt32(hour) == DateTime.Now.Hour && Convert.ToInt32(min) == DateTime.Now.Minute && Convert.ToInt32(sec) > DateTime.Now.Second));
        }

        private void AddImages()
        {
            List<TourImage> tourImages = tourImageRepository.GetAll();
            foreach (TourImage image in tourImages)
            {
                if (image.TourId == -1)
                {
                    image.TourId = tourId;
                    tourImageRepository.Update(image);
                }
            }
        }

        private void UpdateCheckPoints()
        {
            List<CheckPoint> checkPoints = checkPointRepository.GetAll();
            int i = 1;
            foreach (CheckPoint checkPoint in checkPoints)
            {
                if (checkPoint.TourId == -1)
                {
                    checkPoint.TourId = tourId;
                    checkPoint.Order = i;
                    checkPointRepository.Update(checkPoint);
                    i++;
                }
            }
        }



        private void CancelTour_Click(object sender, RoutedEventArgs e)
        {
            List<CheckPoint> checkPoints = checkPointRepository.GetAll();
            foreach (CheckPoint checkPoint in checkPoints)
            {
                if (checkPoint.TourId == -1)
                {
                    checkPointRepository.Delete(checkPoint);
                }
            }

            List<TourImage> tourImages = tourImageRepository.GetAll();
            foreach (TourImage image in tourImages)
            {
                if (image.TourId == -1)
                {
                    tourImageRepository.Delete(image);
                }
            }


        }

        private void AddTourImage_Click(object sender, RoutedEventArgs e)
        {
            TourImageForm tourImageForm = new TourImageForm(tourImageRepository, TourImages);
            tourImageForm.Show();

           /*  OpenFileDialog op = new OpenFileDialog();
             op.Title = "Select a picture";
             op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
               "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
               "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                string fullPath = op.FileName;


                string[] parts = fullPath.Split('\\');
                fullPath = parts[parts.Length - 1];
                string im = "/Resources/Images/" + fullPath;
                fullPath = im;
                ImageUrl = fullPath;
                TourImage newImage = new TourImage();
                newImage.Url = fullPath;
                newImage.TourId = -1;
                TourImage savedImage = tourImageRepository.Save(newImage);
                TourImages.Add(savedImage);


            }*/
        }


        private void ComboBoxCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxCountry.SelectedItem != null)
            {
                CitiesByCountry.Clear();
                foreach (string city in locationRepository.GetCitiesByCountry((string)ComboBoxCountry.SelectedItem))
                {
                    CitiesByCountry.Add(city);
                }
                ComboBoxCity.IsEnabled = true;
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            newInstance = new TourInstance();
            if (IsDateValid() && IsTimeValid())
            {
                newInstance.StartDate = InstanceStartDate;
                newInstance.StartClock = InstanceStartHour;
                newInstance.Guide = currentGuide;
                newInstance.CoverImage = "";
                Instances.Add(newInstance);
                InstanceStartHourTB.Clear();
                Picker.Text = "";
                Picker.BorderBrush= new SolidColorBrush(Colors.Green);
            }
        }
        private bool IsDateValid()
        {
            if (InstanceStartDate.Date < DateTime.Now.Date)
            {
                Picker.BorderBrush = Brushes.Red;
                PickerLabel.Content = "Can't choose date from past";
                return false;
            }
            else
            {
                Picker.BorderBrush = Brushes.Green;
                PickerLabel.Content = string.Empty;
                return true;
            }
        }

        private bool IsTimeValid()
        {
            var content = InstanceStartHourTB.Text;
            var regex = "(([0-1][0-9])|(2[0-3]))\\:[0-5][0-9]\\:[0-5][0-9]$";
            Match match = Regex.Match(content, regex, RegexOptions.IgnoreCase);
            bool valid = false;

 
            if (match.Success && InstanceStartDate.Date > DateTime.Now.Date)
            {
                valid = true;
            }
            else if (match.Success && InstanceStartDate.Date == DateTime.Now.Date)
            {
                string times = match.ToString();

                valid = IsTodayTimeValid(times);
            }
            return valid;
        }

        private bool IsTodayTimeValid(string times)
        {
            bool valid = false;
            int hour = Convert.ToInt32(times.Split(':')[0]);
            int minute = Convert.ToInt32(times.Split(':')[1]);
            int second = Convert.ToInt32(times.Split(':')[2]);

            if (IsTodayTimeFromPast(hour, minute, second))
            {
               
                InstanceStartHourTB.BorderBrush = Brushes.Red;
            }
            else if (IsTodayTimeFromFuture(hour, minute, second))
            {
                InstanceStartHourTB.BorderBrush = Brushes.Green;
                
                valid = true;
            }

            return valid;
        }

        private bool IsTodayTimeFromPast(int hour, int minute, int second)
        {
            return ((hour < DateTime.Now.Hour) || (hour == DateTime.Now.Hour && minute < DateTime.Now.Minute) || (hour == DateTime.Now.Hour && minute == DateTime.Now.Minute && second < DateTime.Now.Second));
        }

        private bool IsTodayTimeFromFuture(int hour, int minute, int second)
        {
            return ((hour > DateTime.Now.Hour) || (hour == DateTime.Now.Hour && minute > DateTime.Now.Minute) || (hour == DateTime.Now.Hour && minute == DateTime.Now.Minute && second > DateTime.Now.Second));
        }

        private void CancelTime_Click(object sender, RoutedEventArgs e)
        {
            if(selectedInstance!=null) 
            {
                Instances.Remove(selectedInstance);
                tourInstanceRepository.Delete(selectedInstance);
            }
           
        }

        private void OKCheckPoint_Click(object sender, RoutedEventArgs e)
        {
            if (IsCkeckPointValid())
            {
                CheckPoint newCheckPoint = new CheckPoint(NameT, false, -1, -1);
                CheckPoint savedCheckPoint = checkPointRepository.Save(newCheckPoint);
                TourPoints.Add(savedCheckPoint);
                CheckPointName.Clear();
            }
        }

        private bool IsCkeckPointValid()
        {
            bool valid = false;
            if (CheckPointName.Text.Trim().Equals(""))
            {
                CheckPointName.BorderBrush = Brushes.Red;
                CheckPointName.BorderThickness = new Thickness(1);
                NameLabel.Content = "This field can't be empty";
            }
            else
            {
                valid = true;
                CheckPointName.BorderBrush = Brushes.Green;
                NameLabel.Content = string.Empty;
            }
            return valid;
        }

        private void CancelCheckPoint_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCheckPoint != null)
            {
                TourPoints.Remove(SelectedCheckPoint);
                checkPointRepository.Delete(SelectedCheckPoint);
                CheckPointName.Clear();
            }
        }
      

    }
}
