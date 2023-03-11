using System;
using System.Collections;
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
        TimeSpan difference { get; set; }

        
        public Accommodation currentAccommodation { get; set; }
        public List<AccommodationReservation> reservations { get; set; }
        private AccommodationReservationRepository accommodationReservationRepository;



        public AccommodationReservationForm(Accommodation currentAccommodation)
        {
            InitializeComponent();
            this.DataContext = this;
            this.currentAccommodation = currentAccommodation;
            accommodationReservationRepository = new AccommodationReservationRepository();
            this.reservations = new List<AccommodationReservation>(accommodationReservationRepository.GetAll());


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
            return (StartDate >= EndDate || Convert.ToInt32(difference.TotalDays) < Convert.ToInt32(numberOfDays.Text) || StartDate < DateTime.Now || StartDate == null || EndDate == null);
        }

        public bool IsEnteredCorrectDateRange()
        {
            return (Convert.ToInt32(difference.TotalDays) < currentAccommodation.MinDaysForReservation || Convert.ToInt32(numberOfDays.Text) < currentAccommodation.MinDaysForReservation);
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            difference = EndDate.Subtract(StartDate);
            int daysNumberFromCalendar = Convert.ToInt32(difference.TotalDays);

            if (IsValidDateInput())
            {
                MessageBox.Show("Non valid input, please enter values again!");

            }
            else if (IsEnteredCorrectDateRange())
            {
                MessageBox.Show("The minimum number of days for booking this accommodation is " + currentAccommodation.MinDaysForReservation.ToString());
            }

            else
            {
                if (reservations.Count == 0)
                {
                    AccommodationReservation reservation = new AccommodationReservation(0, currentAccommodation.Id, StartDate, EndDate);
                    accommodationReservationRepository.Add(reservation);
                    this.Close();       //napisati rezerv uspjesno dodata, ili tako nesto
                }
                FindAvailableDatesInRange(currentAccommodation.Id);
                
                if (reservations.Count == 1)
                {
                    AccommodationReservation reservation = new AccommodationReservation(0, currentAccommodation.Id, StartDate, EndDate);
                    accommodationReservationRepository.Add(reservation);


                    this.Close();
                }


            }


        }

        public bool IsDayAvailable(int currentAccommodationId, DateTime date)
        {
            foreach (AccommodationReservation reservation in reservations)
            {
                if (currentAccommodationId == reservation.AccommodationId)
                {
                    if (date >= reservation.ComingDate && date <= reservation.LeavingDate)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void AddAvailableDateToList(DateTime date, ref List<DateTime> freeDays, ref List<DateTime> freeDaysHelp, ref List<List<DateTime>> dateTimes)
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

        public bool AreDatesConsecutive(List<DateTime> dates)
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

        public void FindAvailableDatesInRange(int currentAccommodationId)
        {
            DateTime start = StartDate;
            DateTime end = EndDate;
            bool freeDateRangeExists = false;    //ptretvoriti u metodu dio vezan za ovo
            List<DateTime> freeDays = new List<DateTime>();
            List<DateTime> freeDaysHelp = new List<DateTime>();
            List<List<DateTime>> dateTimes = new List<List<DateTime>>();

            for (int i = 0; i <= difference.TotalDays; i++)
            {
                if (IsDayAvailable(currentAccommodationId, start))
                {
                    AddAvailableDateToList(start, ref freeDays, ref freeDaysHelp, ref dateTimes);  //potrebno isprazniti liste negdje
                }
                start = start.AddDays(1);
            }

            if (dateTimes.Count > 0)
            {
                DatesForAccommodationReservation datesListWindow = new DatesForAccommodationReservation();
                bool isConsecutive = true;
                foreach (List<DateTime> dates in dateTimes)
                {
                    if (AreDatesConsecutive(dates))
                    {
                        freeDateRangeExists = true;
                        DateTime startDate = dates[0];
                        DateTime endDate = dates[Convert.ToInt32(numberOfDays.Text)-1];
                        datesListWindow.AddNewDateRange(startDate, endDate);
                    } 
                }

                if (freeDateRangeExists)
                    datesListWindow.Show();
                else
                    FindAvailableDatesOutRange();
            }

            else
            {
                MessageBox.Show("No free days."); 
            }
        }

        public void FindAvailableDatesOutRange()
        {

        }




    private void CalendarStartDate_GotMouseCapture(object sender, MouseEventArgs e)
    {
        UIElement originalElement = e.OriginalSource as UIElement;
        if (originalElement != null)
        {
            originalElement.ReleaseMouseCapture();
        }
    }
    private void CalendarEndDate_GotMouseCapture(object sender, MouseEventArgs e)
    {
        UIElement originalElement = e.OriginalSource as UIElement;
        if (originalElement != null)
        {
            originalElement.ReleaseMouseCapture();
        }
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}   
}

