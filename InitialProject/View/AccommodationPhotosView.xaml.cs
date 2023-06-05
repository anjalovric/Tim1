using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for AccommodationPhotosView.xaml
    /// </summary>
    public partial class AccommodationPhotosView : Window
    {
        public AccommodationPhotosView(List<string> imagesUrl)
        {
            InitializeComponent();
            this.DataContext = this;
            foreach (string url in imagesUrl)
            { 
                Image image = new Image();
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(url); ;
                bitmapImage.EndInit();
                image.Source = bitmapImage;
                setPhotoDimensions(ref image);
                imagesList.Items.Add(image);
            }
        }

        private void setPhotoDimensions(ref Image image)
        {
            image.Width = imagesList.Width;
            image.Height = imagesList.Height;
            image.VerticalAlignment = VerticalAlignment.Center;
            image.HorizontalAlignment = HorizontalAlignment.Center;
        }
    }
}

