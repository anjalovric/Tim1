using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;

namespace InitialProject.WPF.ViewModels
{
    public class OwnerReviewViewModel : INotifyPropertyChanged
    {
        public OwnerReview OwnerReview { get; set; }

        private string imageUrl;
        private List<AccommodationReviewImage> Images { get; set; }
        private int imageCounter;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public OwnerReviewViewModel(OwnerReview ownerReview)
        {
            OwnerReview = ownerReview;
            MakeImagesForReview();
            MakeFirstImage();
            imageCounter = 0;
        }

        private void MakeFirstImage()
        {
            if(Images.Count >0)
            {
                ImageUrl = "/" + Images[0].RelativeUri;
            }
        }

        private void MakeImagesForReview()
        {
            Images = new List<AccommodationReviewImage>();
            AccommodationReviewImageService imageService = new AccommodationReviewImageService();
            Images = imageService.GetAllByReservation(OwnerReview.Reservation);
        }

        public string ImageUrl
        {
            get => imageUrl;
            set
            {
                if (!value.Equals(imageUrl))
                {
                    imageUrl = value;
                    OnPropertyChanged();
                }
            }
        }

        public void GetNextImage()
        {
            if (imageCounter != Images.Count-1)
                imageCounter += 1;
            else
                imageCounter = 0;
            ImageUrl = "/" + Images[imageCounter].RelativeUri;
        }

        public void GetPreviousImage()
        {
            if (imageCounter != 0)
                imageCounter -= 1;
            else
                imageCounter = Images.Count-1;
            ImageUrl = "/" + Images[imageCounter].RelativeUri;
        }
    }
}
