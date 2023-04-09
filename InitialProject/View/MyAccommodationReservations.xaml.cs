using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using InitialProject.Service;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for MyAccommodationReservations.xaml
    /// </summary>
    public partial class MyAccommodationReservations : Page
    {
        private Frame Main;
        private Guest1 guest1;
        private AccommodationReservationService accommodationReservationService;
        private OwnerReviewService ownerReviewService;
        private CancelAccommodationReservationService cancelAccommodationReservationService;
        private ObservableCollection<AccommodationReservation> completedReservations;
        public ObservableCollection<AccommodationReservation> CompletedReservations
        {
            get { return completedReservations; }
            set
            {
                if (value != completedReservations)
                    completedReservations = value;
                OnPropertyChanged("CompletedReservations");
            }

        }

        private ObservableCollection<AccommodationReservation> notCompletedReservations;
        public ObservableCollection<AccommodationReservation> NotCompletedReservations
        {
            get { return notCompletedReservations; }
            set
            {
                if (value != notCompletedReservations)
                    notCompletedReservations = value;
                OnPropertyChanged("NotCompletedReservations");
            }

        }

        private AccommodationReservation selectedCompletedReservation;
        public AccommodationReservation SelectedCompletedReservation
        {
            get { return selectedCompletedReservation; }
            set
            {
                if (value != selectedCompletedReservation)
                    selectedCompletedReservation = value;
                OnPropertyChanged("SelectedCompletedReservation");
            }

        }

        private AccommodationReservation selectedNotCompletedReservation;
        public AccommodationReservation SelectedNotCompletedReservation
        {
            get { return selectedNotCompletedReservation; }
            set
            {
                if (value != selectedNotCompletedReservation)
                    selectedNotCompletedReservation = value;
                OnPropertyChanged("SelectedNotCompletedReservation");
            }

        }

        public MyAccommodationReservations(Guest1 guest1, ref Frame Main)
        {
            InitializeComponent();
            this.guest1 = guest1;
            DataContext = this;
            this.Main = Main;
            this.DataContext = this;

            accommodationReservationService = new AccommodationReservationService();
            ownerReviewService = new OwnerReviewService();
            cancelAccommodationReservationService = new CancelAccommodationReservationService();
           
            CompletedReservations = new ObservableCollection<AccommodationReservation>(accommodationReservationService.FillCompletedReservations(guest1));
            NotCompletedReservations = new ObservableCollection<AccommodationReservation>(accommodationReservationService.FillUpcomingAndCurrentReservations(guest1));

        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 


        private void RateOwnerButton_Click(object sender, RoutedEventArgs e)
        {
            if(ownerReviewService.HasReview(SelectedCompletedReservation))
            {
                MessageBox.Show("This reservation is already reviewed.");
                return;
            }

            if(SelectedCompletedReservation.Departure<DateTime.Now.AddDays(-5))
            {
                MessageBox.Show("You can't rate this reservation because 5 days have passed since its departure.");
                return;
            }

            OwnerAndAccommodationReviewForm ownerAndAccommodationReviewForm = new OwnerAndAccommodationReviewForm(SelectedCompletedReservation, ref Main, ownerReviewService);

        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

            if(!cancelAccommodationReservationService.IsCancellationAllowed(SelectedNotCompletedReservation))
            {
                MessageBox.Show("You can't cancel this reservation.");
            }
            else
                ConfirmCancellation();

        }
        private void ConfirmCancellation()
        {
            MessageBoxResult result = cancelAccommodationReservationService.ConfirmCancellationMessageBox();
            if (result == MessageBoxResult.Yes)
            {
                cancelAccommodationReservationService.Add(SelectedNotCompletedReservation);
                accommodationReservationService.Delete(SelectedNotCompletedReservation);
                NotCompletedReservations.Remove(SelectedNotCompletedReservation);
            }
        }
        private void ChangeDateButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeDateAccommodationReservationForm form = new ChangeDateAccommodationReservationForm(SelectedNotCompletedReservation);
            form.Show();
        }
    }
}
