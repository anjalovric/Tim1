using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest1Views;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class OwnerAndAccommodationReviewFormViewModel:INotifyPropertyChanged
    {
        private AccommodationReservation reservation;
        public Uri RelativeUri { get; set; }
        private List<AccommodationReviewImage> images;
        public RelayCommand SendCommand { get; set; }
        public RelayCommand AddPhotoCommand { get; set; }
        public RelayCommand NextPhotoCommand { get; set; }
        public RelayCommand PreviousPhotoCommand { get; set; }
        public RelayCommand DeletePhotoCommand { get; set; }
        public int AccommodationCleanliness { get; set; }
        public int OwnerCorrectness { get; set; }
        public string Comments { get; set; }
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
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private Guest1 guest1;
        public OwnerAndAccommodationReviewFormViewModel(Guest1 guest1,AccommodationReservation SelectedCompletedReservation)
        {
            this.reservation = SelectedCompletedReservation;
            this.guest1 = guest1;
            images = new List<AccommodationReviewImage>();
            MakeCommands();
        }
        private bool CanExecute(object sender)
        {
            return true;
        }

        private void MakeCommands()
        {
            SendCommand = new RelayCommand(Send_Executed, CanExecute);
            AddPhotoCommand = new RelayCommand(AddPhoto_Executed, CanExecute);
            NextPhotoCommand = new RelayCommand(NextPhoto_Executed, CanExecute);
            PreviousPhotoCommand = new RelayCommand(PreviousPhoto_Executed, CanExecute);
            DeletePhotoCommand = new RelayCommand(DeletePhoto_Executed, CanExecute);
        }
        private void Send_Executed(object sender)
        {
            if (IsImageUploadValid())
            {
                StoreReview();
                StoreImages();
                MessageBox.Show("Review successfully sent!");
                Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = new MyAccommodationReservationsView(guest1);
            }
            else
                MessageBox.Show("You must upload at least one photo!");
        }
        private void StoreImages()
        {
            AccommodationReviewImageService accommodationReviewImageService = new AccommodationReviewImageService();
            foreach (AccommodationReviewImage image in images)
            {
                accommodationReviewImageService.Add(image);
            }
        }
        private void StoreReview()
        {
            OwnerReviewService ownerReviewService = new OwnerReviewService();
            if (OwnerCorrectness == 0)
                OwnerCorrectness = 1;
            if (AccommodationCleanliness == 0)
                AccommodationCleanliness = 1;
            OwnerReview ownerReview = new OwnerReview(reservation, AccommodationCleanliness, OwnerCorrectness, Comments);
            ownerReviewService.Add(ownerReview);
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
        
        private void AddPhoto_Executed(object sender)
        {
            OpenFileDialog openFileDialog = MakeOpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                String relative = MakeRelativePath(openFileDialog);
                RelativeUri = new Uri("/" + relative, UriKind.Relative);
                ImageSource = new BitmapImage(new Uri("/" + relative, UriKind.Relative));
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
        
        private void DeletePhoto_Executed(object sender)
        {
            if (images.Count != 0)
            {
                for (int i = 0; i < images.Count; i++)
                {
                    if (ImageSource.ToString().Contains(images[i].RelativeUri))
                    {
                        AccommodationReviewImage image = images[i];
                        images.Remove(image);
                        RemoveImage(i);
                    }
                }
            }
        }
        private void NextPhoto_Executed(object sender)
        {
            for (int i = 0; i < images.Count; i++)
            {
                if (ImageSource.ToString().Contains(images[i].RelativeUri))
                {
                    int k = i + 1;
                    if (k < images.Count)
                    {
                        ImageSource = new BitmapImage(new Uri("/" + images[k].RelativeUri, UriKind.Relative));
                        break;
                    }

                    if (k == images.Count)
                    {
                        ImageSource = new BitmapImage(new Uri("/" + images[0].RelativeUri, UriKind.Relative));
                        break;
                    }
                }

            }
        }
        private void PreviousPhoto_Executed(object sender)
        {
            for (int i = 0; i < images.Count; i++)
            {
                if (ImageSource.ToString().Contains(images[i].RelativeUri))
                {
                    int k = i - 1;
                    if (k >= 0)
                    {
                        ImageSource = new BitmapImage(new Uri("/" + images[k].RelativeUri, UriKind.Relative));
                        break;
                    }

                    if (k < 0)
                    {
                        ImageSource = new BitmapImage(new Uri("/" + images[images.Count - 1].RelativeUri, UriKind.Relative));
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
                    ImageSource = new BitmapImage(new Uri("/" + images[k].RelativeUri, UriKind.Relative));
                else
                    ImageSource = new BitmapImage(new Uri("/" + images[images.Count - 1].RelativeUri, UriKind.Relative));
            }
            else
                ImageSource = null;
        }
    }
}
