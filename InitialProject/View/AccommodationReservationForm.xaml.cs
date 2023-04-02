using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for AccommodationReservationForm.xaml
    /// </summary>
    public partial class AccommodationReservationForm : Window
    {
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }
        public TimeSpan lengthOfStay { get; set; }
        public Accommodation currentAccommodation { get; set; }
        private AccommodationRepository accommodationRepository;
        public List<AccommodationReservation> reservations { get; set; }
        private AccommodationReservationRepository accommodationReservationRepository;

        private Guest1 guest1;

        private List<DateTime> availableDates;
        private List<DateTime> availableDatesHelp;
        private List<List<DateTime>> availableDateRanges;
        
        public AccommodationReservationForm(Accommodation currentAccommodation, ref AccommodationRepository accommodationRepository, Guest1 guest1)
        {
            InitializeComponent();
            this.DataContext = this;

            this.guest1 = guest1;
            this.currentAccommodation = currentAccommodation;
            accommodationReservationRepository = new AccommodationReservationRepository();
            this.accommodationRepository = accommodationRepository;
            this.reservations = new List<AccommodationReservation>(accommodationReservationRepository.GetAll());
            SetAccommodation();

            availableDates = new List<DateTime>();
            availableDatesHelp = new List<DateTime>();
            availableDateRanges = new List<List<DateTime>>();
        }

        private void SetAccommodation()
        {
            foreach (AccommodationReservation reservation in reservations)
            {
                reservation.Accommodation = accommodationRepository.GetAll().Find(accommodationInstance => accommodationInstance.Id == reservation.Accommodation.Id);
            }
        }

        private void DecrementDaysNumberButton_Click(object sender, RoutedEventArgs e)
        {
            int changedDaysNumber;
            if (Convert.ToInt32(numberOfDays.Text) > 1)
            {
                changedDaysNumber = Convert.ToInt32(numberOfDays.Text) - 1;
                numberOfDays.Text = changedDaysNumber.ToString();
            }
        }

        private void IncrementDaysNumberButton_Click(object sender, RoutedEventArgs e)
        {
            int changedDaysNumber;
            changedDaysNumber = Convert.ToInt32(numberOfDays.Text) + 1;
            numberOfDays.Text = changedDaysNumber.ToString();
        }

        private bool IsValidDateInput()
        {
            return (Arrival <= Departure && Convert.ToInt32(lengthOfStay.TotalDays) >= (Convert.ToInt32(numberOfDays.Text) - 1) && Arrival.Date > DateTime.Now && Arrival != null && Departure != null);
        }

        private bool IsEnteredCorrectDateRange()
        {
            return ((Convert.ToInt32(lengthOfStay.TotalDays) + 1) >= currentAccommodation.MinDaysForReservation && Convert.ToInt32(numberOfDays.Text) >= currentAccommodation.MinDaysForReservation);
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            lengthOfStay = Departure.Subtract(Arrival);
            int daysNumberFromCalendar = Convert.ToInt32(lengthOfStay.TotalDays);

            if (!IsValidDateInput())
            {
                MessageBox.Show("Non valid input, please enter values again!");
            }
            else if (!IsEnteredCorrectDateRange())
            {
                MessageBox.Show("The minimum number of days for booking this accommodation is " + currentAccommodation.MinDaysForReservation.ToString());
            }
            else
            {
                GetAvailableDates(currentAccommodation.Id);
            }
        }

        private bool IsDateAvailable(int currentAccommodationId, DateTime date)
        {
            foreach (AccommodationReservation reservation in reservations)
            {
                if (currentAccommodationId == reservation.Accommodation.Id)
                {
                    if (date >= reservation.Arrival && date <= reservation.Departure)
                    {
                        return false;
                    }
                }
            }
           return true;
        }
        
        private void AddAvailableDateToList(DateTime date)
        {
            availableDates.Add(date);
            availableDatesHelp.Add(date);
            if (availableDates.Count == Convert.ToInt32(numberOfDays.Text))
            {
                availableDateRanges.Add(availableDatesHelp);
                availableDates.Remove(availableDates[0]);
                availableDatesHelp = new List<DateTime>(availableDates);
            }
        }

        private void AddAvailableDateOutRangeToList(DateTime date)
        {
            availableDates.Add(date);
            availableDatesHelp.Add(date);
            if (availableDates.Count == Convert.ToInt32(numberOfDays.Text))
            {
                if(AreAvailableDatesConsecutive(availableDates))
                    availableDateRanges.Add(availableDatesHelp);

                availableDates.Remove(availableDates[0]);
                availableDatesHelp = new List<DateTime>(availableDates);
            }
        }

        private bool AreAvailableDatesConsecutive(List<DateTime> dates)
        {
            for (int i = 0; i < Convert.ToInt32(numberOfDays.Text) - 1; i++)
            {
                if (dates[i + 1].Subtract(dates[i]).TotalDays > 1)
                {
                    return false;
                }
            }
            return true;
        }

        public void GetAvailableDates(int currentAccommodationId)
        {
            reservations = accommodationReservationRepository.GetAll();
            availableDates = new List<DateTime>();
            availableDatesHelp = new List<DateTime>();
            availableDateRanges = new List<List<DateTime>>();
            FillDateRangesList(currentAccommodationId);

            DatesForAccommodationReservation suggestedDates = new DatesForAccommodationReservation(currentAccommodation, accommodationReservationRepository, guest1);
            suggestedDates.Owner = this;
            if (availableDateRanges.Count > 0 && AvailableDateRangeExists(ref suggestedDates))
                suggestedDates.Show();
            else
            { 
                FindAvailableDatesOutRange();
                DisplayAvailableDatesOutRange();
            }
                
        }

        private void FillDateRangesList(int currentAccommodationId)
        {
            DateTime start = Arrival;
            for (int i = 0; i <= lengthOfStay.TotalDays; i++)
            {
                if (IsDateAvailable(currentAccommodationId, start))
                    AddAvailableDateToList(start);
                start = start.AddDays(1);
            }
        }

        private bool AvailableDateRangeExists(ref DatesForAccommodationReservation suggestedDates)
        {
            bool existed = false;
            foreach (List<DateTime> dates in availableDateRanges)
            {
                if (AreAvailableDatesConsecutive(dates))
                {
                    existed = true;
                    DateTime arrival = dates[0];
                    DateTime departure = dates[Convert.ToInt32(numberOfDays.Text) - 1];
                    suggestedDates.AddNewDateRange(arrival, departure);
                }
            }
            return existed;
        }

        private void FindAvailableDatesOutRange()
        {
            availableDates = new List<DateTime>();
            availableDatesHelp = new List<DateTime>();
            availableDateRanges = new List<List<DateTime>>();
            DateTime start = Departure;
            while (IsDateAvailable(currentAccommodation.Id, start))
                start = start.AddDays(-1);

            start = start.AddDays(1);
            FillDatesOutRangeList(ref start);
        }

        private void FillDatesOutRangeList(ref DateTime start)
        {
            while (availableDateRanges.Count < 3)
            {
                if (IsDateAvailable(currentAccommodation.Id, start))
                {
                    AddAvailableDateOutRangeToList(start);
                }
                start = start.AddDays(1);
            }
        }

        private void DisplayAvailableDatesOutRange()
        {
            DatesForAccommodationReservation suggestedDates = new DatesForAccommodationReservation(currentAccommodation, accommodationReservationRepository, guest1);
            suggestedDates.Owner = this;
            foreach (List<DateTime> dates in availableDateRanges)
            {
                DateTime arrival = dates[0];
                DateTime departure = dates[Convert.ToInt32(numberOfDays.Text) - 1];
                suggestedDates.AddNewDateRange(arrival, departure);
            }
            suggestedDates.Show();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            if (Mouse.Captured is CalendarItem)
            {
                Mouse.Capture(null);
            }
        }
    }   
}

