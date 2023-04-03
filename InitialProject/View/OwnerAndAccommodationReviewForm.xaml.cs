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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for OwnerAndAccommodationReviewForm.xaml
    /// </summary>
    public partial class OwnerAndAccommodationReviewForm : Page
    {
        private AccommodationReservation reservation;
        private OwnerReviewService ownerReviewService;
        public Frame Main;

        public OwnerAndAccommodationReviewForm(AccommodationReservation reservation, ref Frame Main, OwnerReviewService ownerReviewService)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Main = Main;
            this.Main.Content = this;
            this.reservation = reservation; //trenutna rezervacija koju ocjenjujem
            this.ownerReviewService = ownerReviewService;
        }

        private void SendOwnerReviewButton_Click(object sender, RoutedEventArgs e)
        {
            
            OwnerReview ownerReview = new OwnerReview(reservation, Convert.ToInt32(CleanlinessSlider.Value), Convert.ToInt32(CorrectnessSlider.Value), Comments.Text);
            ownerReviewService.Save(ownerReview);
            NavigationService.GoBack();
            
            
        }

    }
}
