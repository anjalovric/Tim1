using System;
using System.Collections.Generic;
using System.Linq;
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
using InitialProject.Service;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for ChangeDateAccommodationReservationForm.xaml
    /// </summary>
    public partial class ChangeDateAccommodationReservationForm : Window
    {
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }

        public String Reason { get; set; }

        public AccommodationReservation reservation { get; set; }

        

        private ChangeAccommodationReservationDateRequestService requestService;
        public ChangeDateAccommodationReservationForm(AccommodationReservation reservation)
        {
            InitializeComponent();
            this.DataContext = this;
            requestService = new ChangeAccommodationReservationDateRequestService();
            this.reservation = reservation; 
            
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if(IsValidDateInput())
            {
                ChangeAccommodationReservationDateRequest newRequest = new ChangeAccommodationReservationDateRequest(reservation, Arrival, Departure, Reason);
                requestService.Add(newRequest);
                MessageBox.Show("Request successfully sent!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter valid dates!");
            }
        }

        private bool IsValidDateInput()
        {
            return (Arrival <= Departure && Arrival.Date > DateTime.Now && Arrival != null && Departure != null);
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
