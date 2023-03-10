using InitialProject.Forms;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for TourForm.xaml
    /// </summary>
    public partial class TourForm : Window,INotifyPropertyChanged
    {
        

        private readonly TourRepository _tourRepository;
        private readonly LocationRepository _locationRepository;
        private readonly CheckPointRepository _checkPointRepository;
        private readonly TourImageRepository _tourImageRepository;
        private readonly TourInstanceRepository _tourInstanceRepository;
        public int pointCounter = 0;
        public ObservableCollection<CheckPoint> TourPoints { get; set; }
        public ObservableCollection<TourImage> TourImages { get; set; }
        public ObservableCollection<TourInstance> Instances { get; set; }  
        public ObservableCollection<TourInstance> TodayInstances { get; set; }

        public Tour saved;
        private int _tourId;
        private string _hour;
        public string Hours
        {
            get => _hour;
            set
            {
                if (value != _hour)
                {
                    _hour = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _name;
        public string NameT
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (value != _city)
                {
                    _city = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                if (value != _country)
                {
                    _country = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _language;
        public string LanguageT
        {
            get => _language;
            set
            {
                if (value != _language)
                {
                    _language = value;
                    OnPropertyChanged();
                }
            }
        }
        private double _duration;

        public double Duration
        {
            get => _duration;
            set
            {
                if (value != _duration)
                {
                    _duration = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _maxGuests;

        public int MaxGuests
        {
            get => _maxGuests;
            set
            {
                if (value != _maxGuests)
                {
                    _maxGuests = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime _start;
        public DateTime Start
        {
            get => _start;
            set
            {
                if (value != _start)
                {
                    _start = value;
                    OnPropertyChanged();
                }
            }
        }
        public TourForm(ObservableCollection<TourInstance> todayInstances)
        {
            InitializeComponent();
            DataContext = this;
            _tourRepository=new TourRepository();
            _locationRepository=new LocationRepository();
            _checkPointRepository = new CheckPointRepository();
            _tourImageRepository=new TourImageRepository();
            _tourInstanceRepository=new TourInstanceRepository();
            TourPoints = new ObservableCollection<CheckPoint>();
            TourImages = new ObservableCollection<TourImage>();
            Instances = new ObservableCollection<TourInstance>();
            TodayInstances = todayInstances;
            AddNewTour.IsEnabled= false;
           // NewDate.IsEnabled= false;
             

        }

    public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));  
        }

        

        private void AddTour(object sender, RoutedEventArgs e)
        {
            Location newLocation = new Location(_city, _country);
            Location savedLocation = _locationRepository.Save(newLocation);

            Tour newTour = new Tour(NameT, _maxGuests, _duration, newLocation, _description, LanguageT);
            Tour savedTour = _tourRepository.Save(newTour);
            _tourId = savedTour.Id;
            saved = savedTour;
            //TourInstance newTourInstance = new TourInstance(savedTour, Start, Hours);
            //TourInstance savedTourInstance = _tourInstanceRepository.Save(newTourInstance);

            UpdateCheckPoints();
            AddImages();
            SaveInstances(savedTour);
            this.Close();
            //AddNewTour.IsEnabled = false;
           // Cancel.IsEnabled = false;
            // NewDate.IsEnabled = true;
            
        }

        private void SaveInstances(Tour savedTour)
        {
            TourInstanceRepository tourInstanceRepository = new TourInstanceRepository();
            foreach(TourInstance instance in Instances)
            {
                instance.Tour = savedTour;
                tourInstanceRepository.Save(instance);
                DisplayIfToday(instance);
            }
        }

        private void DisplayIfToday(TourInstance instance)
        {
            if (instance.StartDate.Equals(DateTime.Today))
            {
                TodayInstances.Add(instance);
            }
        }
        private void AddImages()
        {
            List<TourImage> tourImages = _tourImageRepository.GetAll();
            foreach (TourImage image in tourImages)
            {
                if (image.TourId == -1)
                {
                    image.TourId = _tourId;
                    _tourImageRepository.Update(image);
                }
            }
        }

        private void UpdateCheckPoints()
        {
            List<CheckPoint> checkPoints = _checkPointRepository.GetAll();
            int i = 1;
            foreach (CheckPoint checkPoint in checkPoints)
            {
                if (checkPoint.TourId == -1)
                {
                    checkPoint.TourId = _tourId;
                    checkPoint.Order = i;
                    _checkPointRepository.Update(checkPoint);
                    i++;
                }
            }
        }

        private void AddCheckPoint(object sender, RoutedEventArgs e)
        {
            CheckPointForm form = new CheckPointForm(_checkPointRepository,TourPoints,this.AddNewTour);
            form.Show();

        }

        private void CancelTour(object sender, RoutedEventArgs e)
        {
            List<CheckPoint> checkPoints = _checkPointRepository.GetAll();
            foreach (CheckPoint checkPoint in checkPoints)
            {
                if (checkPoint.TourId == -1)
                {
                    _checkPointRepository.Delete(checkPoint);
                }
            }

            List<TourImage> tourImages = _tourImageRepository.GetAll();
            foreach (TourImage image in tourImages)
            {
                if (image.TourId == -1)
                { 
                    _tourImageRepository.Delete(image);
                }
            }
            
            this.Close();
        }

        private void AddTourImage(object sender, RoutedEventArgs e)
        {
            TourImageForm tourImageForm = new TourImageForm(_tourImageRepository,TourImages);
            tourImageForm.Show();
        }

        private void NewInstance(object sender, RoutedEventArgs e)
        {
            NewTourInstanceDate newTourInstance = new NewTourInstanceDate(saved, Instances);
            newTourInstance.Show();
        }

    }
}
