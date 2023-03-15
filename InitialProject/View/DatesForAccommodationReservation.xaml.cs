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
using System.Windows.Shapes;
using InitialProject.Model;
using InitialProject.Repository;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for DatesForAccommodationReservation.xaml
    /// </summary>
    public partial class DatesForAccommodationReservation : Window
    {
        private Accommodation currentAccommodation;
        private AccommodationReservationRepository accommodationReservationRepository;

        public ObservableCollection<AvailableDatesForAccommodationReservation> availableDatesForAccommodations { get; set; }
        private AvailableDatesForAccommodationReservation selectedDateRange;
        public AvailableDatesForAccommodationReservation SelectedDateRange
        {
            get { return selectedDateRange; }
            set
            {
                if (selectedDateRange != value)
                {
                    selectedDateRange = value;
                    this.OnPropertyChanged("SelectedDateRange");
                }
            }
        }

        public DatesForAccommodationReservation(Accommodation currentAccommodation, AccommodationReservationRepository accommodationReservationRepository)
        {
            InitializeComponent();
            this.DataContext = this;

            this.currentAccommodation = currentAccommodation;
            availableDatesForAccommodations = new ObservableCollection<AvailableDatesForAccommodationReservation>();
            this.accommodationReservationRepository = accommodationReservationRepository;
        }

        private void ChooseDateButton_Click(object sender, RoutedEventArgs e)
        {
            AccommodationGuestsNumberInput guestsNumber = new AccommodationGuestsNumberInput(currentAccommodation, selectedDateRange, accommodationReservationRepository, availableDatesForAccommodations);
            guestsNumber.Owner = this;
            guestsNumber.Show();
        }
        public void AddNewDateRange(DateTime arrivalDate, DateTime departureDate)
        {
            departureDate = departureDate.AddHours(23);
            departureDate = departureDate.AddMinutes(59);
            availableDatesForAccommodations.Add(new AvailableDatesForAccommodationReservation(arrivalDate, departureDate));
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
