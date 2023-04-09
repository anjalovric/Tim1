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
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.View.Owner;
using InitialProject.Service;
using InitialProject.Domain.Model;
using Microsoft.Win32;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.WPF.Views;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for OwnerAndAccommodationReviewForm.xaml
    /// </summary>
    public partial class OwnerAndAccommodationReviewForm : Page
    {
        private AccommodationReservation reservation;
        private OwnerReviewService ownerReviewService;
        private AccommodationReviewImageService accommodationReviewImageService;
        public Uri relativeUri { get; set; }
        private List<AccommodationReviewImage> images;
        public OwnerAndAccommodationReviewForm(AccommodationReservation reservation, OwnerReviewService ownerReviewService)
        {
            InitializeComponent();
            this.DataContext = this;
            Application.Current.Windows.OfType<Guest1Home>().FirstOrDefault().Main.Content = this;
            this.reservation = reservation;
            this.ownerReviewService = ownerReviewService;
            this.accommodationReviewImageService = new AccommodationReviewImageService();
            images = new List<AccommodationReviewImage>();
        }
        private void SendOwnerReviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsImageUploadValid())
            {
                OwnerReview ownerReview = new OwnerReview(reservation, Convert.ToInt32(CleanlinessSlider.Value), Convert.ToInt32(CorrectnessSlider.Value), Comments.Text);
                ownerReviewService.Save(ownerReview);
                StoreImages();
                NavigationService.GoBack();
            }
            else
                MessageBox.Show("You must upload at least one photo!");  
        }
        private void StoreImages()
        {
            foreach(AccommodationReviewImage image in images)
            {
                accommodationReviewImageService.Add(image);
            }
        }
        private bool IsImageUploadValid()
        {
            if (images.Count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = MakeOpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                String relative = MakeRelativePath(openFileDialog);
                relativeUri = new Uri("/" + relative, UriKind.Relative);
                imagePicture.Source = new BitmapImage(new Uri("/" + relative, UriKind.Relative));
                AccommodationReviewImage accommodationReviewImage = new AccommodationReviewImage(reservation, relative);
                images.Add(accommodationReviewImage);
            }
        }
        private String MakeRelativePath(OpenFileDialog openFileDialog)
        {
            Uri resource = new Uri(openFileDialog.FileName);
            String absolutePath = resource.ToString();
            int relativeIndex = absolutePath.IndexOf("Resources");
            String relative = absolutePath.Substring(relativeIndex);
            return relative;
        }
        private OpenFileDialog MakeOpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files|*.bmp;*.jpg;*.png";
            openFileDialog.FilterIndex = 1;
            return openFileDialog;
        }
        private void DeletePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            if (images.Count != 0)
            {
                for (int i = 0; i < images.Count; i++)
                {
                    if (imagePicture.Source.ToString().Contains(images[i].RelativeUri))
                    {
                        AccommodationReviewImage image = images[i];     
                        images.Remove(image);   
                        RemoveImage(i);
                    }
                }
            }
        }
        private void NextImageButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < images.Count; i++)
            {
                if (imagePicture.Source.ToString().Contains(images[i].RelativeUri))
                {
                    int k = i + 1;
                    if (k < images.Count)
                    {
                        imagePicture.Source = imagePicture.Source = new BitmapImage(new Uri("/" + images[k].RelativeUri, UriKind.Relative));
                        break;
                    }

                    if (k == images.Count)
                    {
                        imagePicture.Source = imagePicture.Source = new BitmapImage(new Uri("/" + images[0].RelativeUri, UriKind.Relative));
                        break;
                    }
                }

            }
        }
        private void BackImageButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < images.Count; i++)
            {
                if (imagePicture.Source.ToString().Contains(images[i].RelativeUri))
                {
                    int k = i - 1;
                    if (k >= 0)
                    {
                        imagePicture.Source = imagePicture.Source = new BitmapImage(new Uri("/" + images[k].RelativeUri, UriKind.Relative));
                        break;
                    }

                    if (k < 0)
                    {
                        imagePicture.Source = imagePicture.Source = new BitmapImage(new Uri("/" + images[images.Count - 1].RelativeUri, UriKind.Relative));
                        break;
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
                {
                    imagePicture.Source  = new BitmapImage(new Uri("/" + images[k].RelativeUri, UriKind.Relative));
                }
                else
                {
                    imagePicture.Source = new BitmapImage(new Uri("/" + images[images.Count - 1].RelativeUri, UriKind.Relative));
                }
            }
            else
            {
                imagePicture.Source = null;
            }
        }
    }
}
//88 linija