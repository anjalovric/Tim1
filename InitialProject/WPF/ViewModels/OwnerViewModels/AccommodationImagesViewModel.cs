using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Service;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class AccommodationImagesViewModel : INotifyPropertyChanged
    {
        public Accommodation Accommodation { get; set; }
        public ObservableCollection<AccommodationImage> Images { get; set; }
        private string imageUrl;
        private int imageCounter = 0;
        public RelayCommand NextImageCommand { get; set; }
        public RelayCommand PreviousImageCommand { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        public AccommodationImagesViewModel(Accommodation accommodation)
        {
            Accommodation = accommodation;
            MakeImages();
            ImageUrl = Images[0].Url;
            MakeCommands();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        private void MakeImages()
        {
            AccommodationImageService accommodationImageService = new AccommodationImageService();
            Images = new ObservableCollection<AccommodationImage>(accommodationImageService.GetAllByAccommodation(Accommodation));
        }

        private void MakeCommands()
        {
            NextImageCommand = new RelayCommand(NextImage_Executed, CanExecute);
            PreviousImageCommand = new RelayCommand(PreviousImage_Executed, CanExecute);
        }
        private bool CanExecute(object sender)
        {
            return true;
        }

        private void NextImage_Executed(object sender)
        {
            if (imageCounter != Images.Count - 1)
                imageCounter += 1;
            else
                imageCounter = 0;

            ImageUrl = Images[imageCounter].Url;
        }

        private void PreviousImage_Executed(object sender)
        {
            if (imageCounter != Images.Count - 1)
                imageCounter += 1;
            else
                imageCounter = 0;

            ImageUrl = Images[imageCounter].Url;
        }
    }
}
