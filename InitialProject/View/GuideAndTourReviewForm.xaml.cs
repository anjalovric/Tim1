
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for RateTourAndGuide.xaml
    /// </summary>
    public partial class GuideAndTourReviewForm : Window
    {
        private GuideAndTourReviewRepository guideAndTourReviewRepository;
        private TourReviewImageService tourReviewImageService;
        private String Comment;
        private Guest2 guest2;
        private TourInstance CurrentTourInstance;
        public int Language;
        private List<TourReviewImage> images;
        private int reviewId;
        public Uri relativeUri { get; set; }
        public int InterestingFacts;
        public int Knowledge;
        public TourReviewImage tourReviewImage;
        public GuideAndTourReviewForm(TourInstance tourInstance,Guest2 guest2)
        {
            InitializeComponent();
            reviewId = -1;
            guideAndTourReviewRepository = new GuideAndTourReviewRepository();
            tourReviewImageService = new TourReviewImageService();
            this.guest2 = guest2;
            CurrentTourInstance = tourInstance;
            images = new List<TourReviewImage>();
            Language = 1;
            InterestingFacts = 1;
            Knowledge = 1;
            tourReviewImage = new TourReviewImage();
        }
        private void ZnanjeInkrement_Click(object sender, RoutedEventArgs e)
        {
            int changedNumber;
            if (Convert.ToInt32(knowledge.Text) < 5)
            {
                changedNumber = Convert.ToInt32(knowledge.Text) + 1;
                knowledge.Text = changedNumber.ToString();
                Knowledge = Convert.ToInt32(knowledge.Text);
            }
        }
        private void ZnanjeDekrement_Click(object sender, RoutedEventArgs e)
        {
            int changedNumber;
            if (Convert.ToInt32(knowledge.Text) > 1)
            {
                changedNumber = Convert.ToInt32(knowledge.Text) - 1;
                knowledge.Text = changedNumber.ToString();
                Knowledge = Convert.ToInt32(knowledge.Text);
            }
        }
        private void JezikInkrement_Click(object sender, RoutedEventArgs e)
        {
            int changedNumber;
            if (Convert.ToInt32(language.Text) < 5)
            {
                changedNumber = Convert.ToInt32(language.Text) + 1;
                language.Text = changedNumber.ToString();
                Language = Convert.ToInt32(language.Text);
            }
        }
        private void JezikDekrement_Click(object sender, RoutedEventArgs e)
        {
            int changedNumber;
            if (Convert.ToInt32(language.Text) > 1)
            {
                changedNumber = Convert.ToInt32(language.Text) - 1;
                Language = Convert.ToInt32(language.Text);
            }
        }
        private void ZanimljivostiInkrement_Click(object sender, RoutedEventArgs e)
        {
            int changedNumber;
            if (Convert.ToInt32(interestingFacts.Text) < 5)
            {
                changedNumber = Convert.ToInt32(interestingFacts.Text) + 1;
                interestingFacts.Text = changedNumber.ToString();
                InterestingFacts = Convert.ToInt32(interestingFacts.Text);
            }
        }
        private void ZanimljivostiDekrement_Click(object sender, RoutedEventArgs e)
        {
            int changedNumber;
            if (Convert.ToInt32(interestingFacts.Text) > 1)
            {
                changedNumber = Convert.ToInt32(interestingFacts.Text) - 1;
                interestingFacts.Text = changedNumber.ToString();
                InterestingFacts = Convert.ToInt32(interestingFacts.Text);
            }
        }
        private void Rate_Click(object sender, RoutedEventArgs e)
        {
            if (guideAndTourReviewRepository.HasReview(CurrentTourInstance))
            {
                MessageBox.Show("This reservation is already reviewed.");
                this.Close();
            }
            else
            {
                Comment = comment.Text;
                GuideAndTourReview guideAndTourReview = new GuideAndTourReview(CurrentTourInstance.Guide.Id, guest2.Id, CurrentTourInstance,Language,InterestingFacts, Knowledge,Comment); 
                guideAndTourReviewRepository.Save(guideAndTourReview);
                StoreImages();
                foreach(TourReviewImage image in images)
                {
                    if (image.GuideAndTourReviewId == -1)
                    {
                        //image.GuideAndTourReviewId = guideAndTourReview.Id;
                        tourReviewImageService.Update(image,guideAndTourReview.Id);
                    } 
                }
                this.Close();
            }
        }
        

        private void StoreImages()
        {
            foreach (TourReviewImage image in images)
            {
                tourReviewImageService.Save(image);
            }
        }
        private void DeletePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            if (images.Count != 0)
            {
                for (int i = 0; i < images.Count; i++)
                {
                    if (imagePicture.Source.ToString().Contains(images[i].RelativeUri))
                    {
                        TourReviewImage image = images[i];    
                        images.Remove(image);
                        RemoveImage(i);
                    }
                }
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

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog=new OpenFileDialog();
            openFileDialog.Filter = "Image files|*.bmp;*.jpg;*.png";
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == true)
            {
                Uri resource=new Uri(openFileDialog.FileName);
                String absolutePath = resource.ToString();
                int relativeIndex = absolutePath.IndexOf("Resources");
                String relative = absolutePath.Substring(relativeIndex);
                relativeUri = new Uri("/"+relative,UriKind.Relative);
                BitmapImage bitmapImage = new BitmapImage(relativeUri);
                bitmapImage.UriSource = relativeUri;
                imagePicture.Source = new BitmapImage(new Uri("/"+relative, UriKind.Relative));
                tourReviewImage = new TourReviewImage(reviewId, relative);
                images.Add(tourReviewImage);
            }

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            comment.Text = "";
        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
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

        private void BackButton_Click(object sender, RoutedEventArgs e)
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
                    imagePicture.Source = imagePicture.Source = new BitmapImage(new Uri("/" + images[k].RelativeUri, UriKind.Relative));
                }
                else
                {
                    imagePicture.Source = imagePicture.Source = new BitmapImage(new Uri("/" + images[images.Count - 1].RelativeUri, UriKind.Relative));
                }
            }
            else
            {
                imagePicture.Source = null;
            }
        }
    }
}
