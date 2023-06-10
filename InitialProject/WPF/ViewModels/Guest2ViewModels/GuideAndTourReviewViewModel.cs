using DotLiquid.Tags;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;
using SixLabors.ImageSharp;
using Image = System.Windows.Controls.Image;
using InitialProject.WPF.Views.Guest2Views;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using InitialProject.WPF.Views.Guest1Views;
using System.Collections.ObjectModel;
using InitialProject.Help;
using NPOI.SS.Formula.Functions;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class GuideAndTourReviewViewModel:INotifyPropertyChanged
    {
        private GuideAndTourReviewRepository guideAndTourReviewRepository;
        public RelayCommand Language_Increment_Command { get; set; }
        public RelayCommand Knowledge_Increment_Command { get; set; }
        public RelayCommand Facts_Increment_Command { get; set; }
        public RelayCommand Language_Decrement_Command { get; set; }
        public RelayCommand Knowledge_Decrement_Command { get; set; }
        public RelayCommand UploadImageCommand { get; set; }
        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand NextCommand { get; set; }
        public RelayCommand BackCommand { get; set; }
        public RelayCommand Facts_Decrement_Command { get; set; }
        public RelayCommand DeletePhotoCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        private TourReviewImageService tourReviewImageService;
        private Guest2 guest2;
        private TourInstance CurrentTourInstance;
        public List<TourReviewImage> images { get; set; }
        private int reviewId;
        public Uri relativeUri { get; set; }
        public TourReviewImage tourReviewImage;
        private string knowledge;
        public string Knowledge
        {
            get { return knowledge; }
            set
            {
                if (value != knowledge)
                    knowledge = value;
                OnPropertyChanged("Knowledge");
            }
        }
        private string language;
        public string Language
        {
            get { return language; }
            set
            {
                if (value != language)
                    language = value;
                OnPropertyChanged("Language");
            }
        }
        private string interestingFacts;
        public string InterestingFacts
        {
            get { return interestingFacts; }
            set
            {
                if (value != interestingFacts)
                    interestingFacts = value;
                OnPropertyChanged("InterestingFacts");
            }
        }
        private string comment;
        public string Comment
        {
            get { return comment; }
            set
            {
                if (value != comment)
                    comment = value;
                OnPropertyChanged("Comment");
            }
        }
        public BitmapImage imageSource { get; set; }
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
        public ICommand HelpCommandInViewModel { get; }
        private GuideAndTourReviewFormView org;
        public GuideAndTourReviewViewModel(TourInstance tourInstance, Guest2 guest2, GuideAndTourReviewFormView org)
        {
            reviewId = -1;
            guideAndTourReviewRepository = new GuideAndTourReviewRepository();
            tourReviewImageService = new TourReviewImageService();
            this.guest2 = guest2;
            this.org = org;
            CurrentTourInstance = tourInstance;
            images = new List<TourReviewImage>();
            MakeCommands();
            Knowledge = "1";
            Language = "1";
            InterestingFacts = "1";
            Comment = "";
            tourReviewImage = new TourReviewImage();
            HelpCommandInViewModel = new RelayCommand(CommandBinding_Executed);
        }
        private void MakeCommands()
        {
            Language_Increment_Command = new RelayCommand(Language_Increment_Executed, CanExecute);
            Knowledge_Increment_Command = new RelayCommand(Knowledge_Increment_Executed, CanExecute);
            Facts_Increment_Command= new RelayCommand(Facts_Increment_Executed, CanExecute);
            Language_Decrement_Command= new RelayCommand(Language_Decrement_Executed, CanExecute);
            Knowledge_Decrement_Command=new RelayCommand(Knowledge_Decrement_Executed, CanExecute);
            Facts_Decrement_Command = new RelayCommand(Facts_Decrement_Executed,CanExecute);
            UploadImageCommand = new RelayCommand(UploadImage_Executed,CanExecute);
            ConfirmCommand = new RelayCommand(Confirm_Executed, ConfirmCanExecute);
            DeleteCommand = new RelayCommand(Delete_Executed, CanExecute);
            NextCommand = new RelayCommand(NextPhoto_Executed, ChangeImageCanExecute);
            BackCommand = new RelayCommand(PreviousPhoto_Executed, ChangeImageCanExecute);
            DeletePhotoCommand=new RelayCommand(DeletePhoto_Executed,CanExecute);
            CancelCommand = new RelayCommand(Cancel_Executed, CanExecute);
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private void Language_Increment_Executed(object sender)
        {
            int changedNumber;
            if (Convert.ToInt32(Language) < 5)
            {
                changedNumber = Convert.ToInt32(Language) + 1;
                Language = changedNumber.ToString();
            }
        }
        private void Knowledge_Increment_Executed(object sender)
        {
            int changedNumber;
            if (Convert.ToInt32(Knowledge) < 5)
            {
                changedNumber = Convert.ToInt32(Knowledge) + 1;
                Knowledge = changedNumber.ToString();
            }
        }
        private bool ChangeImageCanExecute(object sender)
        {
            if (images.Count > 1)
                return true;
            return false;
        }
        private void Facts_Increment_Executed(object sender)
        {
            int changedNumber;
            if (Convert.ToInt32(InterestingFacts) < 5)
            {
                changedNumber = Convert.ToInt32(InterestingFacts) + 1;
                InterestingFacts = changedNumber.ToString();
            }
        }
        private void Language_Decrement_Executed(object sender)
        {
            int changedNumber;
            if (Convert.ToInt32(Language) > 1)
            {
                changedNumber = Convert.ToInt32(Language) - 1;
                Language = changedNumber.ToString();
            }
        }
        private void Knowledge_Decrement_Executed(object sender)
        {
            int changedNumber;
            if (Convert.ToInt32(Knowledge) > 1)
            {
                changedNumber = Convert.ToInt32(Knowledge) - 1;
                Knowledge = changedNumber.ToString();
            }
        }
        private void Facts_Decrement_Executed(object sender)
        {
            int changedNumber;
            if (Convert.ToInt32(InterestingFacts) > 1)
            {
                changedNumber = Convert.ToInt32(InterestingFacts) - 1;
                InterestingFacts = changedNumber.ToString();
            }
        }

        private void Cancel_Executed(object sender)
        {
            Application.Current.Windows.OfType<GuideAndTourReviewFormView>().FirstOrDefault().Close();
        }
        private bool ConfirmCanExecute(object sender)
        {
            return IsImageUploadValid()&&comment!="";
        }

        private void Confirm_Executed(object sender)
        {
            if (IsImageUploadValid())
            {
                int id = StoreReview().Id;
                StoreImages(id);
                MessageBox.Show("Review successfully sent.");
                Application.Current.Windows.OfType<GuideAndTourReviewFormView>().FirstOrDefault().Close();
            }
            else
                MessageBox.Show("You must upload at least one photo!");
        }

        private void StoreImages(int reviewId)
        {
            TourReviewImageService tourReviewImageService = new TourReviewImageService();
            foreach (TourReviewImage image in images)
            {
                image.GuideAndTourReviewId = reviewId;
                tourReviewImageService.Save(image);
            }
        }
        private GuideAndTourReview StoreReview()
        {
            GuideAndTourReviewService guideAndTourReviewService = new GuideAndTourReviewService();
            GuideAndTourReview guideAndTourReview = new GuideAndTourReview(CurrentTourInstance.Guide.Id, guest2, CurrentTourInstance, Convert.ToInt32(Language), Convert.ToInt32(InterestingFacts), Convert.ToInt32(Knowledge), Comment);
            GuideAndTourReview savedReview= guideAndTourReviewService.Save(guideAndTourReview);
            GuideAndTourReviewNotification reviewNotification = new GuideAndTourReviewNotification(savedReview.Id, savedReview.GuideId);
            ReviewNotificationService reviewNotificationService = new ReviewNotificationService();
            reviewNotificationService.Save(reviewNotification);
            return savedReview;
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

        private void UploadImage_Executed(object sender)
        {
            OpenFileDialog openFileDialog = MakeOpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                String relative = MakeRelativePath(openFileDialog);
                relativeUri = new Uri("/" + relative, UriKind.Relative);
                ImageSource = new BitmapImage(new Uri("/" + relative, UriKind.Relative));
                tourReviewImage = new TourReviewImage(reviewId, relative);
                images.Add(tourReviewImage);
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
                        TourReviewImage image = images[i];
                        images.Remove(image);
                        RemoveImage(i);
                    }
                }
            }
        }

        private void Delete_Executed(object sender)
        {
            Comment = "";
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
                {
                    ImageSource = new BitmapImage(new Uri("/" + images[k].RelativeUri, UriKind.Relative));
                }
                else
                {
                    ImageSource = new BitmapImage(new Uri("/" + images[images.Count - 1].RelativeUri, UriKind.Relative));
                }
            }
            else
            {
                ImageSource = null;
            }
        }
        private void CommandBinding_Executed(object sender)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                string str = ShowToursHelp.GetHelpKey((DependencyObject)focusedControl);
                ShowToursHelp.ShowHelpForGrading(str, org);
            }
        }

    }
}
