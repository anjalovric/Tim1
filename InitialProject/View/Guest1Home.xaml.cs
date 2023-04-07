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
using InitialProject.Service;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for Guest1Home.xaml
    /// </summary>
    public partial class Guest1Home : Window
    {
        private Guest1SearchAccommodation guest1SearchAccommodation;
        private MyAccommodationReservations myReservations;
        private Guest1 guest1;
        private Guest1Service guest1Service;
        private SentAccommodationReservationRequests sentAccommodationReservationRequests;
        private Guest1Profile guest1Profile;
        
        public Guest1Home(User user)
        {
            InitializeComponent();

            guest1Service = new Guest1Service();
            GetGuest1ByUser(user);

            guest1SearchAccommodation = new Guest1SearchAccommodation(guest1, ref Main);
            myReservations = new MyAccommodationReservations(guest1, ref Main);
            sentAccommodationReservationRequests = new SentAccommodationReservationRequests();
            guest1Profile = new Guest1Profile(guest1);
            Main.Content = guest1SearchAccommodation;

        }

        private void GetGuest1ByUser(User user)
        {
            this.guest1 = new Guest1();
            this.guest1 = guest1Service.GetByUsername(user.Username);
            
        }

        private void BookingMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = guest1SearchAccommodation;   //da li da se pravi novi page (kako bi se resetovalo sve?)
        }
        private void MyReservationsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            myReservations = new MyAccommodationReservations(guest1, ref Main); //moram ga opet napraviti da bi se azuriralo nakon pravljenja nove rezervacije (u upcoming reservations)
            Main.Content = myReservations;   //da li da se pravi novi page (kako bi se resetovalo sve?)
            
        }//negdje proslijedjujem ref Frame?

        private void SignOutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.Owner.Show();
        }

        private void SentRequestsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = sentAccommodationReservationRequests;
        }

        private void MyProfileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = guest1Profile;
        }
    }
}
