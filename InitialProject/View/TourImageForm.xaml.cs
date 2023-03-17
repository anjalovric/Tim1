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
using System.Xml.Linq;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for TourImageForm.xaml
    /// </summary>
    public partial class TourImageForm : Window, INotifyPropertyChanged
    {
        private TourImageRepository tourImageRepository;
        private string url;
        public ObservableCollection<TourImage> tourImages;
        public string Url
        {
            get => url;
            set
            {
                if (value != url)
                {
                    url = value;
                    OnPropertyChanged();
                }
            }
        }
        public TourImageForm(TourImageRepository imageRepository,ObservableCollection<TourImage> images)
        {
            InitializeComponent();
            DataContext = this;
            tourImageRepository = imageRepository;
            tourImages = images;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddTourImage_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                TourImage newImage = new TourImage();
                newImage.Url = Url;
                newImage.TourId = -1;
                TourImage savedImage = tourImageRepository.Save(newImage);
                tourImages.Add(savedImage);
                this.Close();
            }

        }

        private void CancelTourImage_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private bool IsValid()
        {
            bool valid=false;
            if (TourImageUrl.Text.Trim().Equals(""))
            {
                TourImageUrl.BorderBrush = Brushes.Red;
                TourImageUrl.BorderThickness=new Thickness(1);
                ImageLabel.Content = "This field can't be empty";
            }else
            {
                valid = true;
                TourImageUrl.BorderBrush = Brushes.Green;
                ImageLabel.Content = string.Empty;
            }
            return valid;
        }
    }
}
