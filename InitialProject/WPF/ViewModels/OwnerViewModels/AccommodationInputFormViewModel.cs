using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views;
using Microsoft.Win32;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class AccommodationInputFormViewModel : INotifyPropertyChanged
    {
        public Owner owner;
        private Location location;
        private string name;
        private AccommodationType type;
        private int capacity;
        private int minDaysForReservation;
        private int minDaysToCancel;
        private string imageUrl;
        private bool isCityComboBoxEnabled;
        private AccommodationService accommodationService;
        private int imageCounter = 0;
        private bool isDemoOn;
        public List<AccommodationType> AccommodationTypes { get; set; }
        public ObservableCollection<AccommodationImage> Images { get; set; }
        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> CitiesByCountry { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand OkCommand { get; set; }
        public RelayCommand AddImageCommand { get; set; }
        public RelayCommand RemoveImageCommand { get; set; }
        public RelayCommand NextImageCommand { get; set; }
        public RelayCommand PreviousImageCommand { get; set; }
        public RelayCommand EnableCityCommand { get; set; }
        public AccommodationInputFormViewModel(Owner owner)
        {
            this.owner = owner;
            accommodationService = new AccommodationService();
            MakeListOfTypes();
            Images = new ObservableCollection<AccommodationImage>();
            MakeListOfLocations();
            InitializeAccommodation();
            MakeCommands();
        }

        private void MakeCommands()
        {
            CancelCommand = new RelayCommand(Cancel_Executed, CanExecute);
            OkCommand = new RelayCommand(Ok_Executed, OKCanExecute);
            AddImageCommand = new RelayCommand(AddImage_Executed, CanExecute);
            RemoveImageCommand = new RelayCommand(RemoveImage_Executed, RemoveImageCanExecute);
            NextImageCommand = new RelayCommand(NextImage_Executed, ChangeImageCanExecute);
            PreviousImageCommand = new RelayCommand(PreviousImage_Executed, ChangeImageCanExecute);
            EnableCityCommand = new RelayCommand(EnableCityComboBox_Executed, CanExecute);
        }
        private void InitializeAccommodation()
        {
            location = new Location();
            Type = new AccommodationType();
            minDaysToCancel = 1;
            minDaysForReservation = 1;
            capacity = 1;
        }

        private bool CanExecute(object sender)
        {
            return true;
        }

        public bool OKCanExecute(object sender)
        {
            if (IsDemoOn)
                return true;
            if(!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Location.City) && Type.Id!=0 && Images.Count > 0)
            {
                return true;
            }
            return false;
        }

        private bool ChangeImageCanExecute(object sender)
        {
            if (Images.Count > 1)
                return true;
            return false;
        }

        private bool RemoveImageCanExecute(object sender)
        {
            if (Images.Count >= 1 && !string.IsNullOrEmpty(ImageUrl))
                return true;
            return false;
        }
        private void Cancel_Executed(object sender)
        {
            AccommodationView accommodationView = new AccommodationView(owner);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = accommodationView;
        }

        private void Ok_Executed(object sender)
        {
            SaveAccommodation();
            AccommodationView accommodationView = new AccommodationView(owner);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = accommodationView;
        }
        private void AddImage_Executed(object sender)
        {
            AddImageFromFileSystem();
        }

        private void RemoveImage_Executed(object sender)
        {
            if (Images.Count > 1)
                ImageUrl = Images[Images.Count - 1].Url;
            else
                ImageUrl = "";

            foreach (AccommodationImage image in Images)
            {
                if (image.Url.Equals(ImageUrl))
                {
                    Images.Remove(image);
                    break;
                }
            }
        }

        private void NextImage_Executed(object sender)
        {
            if (Images.Count > 1)
            {
                if (imageCounter != Images.Count - 1)
                    imageCounter += 1;
                else
                    imageCounter = 0;

                ImageUrl = Images[imageCounter].Url;
            }
        }

        private void PreviousImage_Executed(object sender)
        {
            if(Images.Count > 1)
            {
                if (imageCounter != Images.Count - 1)
                    imageCounter += 1;
                else
                    imageCounter = 0;

                ImageUrl = Images[imageCounter].Url;
            }
        }
        private void MakeListOfLocations()
        {
            LocationService locationService = new LocationService();
            Countries = new ObservableCollection<string>(locationService.GetAllCountries());
            CitiesByCountry = new ObservableCollection<string>();
            isCityComboBoxEnabled = false;
        }

        private void MakeListOfTypes()
        {
            AccommodationTypeService typeService = new AccommodationTypeService();
            AccommodationTypes = typeService.GetAll();
        }
        public bool IsCityComboBoxEnabled
        {
            get { return isCityComboBoxEnabled; }
            set
            {
                if (value != isCityComboBoxEnabled)
                {
                    isCityComboBoxEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ImageUrl
        {
            get { return imageUrl; }
            set
            {
                if (!value.Equals(imageUrl))
                {
                    imageUrl = value;
                    OnPropertyChanged();
                }
            }
        }
        public Location Location
        {
            get { return location; }
            set
            {
                if (value != location)
                {
                    location = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (!value.Equals(name))
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

        public AccommodationType Type
        {
            get { return type; }
            set
            {
                if (value != type)
                {
                    type = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Capacity
        {
            get { return capacity; }
            set
            {
                if (value != capacity)
                {
                    capacity = value;
                    OnPropertyChanged();
                }
            }
        }

        public int MinDaysForReservation
        {
            get { return minDaysForReservation; }
            set
            {
                if (value != minDaysForReservation)
                {
                    minDaysForReservation = value;
                    OnPropertyChanged();
                }
            }
        }

        public int MinDaysToCancel
        {
            get { return minDaysToCancel; }
            set
            {
                if (value != minDaysToCancel)
                {
                    minDaysToCancel = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void EnableCityComboBox_Executed(object sender)
        {
            LocationService locationService = new LocationService();
            if (Location.Country != null)
            {
                CitiesByCountry.Clear();
                foreach (string city in locationService.GetCitiesByCountry(Location.Country))
                {
                    CitiesByCountry.Add(city);
                }
                IsCityComboBoxEnabled = true;
            }
        }
        private void MakeAndAddImage()
        {
            AccommodationImage image = new AccommodationImage();
            image.Url = ImageUrl;
            image.Id = -1;
            Images.Add(image);
        }

        private void SaveImages(Accommodation accommodation)
        {
            AccommodationImageService imageService = new AccommodationImageService();
            foreach (AccommodationImage image in Images)
            {
                image.Accommodation = accommodation;
                image.Id = imageService.Add(image);
            }
        }

        public void SaveAccommodation()
        {
            Accommodation newAccommodation = new Accommodation();
            newAccommodation.Name = Name;
            newAccommodation.Owner = owner;
            newAccommodation.Capacity = Capacity;
            newAccommodation.MinDaysToCancel = MinDaysToCancel;
            newAccommodation.MinDaysForReservation = minDaysForReservation;
            newAccommodation.Location = Location;
            newAccommodation.Type = Type;
            accommodationService.Add(newAccommodation);
            SaveImages(newAccommodation);
            SaveNotification();
        }

        private void SaveNotification()
        {
            OwnerNotificationsService notificationsService = new OwnerNotificationsService();
            notificationsService.Add(Domain.Model.OwnerNotificationType.ACCOMMODATION_ADDED, owner);
        }

        public void AddImageFromFileSystem()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files|*.bmp;*.jpg;*.png";
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == true)
            {
                Uri resource = new Uri(openFileDialog.FileName);
                string absolutePath = resource.ToString();
                int relativeIndex = absolutePath.IndexOf("Resources");
                string relative = absolutePath.Substring(relativeIndex);
                ImageUrl = "/" + relative;
                MakeAndAddImage();
            }
        }

        public bool IsDemoOn
        {
            get { return isDemoOn; }
            set
            {
                if (value != isDemoOn)
                {
                    isDemoOn = value;
                    OnPropertyChanged();
                }
            }
        }

    }
}
