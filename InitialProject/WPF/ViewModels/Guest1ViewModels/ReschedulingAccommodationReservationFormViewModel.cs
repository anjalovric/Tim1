using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows;
using InitialProject.Model;
using InitialProject.WPF.Views.Guest1Views;
using System.Windows.Interactivity;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class ReschedulingAccommodationReservationFormViewModel
    {
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }

        public String Reason { get; set; }

        public AccommodationReservation Reservation { get; set; }

        public RelayCommand BackCommand { get; set; }
        public RelayCommand SendCommand { get; set; }
        public RelayCommand OnPreviewMouseUpCommand { get; set; }


        public ReschedulingAccommodationReservationFormViewModel(AccommodationReservation SelectedNotCompletedReservation)
        {
            this.Reservation = SelectedNotCompletedReservation;
            MakeCommands();
        }

        private void MakeCommands()
        {
            BackCommand = new RelayCommand(Back_Executed, CanExecute);
            SendCommand = new RelayCommand(Send_Executed, CanExecute);
            OnPreviewMouseUpCommand = new RelayCommand(OnPreviewMouseUp_Executed, CanExecute);
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private void Back_Executed(object sender)
        {
            Application.Current.Windows.OfType<ReschedulingAccommodationReservationFormView>().FirstOrDefault().Close();
        }

       

        private void Send_Executed(object sender)
        {

            if (IsValidDateInput())
            { 
                StoreRequest();
                Application.Current.Windows.OfType<ReschedulingAccommodationReservationFormView>().FirstOrDefault().Close();
                MessageBox.Show("Request successfully sent!");
            }
            else
            {
                MessageBox.Show("Please enter valid dates!");
            }
        }

        private void StoreRequest()
        {
            Service.ReschedulingAccommodationRequestService requestService;
            requestService = new Service.ReschedulingAccommodationRequestService();
            Model.ReschedulingAccommodationRequest newRequest = new Model.ReschedulingAccommodationRequest(Reservation, Arrival, Departure, Reason);
            requestService.Add(newRequest);
        }

        private bool IsValidDateInput()
        {
            return (Arrival <= Departure && Arrival.Date > DateTime.Now && Arrival != null && Departure != null);
        }

        private void OnPreviewMouseUp_Executed(Object sender)
        {
            OnPreviewMouseUp(null);
        }
        private void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            if (Mouse.Captured is CalendarItem)
            {
                Mouse.Capture(null);
            }
        }

    }
}
