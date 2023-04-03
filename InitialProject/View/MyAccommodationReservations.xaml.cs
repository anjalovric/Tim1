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
        private Guest1 guest1;
        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> CitiesByCountry { get; set; }
        private LocationRepository locationRepository;
        private Location location;

        private AccommodationType accommodationType;
        private AccommodationTypeRepository accommodationTypeRepository;

        private List<AccommodationImage> accommodationImages;
        private AccommodationImageRepository accommodationImageRepository;

        private AccommodationRepository accommodationRepository;
        private AccommodationReservationRepository accommodationReservationRepository;
        private List<Accommodation> accommodations;
        private List<Model.Owner> owners;
        private OwnerRepository ownerRepository;
        public Frame Main;
        private OwnerReviewRepository ownerReviewRepository;
        private List<AccommodationReservation> allReservations;



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

            accommodationRepository = new AccommodationRepository();
            accommodations = new List<Accommodation>(accommodationRepository.GetAll());
            accommodationReservationRepository = new AccommodationReservationRepository();

            ownerRepository = new OwnerRepository();
            owners = new List<Model.Owner>(ownerRepository.GetAll());
            ownerReviewRepository = new OwnerReviewRepository();


            accommodationType = new AccommodationType();
            accommodationTypeRepository = new AccommodationTypeRepository();

            locationRepository = new LocationRepository();
            location = new Location();
            Countries = new ObservableCollection<string>(locationRepository.GetAllCountries());
            CitiesByCountry = new ObservableCollection<string>();
            allReservations = new List<AccommodationReservation>(accommodationReservationRepository.GetAll());
            FillAccommodationsToReservations();
            FillOwnerToAccommodationReservations();
            SetAccommodationTypes();
            SetAccommodationLocations();



            FillCompletedReservations();

            FillUpcomingAndCurrentReservations();

            



        }



        private void FillUpcomingAndCurrentReservations()
        {
            NotFinishedReservations = new ObservableCollection<AccommodationReservation>();

            foreach (AccommodationReservation reservation in allReservations)
            {
                if (reservation.Departure >= DateTime.Now && reservation.Guest.Id == guest1.Id)
                {
                    NotFinishedReservations.Add(reservation);
                }
            }
        }
        private void FillOwnerToAccommodationReservations()
        {
            foreach (AccommodationReservation reservation in allReservations)
            {
                reservation.Accommodation.Owner = owners.Find(owner => owner.Id == reservation.Accommodation.Owner.Id);
            }
        }

        private void FillCompletedReservations()
        {
            CompletedAccommodationReservations = new ObservableCollection<AccommodationReservation>();

            foreach(AccommodationReservation reservation in allReservations)
            {
                if(reservation.Departure<DateTime.Now && reservation.Guest.Id==guest1.Id)
                {
                    CompletedAccommodationReservations.Add(reservation);
                }
            }      
        }

        private void FillAccommodationsToReservations()
        {
            foreach(AccommodationReservation reservation in allReservations)
            {
                reservation.Accommodation = accommodations.Find(accommodation => accommodation.Id == reservation.Accommodation.Id);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

         private void SetAccommodationLocations()
        {
            List<Location> locations = locationRepository.GetAll();
            foreach (AccommodationReservation reservation in allReservations)
            {
                reservation.Accommodation.Location = locations.Find(n => n.Id == reservation.Accommodation.Location.Id);
            }
        }

        private void SetAccommodationTypes()
        {
            List<AccommodationType> types = accommodationTypeRepository.GetAll();
            foreach (AccommodationReservation reservation in allReservations)
            {
                if (types.Find(n => n.Id == reservation.Accommodation.Type.Id) != null)
                {
                    reservation.Accommodation.Type = types.Find(n => n.Id == reservation.Accommodation.Type.Id);
                }
            }
        }

        private void RateOwnerButton_Click(object sender, RoutedEventArgs e)
        {
            if(ownerReviewRepository.HasReview(SelectedCompletedReservation))
            {
                MessageBox.Show("This reservation is already reviewed.");
                return;
            }

            if(SelectedCompletedReservation.Departure<DateTime.Now.AddDays(-5))
            {
                MessageBox.Show("You can't rate this reservation because 5 days have passed since its departure.");
                return;
            }

            OwnerAndAccommodationReviewForm ownerAndAccommodationReviewForm = new OwnerAndAccommodationReviewForm(SelectedCompletedReservation, ref Main, ownerReviewRepository);

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ChangeDateButton_Click(object sender, RoutedEventArgs e)
        {

        }




    }
}
