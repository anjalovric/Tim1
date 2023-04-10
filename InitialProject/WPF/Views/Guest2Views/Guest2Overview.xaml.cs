using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for Guest2Overview.xaml
    /// </summary>
    public partial class Guest2Overview : Window
    {
        public Model.Guest2 WindowGuest2 { get; set; }
        private Guest2 guest2;
        private Guest2Repository guest2Repository;
        public Guest2Overview(User user)
        {
            InitializeComponent();
            guest2Repository = new Guest2Repository();
            guest2 = new Model.Guest2();
            GetGuest2ByUser(user);
            CC.Content = new ShowTours(guest2);
        }
        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new ShowTours(guest2);

        }
        private void ActiveTours_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new ActiveToursForm(guest2);

        }
        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            this.Close();

        }
        private void ShowFinished_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new FinishedTourInstances(guest2);

        }
        private void GetGuest2ByUser(User user)
        {
            guest2 = guest2Repository.GetByUsername(user.Username);
        }
        private void Vouchers_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new VoucherViewForm(guest2);

        }
       
    }
}
