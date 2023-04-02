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
    /// Interaction logic for Guest1Home.xaml
    /// </summary>
    public partial class Guest1Home : Window
    {
        private Guest1SearchAccommodation guest1SearchAccommodation;
        private MyAccommodationReservations myReservations;
        private Guest1 guest1;
        private Guest1Repository guest1Repository;
        public Guest1Home(User user)
        {
            InitializeComponent();

            guest1Repository = new Guest1Repository();
            this.guest1 = new Guest1();
            GetGuest1ByUser(user);

            guest1SearchAccommodation = new Guest1SearchAccommodation(guest1);
            myReservations = new MyAccommodationReservations(guest1, ref Main);
            Main.Content = guest1SearchAccommodation;

        }

        private void GetGuest1ByUser(User user)
        {
            this.guest1 = guest1Repository.GetByUsername(user.Username);
        }

        private void BookingMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = guest1SearchAccommodation;   //da li da se pravi novi page (kako bi se resetovalo sve?)
        }
        private void MyReservationsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = myReservations;   //da li da se pravi novi page (kako bi se resetovalo sve?)
            
        }

        private void SignOutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.Owner.Show();
        }
    }
}
