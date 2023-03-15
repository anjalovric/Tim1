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
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public TimeSpan lengthOfStay { get; set; }
        public Accommodation currentAccommodation { get; set; }
        private AccommodationRepository accommodationRepository;
        public List<AccommodationReservation> reservations { get; set; }
        private AccommodationReservationRepository accommodationReservationRepository;

        List<DateTime> availableDates;
        List<DateTime> availableDatesHelp;
        List<List<DateTime>> availableDateRanges;
        
        public AccommodationReservationForm(Accommodation currentAccommodation, ref AccommodationRepository accommodationRepository)
        {
            InitializeComponent();
            this.DataContext = this;

            this.currentAccommodation = currentAccommodation;
            accommodationReservationRepository = new AccommodationReservationRepository();
            this.accommodationRepository = accommodationRepository;
            this.reservations = new List<AccommodationReservation>(accommodationReservationRepository.GetAll());
            setAccommodation();

            availableDates = new List<DateTime>();
            availableDatesHelp = new List<DateTime>();
            availableDateRanges = new List<List<DateTime>>();
        }

        private void setAccommodation()
        {
            foreach (AccommodationReservation reservation in reservations)
            {
                reservation.Accommodation = accommodationRepository.GetAll().Find(accommodationInstance => accommodationInstance.Id == reservation.Accommodation.Id);
            }
        }

        private void DecrementDaysNumber(object sender, RoutedEventArgs e)
        {
            int changedDaysNumber;
            if (Convert.ToInt32(numberOfDays.Text) > 1)
            {
                changedDaysNumber = Convert.ToInt32(numberOfDays.Text) - 1;
                numberOfDays.Text = changedDaysNumber.ToString();
            }
        }

        private void IncrementDaysNumber(object sender, RoutedEventArgs e)
        {
            int changedDaysNumber;
            changedDaysNumber = Convert.ToInt32(numberOfDays.Text) + 1;
            numberOfDays.Text = changedDaysNumber.ToString();
        }

        private bool IsValidDateInput()
        {
            return (ArrivalDate <= DepartureDate && Convert.ToInt32(lengthOfStay.TotalDays) >= (Convert.ToInt32(numberOfDays.Text) - 1) && ArrivalDate.Date > DateTime.Now && ArrivalDate != null && DepartureDate != null);
        }

        private bool IsEnteredCorrectDateRange()
        {
            return ((Convert.ToInt32(lengthOfStay.TotalDays) + 1) >= currentAccommodation.MinDaysForReservation && Convert.ToInt32(numberOfDays.Text) >= currentAccommodation.MinDaysForReservation);
        }
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            lengthOfStay = DepartureDate.Subtract(ArrivalDate);
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
                    if (date >= reservation.ArrivalDate && date <= reservation.DepartureDate)
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

            DatesForAccommodationReservation datesListWindow = new DatesForAccommodationReservation(currentAccommodation, accommodationReservationRepository);
            if (availableDateRanges.Count > 0 && AvailableDateRangeExists(ref datesListWindow))
                datesListWindow.Show();
            else
            { 
                FindAvailableDatesOutRange();
                DisplayAvailableDatesOutRange();
            }
                
        }

        private void FillDateRangesList(int currentAccommodationId)
        {
            DateTime start = ArrivalDate;
            for (int i = 0; i <= lengthOfStay.TotalDays; i++)
            {
                if (IsDateAvailable(currentAccommodationId, start))
                    AddAvailableDateToList(start);
                start = start.AddDays(1);
            }
        }

        private bool AvailableDateRangeExists(ref DatesForAccommodationReservation datesListWindow)
        {
            bool existed = false;
            foreach (List<DateTime> dates in availableDateRanges)
            {
                if (AreAvailableDatesConsecutive(dates))
                {
                    existed = true;
                    DateTime arrivalDate = dates[0];
                    DateTime departureDate = dates[Convert.ToInt32(numberOfDays.Text) - 1];
                    datesListWindow.AddNewDateRange(arrivalDate, departureDate);
                }
            }
            return existed;
        }

        private void FindAvailableDatesOutRange()
        {
            availableDates = new List<DateTime>();
            availableDatesHelp = new List<DateTime>();
            availableDateRanges = new List<List<DateTime>>();
            DateTime start = DepartureDate;
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
            DatesForAccommodationReservation datesListWindow = new DatesForAccommodationReservation(currentAccommodation, accommodationReservationRepository);
            foreach (List<DateTime> dates in availableDateRanges)
            {
                DateTime arrivalDate = dates[0];
                DateTime departureDate = dates[Convert.ToInt32(numberOfDays.Text) - 1];
                datesListWindow.AddNewDateRange(arrivalDate, departureDate);
            }
            datesListWindow.Show();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
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

