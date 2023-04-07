using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using InitialProject.Domain.Model;
using InitialProject.Model;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for AccommodationDetails.xaml
    /// </summary>
    public partial class AccommodationDetails : Page
    {
        public Accommodation SelectedAccommodation { get; set; }
        public Frame Main;
        private List<AccommodationImage> images;
        public Uri relativeUri { get; set; }
        public AccommodationDetails(List<string> imagesUrl, Accommodation currentAccommodation, ref Frame Main)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Main = Main;
            this.Main.Content = this;
            SelectedAccommodation = currentAccommodation;
            images = new List<AccommodationImage>();
          
        }
          
        

       
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void NextImageButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < images.Count; i++)
            {
                if (accommodationImage.Source.ToString().Contains(images[i].Url))
                {
                    int k = i + 1;
                    if (k < images.Count)
                    {
                        accommodationImage.Source = new BitmapImage(new Uri("/" + images[k].Url, UriKind.Relative));
                        break;
                    }

                    if (k == images.Count)
                    {
                        accommodationImage.Source = new BitmapImage(new Uri("/" + images[0].Url, UriKind.Relative));
                        break;
                    }
                }

            }
        }

        private void BackImageButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < images.Count; i++)
            {
                if (accommodationImage.Source.ToString().Contains(images[i].Url))
                {
                    int k = i - 1;
                    if (k >= 0)
                    {
                        accommodationImage.Source = new BitmapImage(new Uri("/" + images[k].Url, UriKind.Relative));
                        break;
                    }

                    if (k < 0)
                    {
                        accommodationImage.Source = new BitmapImage(new Uri("/" + images[images.Count - 1].Url, UriKind.Relative));
                        break;
                    }
                }

            }
        }
    }
}
