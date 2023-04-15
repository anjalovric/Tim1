using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Domain.Model;
using InitialProject.WPF.Views.Guest1Views;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using InitialProject.Service;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class AccommodationDetailsViewModel :INotifyPropertyChanged
    {
        private Guest1 guest1;  //trebace kad dodam dugme za rezervaciju
        private int currentCounter = 0;
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
        public Accommodation SelectedAccommodation { get; set; }
        private List<AccommodationImage> images;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RelayCommand NextPhotoCommand { get; set; }
        public RelayCommand PreviousPhotoCommand { get; set; }
        public AccommodationDetailsViewModel(Accommodation currentAccommodation, Guest1 guest1)//obrisi imagesUrl
        {
            SelectedAccommodation = currentAccommodation;
            SetFirstImage();
            MakeCommands();
        }
        
        private void MakeCommands()
        {
            NextPhotoCommand = new RelayCommand(NextPhoto_Executed, CanExecute);
            PreviousPhotoCommand = new RelayCommand(PreviousPhoto_Executed, CanExecute);
        }
        private bool CanExecute(object sender)
        {
            return true;
        }

        private void SetFirstImage()
        {
            AccommodationImageService accommodationImageService = new AccommodationImageService();
            images = new List<AccommodationImage>(accommodationImageService.GetAllByAccommodation(SelectedAccommodation));
            ImageSource = new BitmapImage(new Uri(images[0].Url, UriKind.Relative));
        }
        private void PreviousPhoto_Executed(object sender)
        {
            currentCounter--;
            if (currentCounter < 0)
                currentCounter = images.Count - 1;
            ImageSource = new BitmapImage(new Uri(images[currentCounter].Url, UriKind.Relative));
        }
        private void NextPhoto_Executed(object sender)
        {
            currentCounter++;
            if (currentCounter >= images.Count)
                currentCounter = 0;
            ImageSource = new BitmapImage(new Uri(images[currentCounter].Url, UriKind.Relative));
        }

    }
}
