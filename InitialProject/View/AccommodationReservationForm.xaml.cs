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

            if (StartDate >= EndDate || daysNumberFromCalendar < Convert.ToInt32(numberOfDays.Text) || StartDate<DateTime.Now || StartDate==null || EndDate==null)
            {
                MessageBox.Show("Non valid input, please enter values again!");

            }
            else if (daysNumberFromCalendar < currentAccommodation.MinDaysForReservation || Convert.ToInt32(numberOfDays.Text) < currentAccommodation.MinDaysForReservation)
            {
                MessageBox.Show("The minimum number of days for booking this accommodation is " + currentAccommodation.MinDaysForReservation.ToString());
            }
            
            else
            {
                if(IsReservedAtCertainTime(currentAccommodation.Id))
                {

                }
            }


        }

        public bool IsReservedAtCertainTime(int currentAccommodationId)
        {
            DateTime start = StartDate;
            DateTime end = EndDate;
            List<DateTime> freeDays = new List<DateTime>();
            bool isDayFree = true;

            for(int i=0; i<difference.TotalDays; i++)
            {
                foreach(AccommodationReservation reservation in reservations)
                {
                    if(currentAccommodationId==reservation.AccommodationId)
                    {
                        if(start>=reservation.ComingDate && start<=reservation.LeavingDate)
                        {
                            start.AddDays(1);
                            isDayFree = false;
                            break;
                        }
                    }
                }

                //if a day is free
                if(isDayFree)
                {
                    freeDays.Add(start);
                    start.AddDays(1); 
                }

                isDayFree = true;   //for next day

            }

            if (freeDays.Count>= Convert.ToInt32(numberOfDays.Text))
            {
                freeDays.Sort();

            }
            return false;
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
