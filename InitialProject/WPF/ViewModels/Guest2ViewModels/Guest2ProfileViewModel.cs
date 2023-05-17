using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using InitialProject.Model;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class Guest2ProfileViewModel: INotifyPropertyChanged
    {
        public Model.Guest2 Guest2 { get; set; }
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
        private LocationService locationService;
        public Guest2ProfileViewModel(Model.Guest2 guest2)
        {
            Guest2 = guest2;
            string relative = FindImageRelativePath();
            ImageSource = new BitmapImage(new Uri(relative, UriKind.Relative));
            locationService = new LocationService();
            SetLocation();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string FindImageRelativePath()
        {
            return Guest2.ImagePath;
        }
        private void SetLocation()
        {
            List<Location> locations = locationService.GetAll();
            foreach (Location location in locations)
            {
                if (location.Id == Guest2.Location.Id)
                {
                    Guest2.Location.City = location.City;
                    Guest2.Location.Country = location.Country;
                }
            }
        }
    }
}
