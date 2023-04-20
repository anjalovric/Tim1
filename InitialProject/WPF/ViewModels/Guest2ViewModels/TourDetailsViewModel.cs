using InitialProject.Model;
using InitialProject.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class TourDetailsViewModel:INotifyPropertyChanged
    {
        private Guest2 guest2;  
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
        public TourInstance Selected { get; set; }
        private List<TourImage> images;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RelayCommand NextPhotoCommand { get; set; }
        public RelayCommand PreviousPhotoCommand { get; set; }
        public TourDetailsViewModel(TourInstance currentTourInstance, Guest2 guest2)
        {
            Selected = currentTourInstance;
            this.guest2 = guest2;
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
            TourImageService tourImageService = new TourImageService();
            images = new List<TourImage>(tourImageService.GetByTour(Selected.Tour.Id));
            ImageSource = new BitmapImage(new Uri("/"+images[0].Url, UriKind.Relative));
        }
        private void PreviousPhoto_Executed(object sender)
        {
            currentCounter--;
            if (currentCounter < 0)
                currentCounter = images.Count - 1;
            ImageSource = new BitmapImage(new Uri("/"+images[currentCounter].Url, UriKind.Relative));
        }
        private void NextPhoto_Executed(object sender)
        {
            currentCounter++;
            if (currentCounter >= images.Count)
                currentCounter = 0;
            ImageSource = new BitmapImage(new Uri("/" + images[currentCounter].Url, UriKind.Relative));
        }

    }
}
