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
        public Frame Main;
        private Guest1 guest1;
       
        private AccommodationReservationService accommodationReservationService;
        private OwnerReviewService ownerReviewService;
        private CancelAccommodationReservationService cancelAccommodationReservationService;



        /*private List<AccommodationImage> accommodationImages;
        private AccommodationImageRepository accommodationImageRepository;*/        //dodati za slike




        private ObservableCollection<AccommodationReservation> completedAccommodationReservations;
        public ObservableCollection<AccommodationReservation> CompletedAccommodationReservations
        {
            get { return completedAccommodationReservations; }
            set
            {
                if (value != completedAccommodationReservations)
                    completedAccommodationReservations = value;
                OnPropertyChanged("CompletedAccommodationReservations");
            }

        }

        private ObservableCollection<AccommodationReservation> notFinishedReservations;
        public ObservableCollection<AccommodationReservation> NotFinishedReservations
        {
            get { return notFinishedReservations; }
            set
            {
                if (value != notFinishedReservations)
                    notFinishedReservations = value;
                OnPropertyChanged("NotFinishedReservations");
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

        private AccommodationReservation selectedUpcomingReservation;
        public AccommodationReservation SelectedUpcomingReservation
        {
            get { return selectedUpcomingReservation; }
            set
            {
                if (value != selectedUpcomingReservation)
                    selectedUpcomingReservation = value;
                OnPropertyChanged("SelectedUpcomingReservation");
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
           
            CompletedAccommodationReservations = new ObservableCollection<AccommodationReservation>(accommodationReservationService.FillCompletedReservations(guest1));
            NotFinishedReservations = new ObservableCollection<AccommodationReservation>(accommodationReservationService.FillUpcomingAndCurrentReservations(guest1));

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
            if(!IsCancellationAllowed())
            {
                MessageBox.Show("You can't cancel this reservation.");
            }
            else
            ConfirmCancellation();

        }

        private bool IsCancellationAllowed()
        {
            return DateTime.Now <= SelectedUpcomingReservation.Arrival.AddHours(-24) && DateTime.Now <= SelectedUpcomingReservation.Arrival.AddDays(-selectedUpcomingReservation.Accommodation.MinDaysToCancel);
        }

        private void ConfirmCancellation()
        {
            MessageBoxResult result = ConfirmCancellationMessageBox();
            if (result == MessageBoxResult.Yes)
            {
                CancelledAccommodationReservation cancelledAccommodationReservation = new CancelledAccommodationReservation(SelectedUpcomingReservation);
                cancelAccommodationReservationService.Add(cancelledAccommodationReservation);
                accommodationReservationService.Delete(SelectedUpcomingReservation);
                NotFinishedReservations = new ObservableCollection<AccommodationReservation>(accommodationReservationService.FillUpcomingAndCurrentReservations(guest1));


            }
        }

        private MessageBoxResult ConfirmCancellationMessageBox()           //smije li ovo biti u prozoru?
        {
            string sMessageBoxText = $"Do you want to cancel this reservation?\n";
            string sCaption = "Cancel reservation";
            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Question;
            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

        private void ChangeDateButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeDateAccommodationReservationForm form = new ChangeDateAccommodationReservationForm();
            form.Show();
        }




    }
}
