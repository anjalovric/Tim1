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
using System.Windows.Xps.Serialization;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class OwnerAndAccommodationReviewFormViewModel:INotifyPropertyChanged
    {
        private Guest1 guest1;
        private List<AccommodationReviewImage> images;
        private AccommodationReservation reservation;
        public Uri RelativeUri { get; set; }
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
        private int levelOfUrgencyIndex;
        public int LevelOfUrgencyIndex
        {
            get { return levelOfUrgencyIndex; }
            set
            {
                if (value != levelOfUrgencyIndex)
                    levelOfUrgencyIndex = value;
                OnPropertyChanged("LevelOfUrgencyIndex");
            }

        }

        public string ConditionsOfAccommodation { get; set; }
        public string LevelOfUrgency { get; set; }
        private bool isResetEnabled;
        public bool IsResetEnabled
        {
            get { return isResetEnabled; }
            set
            {
                if (value != isResetEnabled)
                    isResetEnabled = value;
                OnPropertyChanged("IsResetEnabled");
            }

        }
        private bool isNextEnabled;
        public bool IsNextEnabled
        {
            get { return isNextEnabled; }
            set
            {
                if (value != isNextEnabled)
                    isNextEnabled = value;
                OnPropertyChanged("IsNextEnabled");
            }

        }
        private bool isDeleteEnabled;
        public bool IsDeleteEnabled
        {
            get { return isDeleteEnabled; }
            set
            {
                if (value != isDeleteEnabled)
                    isDeleteEnabled = value;
                OnPropertyChanged("IsDeleteEnabled");
            }

        }
        public RelayCommand SendCommand { get; set; }
        public RelayCommand AddPhotoCommand { get; set; }
        public RelayCommand NextPhotoCommand { get; set; }
        public RelayCommand PreviousPhotoCommand { get; set; }
        public RelayCommand DeletePhotoCommand { get; set; }
        public RelayCommand ResetComboBoxCommand { get; set; }
       
        public RelayCommand UrgencySelectionChangedCommand { get; set; }
        private AccommodationRenovationSuggestionService accommodationRenovationSuggestionService;
        public OwnerAndAccommodationReviewFormViewModel(Guest1 guest1,AccommodationReservation SelectedCompletedReservation)
        {
            this.reservation = SelectedCompletedReservation;
            this.guest1 = guest1;
            Initialize();
            MakeCommands();
        }
        private void Initialize()
        {
            accommodationRenovationSuggestionService = new AccommodationRenovationSuggestionService();
            images = new List<AccommodationReviewImage>();
            LevelOfUrgencyIndex = -1;
            AccommodationCleanliness = 1;
            OwnerCorrectness = 1;
            IsResetEnabled = false;
            IsNextEnabled = false;
            IsDeleteEnabled = false;
        }
        private void MakeCommands()
        {
            SendCommand = new RelayCommand(Send_Executed, CanExecute);
            AddPhotoCommand = new RelayCommand(AddPhoto_Executed, CanExecute);
            NextPhotoCommand = new RelayCommand(NextPhoto_Executed, CanExecute);
            PreviousPhotoCommand = new RelayCommand(PreviousPhoto_Executed, CanExecute);
            DeletePhotoCommand = new RelayCommand(DeletePhoto_Executed, CanExecute);
            ResetComboBoxCommand = new RelayCommand(ResetComboBox_Executed, CanExecute);
            UrgencySelectionChangedCommand = new RelayCommand(UrgencySelectionChanged_Executed, CanExecute);
        }

        //execute commands
        private void ResetComboBox_Executed(object sender)
        {
            LevelOfUrgencyIndex = -1;
            LevelOfUrgency = null;
            IsResetEnabled=false;
        }
        
        private void UrgencySelectionChanged_Executed(object sender)
        {
            if (LevelOfUrgencyIndex != -1)
                IsResetEnabled = true;
        } 
        private void Send_Executed(object sender)
        {
            if (!IsImageUploadValid())
                ShowMessageBoxForInvalidPhotoInput();

            else if (!IsRenovationSuggestionValid())
                ShowMessageBoxForInvalidRenovationSuggestion();

            else
            {
                StoreReview();
                StoreImages();
                StoreRenovationSuggestion();
                ShowMessageBoxForSentReview();
                Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = new MyAccommodationReservationsView(guest1);
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
            EnableButtonsForPhotos();
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
            EnableButtonsForPhotos();
        }

        //other methods
        private void StoreRenovationSuggestion()
        {
            if(ConditionsOfAccommodation!=null && ConditionsOfAccommodation!="" && LevelOfUrgency!=null) //else, dont store renovation suggestion (doesnt exist)
            { 
                if (ConditionsOfAccommodation == null)
                    ConditionsOfAccommodation = "";
                if (LevelOfUrgency == null)
                    LevelOfUrgency = "";

                AccommodationRenovationSuggestion suggestion = new AccommodationRenovationSuggestion(reservation,LevelOfUrgencyIndex+1,ConditionsOfAccommodation);
                accommodationRenovationSuggestionService.Add(suggestion);
            }
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
            OwnerReview ownerReview = new OwnerReview(reservation, AccommodationCleanliness, OwnerCorrectness, Comments);
            ownerReviewService.Add(ownerReview);
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
        
        
        private void EnableButtonsForPhotos()
        {
            if (images.Count >= 2)
                IsNextEnabled = true;
            else
                IsNextEnabled = false;
            if (images.Count >= 1)
                IsDeleteEnabled = true;
            else
                IsDeleteEnabled = false;
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

        //Validation for image input
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
        //Validation for renovation suggestion
        private bool IsRenovationSuggestionValid()
        {
            if ((ConditionsOfAccommodation == null || ConditionsOfAccommodation == "") && LevelOfUrgency == null)
                return true;
            if (ConditionsOfAccommodation == null || ConditionsOfAccommodation == "")
                return false;
            if (LevelOfUrgency == null)
                return false;
            return true;
        }
        //Message boxes for validation
        private void ShowMessageBoxForInvalidPhotoInput()
        {
            Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("You must upload at least one photo!", "/Resources/Images/exclamation.png");
            messageBox.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
            messageBox.ShowDialog();
        }
        private void ShowMessageBoxForInvalidRenovationSuggestion()
        {
            Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("You must fill all fields for renovation suggestion!", "/Resources/Images/exclamation.png");
            messageBox.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
            messageBox.ShowDialog();
        }
        private void ShowMessageBoxForSentReview()
        {
            Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("Successfully sent!", "/Resources/Images/done.png");
            messageBox.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
            messageBox.ShowDialog();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
    }
}
