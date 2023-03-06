using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
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
        TourImageRepository _tourImageRepository;
        private string _url;
        public string Url
        {
            get => _url;
            set
            {
                if (value != _url)
                {
                    _url = value;
                    OnPropertyChanged();
                }
            }
        }
        public TourImageForm(TourImageRepository tourImageRepository)
        {
            InitializeComponent();
            DataContext = this;
            _tourImageRepository = tourImageRepository;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddTourImage(object sender, RoutedEventArgs e)
        {
            TourImage newImage = new TourImage();
            newImage.Url = Url;
            newImage.TourId = -1;
            TourImage savedImage = _tourImageRepository.Save(newImage);
            this.Close();
        }

        private void CancelTourImage(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
