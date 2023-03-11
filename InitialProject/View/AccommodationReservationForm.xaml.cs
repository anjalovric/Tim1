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



        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            difference = EndDate.Subtract(StartDate);
            int daysNumberFromCalendar = Convert.ToInt32(difference.TotalDays);

            if (StartDate >= EndDate || daysNumberFromCalendar < Convert.ToInt32(numberOfDays.Text) || StartDate < DateTime.Now || StartDate == null || EndDate == null)
            {
                MessageBox.Show("Non valid input, please enter values again!");

            }
            else if (daysNumberFromCalendar < currentAccommodation.MinDaysForReservation || Convert.ToInt32(numberOfDays.Text) < currentAccommodation.MinDaysForReservation)
            {
                MessageBox.Show("The minimum number of days for booking this accommodation is " + currentAccommodation.MinDaysForReservation.ToString());
            }

            else
            {
                IsReservedAtCertainTime(currentAccommodation.Id);
                if(reservations.Count==0)
                {
                    AccommodationReservation reservation = new AccommodationReservation(0, currentAccommodation.Id, StartDate, EndDate);
                    accommodationReservationRepository.Add(reservation);
                    

                    this.Close();
                }


            }


        }

        public void IsReservedAtCertainTime(int currentAccommodationId)
        {
            DateTime start = StartDate;
            DateTime end = EndDate;
            List<DateTime> freeDays = new List<DateTime>();
            List<DateTime> freeDaysHelp = new List<DateTime>();
            List<List<DateTime>> dateTimes = new List<List<DateTime>>();
            bool isDayFree = true;

            for (int i = 0; i <= difference.TotalDays; i++)
            {
                foreach (AccommodationReservation reservation in reservations)
                {
                    if (currentAccommodationId == reservation.AccommodationId)
                    {
                        if (start >= reservation.ComingDate && start <= reservation.LeavingDate)
                        {
                            start=start.AddDays(1);
                            isDayFree = false;
                            break;
                        }
                    }
                }

                //if a day is free
                if (isDayFree)
                {
                    freeDays.Add(start);
                    freeDaysHelp.Add(start);
                    freeDays.Sort();
                    freeDaysHelp.Sort();
                    if (freeDays.Count == Convert.ToInt32(numberOfDays.Text))
                    {
                        dateTimes.Add(freeDaysHelp);
                        freeDays.Remove(freeDays[0]);
                        freeDaysHelp=new List<DateTime>(freeDays);
                        
                    }
                    start=start.AddDays(1);
                }

                isDayFree = true;   //for next day

            }

            if (dateTimes.Count > 0)
            {
                DatesForAccommodationReservation datesListWindow = new DatesForAccommodationReservation();
                bool isConsecutive = true;
                foreach (List<DateTime> dates in dateTimes)
                {
                    for (int i = 0; i < Convert.ToInt32(numberOfDays.Text)-1; i++)
                    {
                        if (dates[i+1].Subtract(dates[i]).TotalDays > 1)
                        {
                            isConsecutive = false;
                            break;
                        }
                    }

                    //if all dates in one list are consecutive
                    if (isConsecutive)
                    {
                        DateTime startDate = dates[0];
                        DateTime endDate = dates[Convert.ToInt32(numberOfDays.Text)-1];

                        datesListWindow.AddNewDateRange(startDate, endDate);

                    }

                    isConsecutive = true;

                }

                datesListWindow.Show();
            }

            else
            {
                MessageBox.Show("No free days."); 
            }
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

