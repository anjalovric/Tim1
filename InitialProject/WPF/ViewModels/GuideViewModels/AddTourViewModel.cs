using InitialProject.Model;
using InitialProject.Service;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace InitialProject.WPF.ViewModels.GuideViewModels
{
    public class AddTourViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<CheckPoint> TourPoints { get; set; }
        public ObservableCollection<TourImage> TourImages { get; set; }
        public ObservableCollection<TourInstance> Instances { get; set; }
        public ObservableCollection<TourInstance> TodayInstances { get; set; }
        public ObservableCollection<TourInstance> FutureInstances { get; set; }
        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> CitiesByCountry { get; set; }
        public ObservableCollection<string> Languages { get; set; }

        private User loggedInUser;
        public TourInstance selectedInstance { get; set; }
        public CheckPoint SelectedCheckPoint { get; set; }

        private string imageUrl;
        public string ImageUrl
        {
            get => imageUrl;
            set
            {
                if (!value.Equals(imageUrl))
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
                    IsErrorMessageVisible = "Hidden";
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
        private string pastError;
        public string PastError

        {
            get => pastError;
            set
            {
                if (value != pastError)
                {
                    pastError = value;
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
        private double duration;
        public double Duration
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

        private int maxGuests;

        public int MaxGuests
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
        private BitmapImage image;
        public BitmapImage Image
        {
            get => image;
            set
            {
                if (value != image)
                {
                    image = value;
                    OnPropertyChanged();
                }
            }
        }
        private string toast;
        public string Toast
        {
            get => toast;
            set
            {
                if (value != toast)
                {
                    toast = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime date;
        public DateTime Date
        {
            get => date;
            set
            {
                if (value != date)
                {
                    date = value;
                    OnPropertyChanged();
                }
            }
        }
        private string isErrorMessageVisible;
        public string IsErrorMessageVisible
        {
            get => isErrorMessageVisible;
            set
            {
                if (value != isErrorMessageVisible)
                {
                    isErrorMessageVisible = value;
                    OnPropertyChanged();
                }
            }
        }
        private string isAvailable;
        public string IsAvailable
        {
            get => isAvailable;
            set
            {
                if (value != isAvailable)
                {
                    isAvailable = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool isComboBoxCityEnabled;
        public bool IsComboBoxCityEnabled
        {
            get => isComboBoxCityEnabled;
            set
            {
                if (value != isComboBoxCityEnabled)
                {
                    isComboBoxCityEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        private int pointsCount;
        public int PointsCount
        {
            get => pointsCount;
            set
            {
                if (value != pointsCount)
                {
                    pointsCount = value;
                    OnPropertyChanged();
                }
            }
        }
        private int datesCount;
        public int DatesCount
        {
            get => datesCount;
            set
            {
                if (value != datesCount)
                {
                    datesCount = value;
                    OnPropertyChanged();
                }
            }
        }
        public Uri relativeUri { get; set; }

        private List<TourImage> images = new List<TourImage>();
        private TourInstance newInstance;
        private int tourId;

        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand AddDateTimeCommand { get; set; }
        public RelayCommand DeleteDateTimeCommand { get; set; }
        public RelayCommand AddCheckPointCommand { get; set; }
        public RelayCommand RemoveImageCommand { get; set; }
        public RelayCommand NextImageCommand { get; set; }
        public RelayCommand PreviousImageCommand { get; set; }
        public RelayCommand AddImageCommand { get; set; }   
        public RelayCommand DeleteCheckPointCommand { get; set; }
        public RelayCommand EnableCityCommand { get; set; }
        public RelayCommand OkToastCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public AddTourViewModel(ObservableCollection<TourInstance> todayInstances, User user, ObservableCollection<TourInstance> futureInstances)
        {             
            TourPoints = new ObservableCollection<CheckPoint>();
            TourImages = new ObservableCollection<TourImage>();
            Instances = new ObservableCollection<TourInstance>();
            TodayInstances = todayInstances;
            FutureInstances = futureInstances;
            loggedInUser = user;
            SetStartState();
        }
        private void SetStartState()
        {
            AddLanguages();
            Toast = "Hidden";
            isErrorMessageVisible = "Hidden";
            MakeCommands();
            MakeListOfLocations();
            Start = DateTime.Now;
            Date = DateTime.Now;
            IsAvailable = "Hidden";
            DatesCount = 0;
            PointsCount = 0;
        }
        private void MakeListOfLocations()
        {
            LocationService locationService = new LocationService();
            Countries = new ObservableCollection<string>(locationService.GetAllCountries());
            CitiesByCountry = new ObservableCollection<string>();
            IsComboBoxCityEnabled = false;
        }
        private void AddLanguages()
        {
            Languages = new ObservableCollection<string>();
            Languages.Add("english");
            Languages.Add("spanish");
            Languages.Add("russian");
            Languages.Add("arabic");
            Languages.Add("serbian");
            Languages.Add("italian");
        }
        private bool CanExecute(object sender)
        {
            return true;
        }

        private void MakeCommands()
        {
            ConfirmCommand = new RelayCommand(Confirm_Executed, CanExecute);
            AddDateTimeCommand = new RelayCommand(OKDateTime_Executed, CanExecute);
            DeleteDateTimeCommand = new RelayCommand(CancelTime_Executed, CanExecute);
            AddCheckPointCommand = new RelayCommand(OKCheckPoint_Executed, CanExecute);
            RemoveImageCommand = new RelayCommand(DeleteImage_Executed, CanExecute);
            NextImageCommand = new RelayCommand(NextImage_Executed, CanExecute);
            PreviousImageCommand = new RelayCommand(PreviousImage_Executed, CanExecute);
            AddImageCommand = new RelayCommand(AddTourImage_Executed, CanExecute);
            DeleteCheckPointCommand = new RelayCommand(CancelCheckPoint_Executed, CanExecute);
            OkToastCommand=new RelayCommand(OkToast_Executed , CanExecute);
            EnableCityCommand= new RelayCommand(EnableCityComboBox_Executed, CanExecute);
        }
        private void Confirm_Executed(object sender)
        {
                LocationService locationService=new LocationService();
                Location newLocation = locationService.GetByCityAndCountry(Country, City);
                TourService tourService = new TourService();

                Tour newTour = new Tour(namet, MaxGuests,Duration, newLocation, description, LanguageT);
                Tour savedTour = tourService.Save(newTour);
                tourId = savedTour.Id;
                SaveInputs(savedTour);
                Toast = "Visible";
        }

        private void SaveInputs(Tour savedTour)
        {
            CheckPointService checkPointService = new CheckPointService();
            checkPointService.SaveCheckPoints(tourId, TourPoints);
            TourImageService tourImageService = new TourImageService();
            tourImageService.SaveTourImages(tourId, images);
            TourInstanceService tourInstanceService = new TourInstanceService();
            tourInstanceService.SaveInstances(savedTour, loggedInUser, FutureInstances, TodayInstances, Instances, images);
        }
        private void AddTourImage_Executed(object sender)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files|*.bmp;*.jpg;*.png";
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == true)
            {
                Uri resource = new Uri(openFileDialog.FileName);
                relativeUri = new Uri("/" + resource.ToString().Substring(resource.ToString().IndexOf("Resources")), UriKind.Relative);
                BitmapImage bitmapImage = new BitmapImage(relativeUri);
                bitmapImage.UriSource = relativeUri;
                CreateImage(resource.ToString().Substring(resource.ToString().IndexOf("Resources")));
            }
        }
        private void CreateImage(String relative)
        {
            Image = new BitmapImage(new Uri("/" + relative, UriKind.Relative));
            TourImage newImage = new TourImage(relative,-1);
            images.Add(newImage);
        }
        public void EnableCityComboBox_Executed(object sender)
        {
            LocationService locationService = new LocationService();
            if (Country != null)
            {
                CitiesByCountry.Clear();
                foreach (string city in locationService.GetCitiesByCountry((string)Country))
                {
                    CitiesByCountry.Add(city);
                }
                IsComboBoxCityEnabled = true;
            }
        }
        private void OKDateTime_Executed(object sender)
        {
            GuideService guideService = new GuideService();
            newInstance = new TourInstance();

                string date = Date.ToString().Split(" ")[0] + " " + InstanceStartDate.ToString().Split(" ")[1] + " " + InstanceStartDate.ToString().Split(" ")[2];
                newInstance.StartDate = Convert.ToDateTime(date);
                newInstance.Date = InstanceStartDate.ToString().Split(' ')[0];
                newInstance.Guide = guideService.GetByUsername(loggedInUser.Username);
                newInstance.CoverImage = "";
            Guide loggedGuide = guideService.GetByUsername(loggedInUser.Username);
            if (IsTimeValid() && IsGuideAvailable(newInstance, loggedGuide.Id))
            {
                Instances.Add(newInstance);
                IsErrorMessageVisible = "Hidden";
                IsAvailable = "Hidden";
                DatesCount++;
            }       
        }
        private bool IsTimeValid()
        {
            if (!(newInstance.StartDate.Date > DateTime.Now.Date || (InstanceStartDate.Date == DateTime.Now.Date && InstanceStartDate > DateTime.Now)))
            {
                var app = (App)Application.Current;
                if (app.Lang.Equals("en-US"))
                    PastError = "Can't choose time from past";
                else
                    PastError = "Ne možete odabrati prošlo vreme";

                IsErrorMessageVisible = "Visible";
                return false;
            }
            return true;
        }
        private bool IsGuideAvailable(TourInstance instance,int guideId)
        {
            AvailableDatesForTour availableDatesForTour = new AvailableDatesForTour();
            if (Duration >= 0.1 && availableDatesForTour.ScheduleTourInstances(instance, instance.StartDate, instance.StartDate.AddHours(Duration), Duration,guideId).Count > 0)
            {
                IsAvailable = "Visible";
                return false;
            }
            return true;
        }
        private void CancelTime_Executed(object sender)
        {
            if (selectedInstance != null)
                Instances.Remove(selectedInstance);
            DatesCount--;
        }
        private void OKCheckPoint_Executed(object sender)
        {
            if (!NameT.Equals("") && NameT.Length>1)
            {
                CheckPoint newCheckPoint = new CheckPoint(NameT, false, -1, -1);
                TourPoints.Add(newCheckPoint);
                NameT = "";
                PointsCount++;
            }
        }
        private void CancelCheckPoint_Executed(object sender)
        {
            if (SelectedCheckPoint != null)
                TourPoints.Remove(SelectedCheckPoint);
                PointsCount--;
        }
        private void NextImage_Executed(object sender)
        {
            for (int i = 0; i < images.Count; i++)
            {
                if (Image.ToString().Contains(images[i].Url))
                {
                    if ((i+1) < images.Count)
                    {
                        Image = new BitmapImage(new Uri("/" + images[(i+1)].Url, UriKind.Relative));
                        break;
                    }
                    if ((i+1) == images.Count)
                    {
                        Image = new BitmapImage(new Uri("/" + images[0].Url, UriKind.Relative));
                        break;
                    }
                }
            }
        }
        private void PreviousImage_Executed(object sender)
        {
            for (int i = 0; i < images.Count; i++)
            {
                if (Image.ToString().Contains(images[i].Url))
                {
                    if ((i-1) >= 0)
                    {
                        Image = new BitmapImage(new Uri("/" + images[(i-1)].Url, UriKind.Relative));
                        break;
                    }
                    if ((i - 1) < 0)
                    {
                        Image = new BitmapImage(new Uri("/" + images[images.Count - 1].Url, UriKind.Relative));
                        break;
                    }
                }
            }
        }
        private void DeleteImage_Executed(object sender)
        {
            TourImageService tourImageService = new TourImageService();
            if (images.Count != 0)
            {
                for (int i = 0; i < images.Count; i++)
                {
                    if (Image.ToString().Contains(images[i].Url))
                    {
                        TourImage tourImage = images[i];
                        tourImageService.Delete(tourImage);
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
                if ((i-1) >= 0)
                    Image = new BitmapImage(new Uri("/" + images[(i-1)].Url, UriKind.Relative));
                else
                    Image = new BitmapImage(new Uri("/" + images[images.Count - 1].Url, UriKind.Relative));
            }
            else
                Image = null;
        }
        private void OkToast_Executed(object sender) 
        {
            IsAvailable = "Hidden";
            IsErrorMessageVisible = "Hidden";
        }
    }
}

