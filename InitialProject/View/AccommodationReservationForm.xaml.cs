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
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan difference { get; set; }
        public Accommodation currentAccommodation { get; set; }
        public List<AccommodationReservation> reservations { get; set; }
        private AccommodationReservationRepository accommodationReservationRepository;

        List<DateTime> freeDays;
        List<DateTime> freeDaysHelp;
        List<List<DateTime>> dateTimes;
        
        public AccommodationReservationForm(Accommodation currentAccommodation, ref AccommodationRepository accommodationRepository)
        {
            InitializeComponent();
            this.DataContext = this;
            this.currentAccommodation = currentAccommodation;
            accommodationReservationRepository = new AccommodationReservationRepository();
            this.reservations = new List<AccommodationReservation>(accommodationReservationRepository.GetAll());
            foreach(AccommodationReservation reservation in reservations)
            {
                reservation.currentAccommodation = accommodationRepository.GetAll().Find(accommodationInstance => accommodationInstance.Id == reservation.currentAccommodation.Id);
            }
            freeDays = new List<DateTime>();
            freeDaysHelp = new List<DateTime>();
            dateTimes = new List<List<DateTime>>();
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

        public bool IsValidDateInput()
        {
            return (StartDate <= EndDate && Convert.ToInt32(difference.TotalDays) >= (Convert.ToInt32(numberOfDays.Text)-1) && StartDate.Date > DateTime.Now && StartDate != null && EndDate != null);
        }

        public bool IsEnteredCorrectDateRange()
        {
            return ((Convert.ToInt32(difference.TotalDays)+1) >= currentAccommodation.MinDaysForReservation && Convert.ToInt32(numberOfDays.Text) >= currentAccommodation.MinDaysForReservation);
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            difference = EndDate.Subtract(StartDate);
            int daysNumberFromCalendar = Convert.ToInt32(difference.TotalDays);

            if (!IsValidDateInput())
            {
                MessageBox.Show("Non valid input, please enter values again!");
            }
            else if (!IsEnteredCorrectDateRange())
            {
                MessageBox.Show("The minimum number of days for booking this accommodation is " + currentAccommodation.MinDaysForReservation.ToString());
            }
            else
            {//
                    SuggestAvailableDates(currentAccommodation.Id);
            }
        }

        public bool IsDateAvailable(int currentAccommodationId, DateTime date)
        {
            foreach (AccommodationReservation reservation in reservations)
            {
                if (currentAccommodationId == reservation.currentAccommodation.Id)
                {
                    if (date >= reservation.ComingDate && date <= reservation.LeavingDate)
                    {
                        return false;
                    }
                }
            }
           return true;
        }
        
        public void AddAvailableDateToList(DateTime date)
        {
            freeDays.Add(date);
            freeDaysHelp.Add(date);
            freeDays.Sort();
            freeDaysHelp.Sort();
            if (freeDays.Count == Convert.ToInt32(numberOfDays.Text))
            {
                dateTimes.Add(freeDaysHelp);
                freeDays.Remove(freeDays[0]);
                freeDaysHelp = new List<DateTime>(freeDays);
            }
        }

        public void AddAvailableDateOutRangeToList(DateTime date)
        {
            freeDays.Add(date);
            freeDaysHelp.Add(date);
            freeDays.Sort();
            freeDaysHelp.Sort();
            if (freeDays.Count == Convert.ToInt32(numberOfDays.Text))
            {
                if(AreAvailableDatesConsecutive(freeDays))
                    dateTimes.Add(freeDaysHelp);
                freeDays.Remove(freeDays[0]);
                freeDaysHelp = new List<DateTime>(freeDays);
            }
        }

        public bool AreAvailableDatesConsecutive(List<DateTime> dates)
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

        public void SuggestAvailableDates(int currentAccommodationId)
        {
            reservations = accommodationReservationRepository.GetAll();
            DateTime start = StartDate;
            DateTime end = EndDate;
            freeDays = new List<DateTime>();
            freeDaysHelp = new List<DateTime>();
            dateTimes = new List<List<DateTime>>();
            for (int i = 0; i <= difference.TotalDays; i++)
            {
                if (IsDateAvailable(currentAccommodationId, start))
                    AddAvailableDateToList(start);
                start = start.AddDays(1);
            }
            if (dateTimes.Count > 0)
            {
                DatesForAccommodationReservation datesListWindow = new DatesForAccommodationReservation(currentAccommodation,accommodationReservationRepository);
                if (AvailableDateRangeExists(ref datesListWindow))
                    datesListWindow.Show();
                else
                {
                    FindAvailableDatesOutRange();
                    DisplayAvailableDatesOutRange();
                }     
            }
            else
            {
                FindAvailableDatesOutRange();
                DisplayAvailableDatesOutRange();
            }
                
        }

        public bool AvailableDateRangeExists(ref DatesForAccommodationReservation datesListWindow)
        {
            bool exists = false;
            foreach (List<DateTime> dates in dateTimes)
            {
                if (AreAvailableDatesConsecutive(dates))
                {
                    exists = true;
                    DateTime startDate = dates[0];
                    DateTime endDate = dates[Convert.ToInt32(numberOfDays.Text) - 1];
                    datesListWindow.AddNewDateRange(startDate, endDate);
                }
            }
            return exists;
        }

        public void FindAvailableDatesOutRange()
        {
            freeDays = new List<DateTime>();
            freeDaysHelp = new List<DateTime>();
            dateTimes = new List<List<DateTime>>();
            DateTime start = EndDate;
            while (IsDateAvailable(currentAccommodation.Id, start))
                start = start.AddDays(-1);

            start = start.AddDays(1);
            while(dateTimes.Count < 3)
            {
                if (IsDateAvailable(currentAccommodation.Id, start))
                {
                    AddAvailableDateOutRangeToList(start);  
                }
                start = start.AddDays(1);
            }
        }

        public void DisplayAvailableDatesOutRange()
        {
            DatesForAccommodationReservation datesListWindow = new DatesForAccommodationReservation(currentAccommodation, accommodationReservationRepository);
            foreach (List<DateTime> dates in dateTimes)
            {
                DateTime startDate = dates[0];
                DateTime endDate = dates[Convert.ToInt32(numberOfDays.Text) - 1];
                datesListWindow.AddNewDateRange(startDate, endDate);
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

