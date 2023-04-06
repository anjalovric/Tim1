
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Repository;
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
    public partial class RateTourAndGuide : Window
    {
        private GuideAndTourReviewRepository guideAndTourReviewRepository;
        private TourReviewImageRepository tourReviewImageRepository;
        private String Comment;
        private TourReservation Reservation;
        private Guest2 guest2;
        private TourInstance CurrentTourInstance;
        public int Language;
        public Uri relativeUri { get; set; }
        public int InterestingFacts;
        public int Knowledge;
        public TourReviewImage tourReviewImage;
        public RateTourAndGuide(TourInstance tourInstance,Guest2 guest2)
        {
            InitializeComponent();
            guideAndTourReviewRepository = new GuideAndTourReviewRepository();
            tourReviewImageRepository = new TourReviewImageRepository();
            this.guest2 = guest2;
            CurrentTourInstance = tourInstance;
            //Reservation = reservation;
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
                GuideAndTourReview guideAndTourReview = new GuideAndTourReview(CurrentTourInstance.Guide.Id, guest2.Id, CurrentTourInstance,Language,InterestingFacts, Knowledge,Comment,tourReviewImage); //trebace se lista proslijedjivati
                guideAndTourReviewRepository.Save(guideAndTourReview);
                this.Close();
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
                tourReviewImage = new TourReviewImage(CurrentTourInstance,relative);
                tourReviewImageRepository.Save(tourReviewImage); //ovdje treba ali u listu. provjeriti i za cancel dugme
            }

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            comment.Text = "";
        }
    }
}
