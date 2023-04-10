using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest1Views;
using NPOI.SS.Formula.Functions;
using System.Windows.Controls;
using System.Windows;
using InitialProject.WPF.Views.GuideViews;
using InitialProject.WPF.Views;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class Guest1HomeViewModel
    {
        private Guest1 guest1;
        private Guest1Service guest1Service;

        public RelayCommand BookingCommand { get; set; }
        public RelayCommand MyReservationsCommand { get; set; }
        public RelayCommand MyProfileCommand { get; set; }
        public RelayCommand SentRequestsCommand { get; set; }
        public RelayCommand SignOutCommand { get; set; }


        public Guest1HomeViewModel(User user)
        {
            guest1Service = new Guest1Service();
            this.guest1 = guest1Service.GetByUsername(user.Username);
            MakeCommands();

        }

        //servise ukloniti iz polja
        //nazivi
        //observ.u serv

        private void MakeCommands()
        {
            BookingCommand = new RelayCommand(Booking_Executed, CanExecute);
            MyReservationsCommand = new RelayCommand(MyReservations_Executed, CanExecute);
            MyProfileCommand = new RelayCommand(MyProfile_Executed, CanExecute);
            SentRequestsCommand = new RelayCommand(SentRequests_Executed, CanExecute);
            SignOutCommand = new RelayCommand(SignOut_Executed, CanExecute);
        }

        private bool CanExecute(object sender)
        {
            return true;
        }

        private void Booking_Executed(object sender)
        {
            Guest1SearchAccommodationView guest1SearchAccommodationView = new Guest1SearchAccommodationView(guest1);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = guest1SearchAccommodationView;
        }

        private void MyReservations_Executed(object sender)
        {
            MyAccommodationReservationsView myReservations = new MyAccommodationReservationsView(guest1);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = myReservations;
        }
        
        private void SentRequests_Executed(object sender)
        {
            SentAccommodationReservationRequestsView sentAccommodationReservationRequests = new SentAccommodationReservationRequestsView(guest1);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = sentAccommodationReservationRequests;
        }
        private void MyProfile_Executed(object sender)
        {
            Guest1Profile guest1Profile = new Guest1Profile(guest1);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = guest1Profile;
        }

        private void SignOut_Executed(object sender)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Close();
        }

    }
}
