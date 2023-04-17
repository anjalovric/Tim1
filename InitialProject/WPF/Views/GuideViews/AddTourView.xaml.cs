using InitialProject.Model;
using InitialProject.Repository;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        public ObservableCollection<string> Languages { get; set; }

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
            set{
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
        private bool citySelected;
        public bool CitySelected
        {
            get => citySelected;
            set
            {
                if (value != citySelected)
                {
                    citySelected = value;
                    OnPropertyChanged();
                }
            }
        }
        public Uri relativeUri { get; set; }
        private List<TourImage> images=new List<TourImage>();
        public AddTourView(ObservableCollection<TourInstance> todayInstances,User user, ObservableCollection<TourInstance> futureInstances)
        {
            InitializeComponent();
            DataContext = this;
            CitySelected = false;
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
            Languages = new ObservableCollection<string>();
            AddLanguages();
            ComboBoxCity.IsEnabled = false;
            loggedInUser = user;
            currentGuide = guideRepository.GetByUsername(user.Username);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void AddLanguages()
        {
            Languages.Add("english");
            Languages.Add("spanish");
            Languages.Add("russian");
            Languages.Add("arabic");
            Languages.Add("serbian");
            Languages.Add("italian");
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
                Toast.Visibility = Visibility.Visible;             
            }
        }
        private bool IsValid()
        {
            return  IsDateTimeValid()
                    & IsCheckPointsValid() & IsImagesValid();
        }
        private bool IsCheckPointsValid()
        {
            if (TourPoints.Count >= 2)
            {
                PointsGrid.BorderBrush = Brushes.Green;
                return true;
            }
            else
            {
                PointsGrid.BorderBrush = Brushes.Red;
                PointsGrid.BorderThickness = new Thickness(1);
                return false;
            }
        }
        private bool IsImagesValid()
        {
            if (images.Count >= 1)
                return true;
            return false;
        }
        private bool IsDateTimeValid()
        {
            if (Instances.Count == 0)
            {
                DateTimeBox.BorderBrush = Brushes.Red;
                return false;
            }
            else
            {
                DateTimeBox.BorderBrush = Brushes.Green;
                return true;
            }
        }
        private void SaveInstances(Tour savedTour)
        {
            TourInstanceRepository tourInstanceRepository = new TourInstanceRepository();
            foreach (TourInstance instance in Instances)
            {
                instance.Guide = currentGuide;
                instance.Tour = savedTour;
                instance.CoverImage = tourImageRepository.GetByTour(savedTour.Id)[0].Url;
                instance.CoverBitmap = new BitmapImage(new Uri("/" + instance.CoverImage, UriKind.Relative));
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
                        TodayInstances.Add(instance);
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
                    checkPointRepository.Delete(checkPoint);
            }
            List<TourImage> tourImages = tourImageRepository.GetAll();
            foreach (TourImage image in tourImages)
            {
                if (image.TourId == -1)
                    tourImageRepository.Delete(image);
            }
        }
        private void AddTourImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files|*.bmp;*.jpg;*.png";
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == true)
            {
                Uri resource = new Uri(openFileDialog.FileName);
                String absolutePath = resource.ToString();
                int relativeIndex = absolutePath.IndexOf("Resources");
                String relative = absolutePath.Substring(relativeIndex);
                relativeUri = new Uri("/" + relative, UriKind.Relative);
                BitmapImage bitmapImage = new BitmapImage(relativeUri);
                bitmapImage.UriSource = relativeUri;
                CreateImage(relative);
            }
        }
        private void CreateImage(String relative)
        {
            imagePicture.Source = new BitmapImage(new Uri("/" + relative, UriKind.Relative));
            TourImage newImage = new TourImage();
            newImage.Url = relative;
            newImage.TourId = -1;
            TourImage savedImage = tourImageRepository.Save(newImage);
            images.Add(savedImage);
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
                newInstance.StartClock = InstanceStartHour;
                newInstance.StartDate = InstanceStartDate;
                newInstance.Date = InstanceStartDate.ToString().Split(' ')[0];
                newInstance.Guide = currentGuide;
                newInstance.CoverImage = "";
                Instances.Add(newInstance);
                ClearDateForm();
            }
        }
        private void OKToast(object sender, RoutedEventArgs e)
        {
            Toast.Visibility = Visibility.Hidden;
        }
        private void ClearDateForm()
        {
            InstanceStartHourTB.Clear();
            Picker.Text = "";
            Picker.BorderBrush = new SolidColorBrush(Colors.Green);
        }
        private bool IsDateValid()
        {
            if (InstanceStartDate.Date < DateTime.Now.Date)
            {
                Picker.BorderBrush = Brushes.Red;
                return false;
            }
            else
            {
                Picker.BorderBrush = Brushes.Green;
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
                valid = true;
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
            }
            else
            {
                valid = true;
                CheckPointName.BorderBrush = Brushes.Green;
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
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0;i<images.Count;i++)
            {
                if (imagePicture.Source.ToString().Contains(images[i].Url))
                {
                    int k = i + 1;
                    if (k < images.Count)
                    {
                        imagePicture.Source = imagePicture.Source = new BitmapImage(new Uri("/" + images[k].Url, UriKind.Relative));
                        break;
                    }
                    if (k == images.Count)
                    {
                        imagePicture.Source = imagePicture.Source = new BitmapImage(new Uri("/" + images[0].Url, UriKind.Relative));
                        break;
                    }
                }

            }
        }
        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < images.Count; i++)
            {
                if (imagePicture.Source.ToString().Contains(images[i].Url))
                {
                    int k = i - 1;
                    if (k >=0)
                    {
                        imagePicture.Source = imagePicture.Source = new BitmapImage(new Uri("/" + images[k].Url, UriKind.Relative));
                        break;
                    }
                    if (k <0)
                    {
                        imagePicture.Source = imagePicture.Source = new BitmapImage(new Uri("/" + images[images.Count-1].Url, UriKind.Relative));
                        break;
                    }
                }

            }
        }

        private void DeleteImage_Click(object sender, RoutedEventArgs e)
        {
            if (images.Count != 0)
            {
                for (int i = 0; i < images.Count; i++)
                {
                    if (imagePicture.Source.ToString().Contains(images[i].Url))
                    {
                        TourImage tourImage = images[i];
                        tourImageRepository.Delete(tourImage);
                        images.Remove(tourImage);
                        RemoveImage(i);

                    }
                }
            }
        }
        private void RemoveImage(int i)
        {
            if (images.Count > 0)
            {
                int k = i - 1;
                if (k >= 0)
                    imagePicture.Source = imagePicture.Source = new BitmapImage(new Uri("/" + images[k].Url, UriKind.Relative));
                else
                    imagePicture.Source = imagePicture.Source = new BitmapImage(new Uri("/" + images[images.Count - 1].Url, UriKind.Relative));
            }
            else
                imagePicture.Source = null;
        }
    }
}
