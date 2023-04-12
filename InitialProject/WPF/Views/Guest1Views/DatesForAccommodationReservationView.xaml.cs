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
using InitialProject.Service;

namespace InitialProject.WPF.Views.Guest1Views
{
    /// <summary>
    /// Interaction logic for DatesForAccommodationReservation.xaml
    /// </summary>
    public partial class DatesForAccommodationReservationView : Window
    {
        private Accommodation currentAccommodation;
        private Guest1 guest1;

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

        public DatesForAccommodationReservationView(Accommodation currentAccommodation, Guest1 guest1)
        {
            InitializeComponent();
            this.DataContext = this;

            this.guest1 = guest1;
            this.currentAccommodation = currentAccommodation;
            availableDatesForAccommodations = new ObservableCollection<AvailableDatesForAccommodationReservation>();
        }

        private void ChooseDateButton_Click(object sender, RoutedEventArgs e)
        {
            AccommodationGuestsNumberInputView guestsNumber = new AccommodationGuestsNumberInputView(currentAccommodation, selectedDateRange, availableDatesForAccommodations, guest1);
            guestsNumber.Owner = this;
            guestsNumber.Show();
        }
        public void AddNewDateRange(DateTime arrival, DateTime departure)
        {
            departure = departure.AddHours(23);
            departure = departure.AddMinutes(59);
            availableDatesForAccommodations.Add(new AvailableDatesForAccommodationReservation(arrival, departure));
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
