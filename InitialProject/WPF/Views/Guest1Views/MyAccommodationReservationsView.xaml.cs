using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.WPF.ViewModels;
using InitialProject.WPF.ViewModels.Guest1ViewModels;

namespace InitialProject.WPF.Views.Guest1Views
{
    /// <summary>
    /// Interaction logic for MyAccommodationReservations.xaml
    /// </summary>
    public partial class MyAccommodationReservationsView : Page
    {
        private MyAccommodationReservationsViewModel myAccommodationReservationsViewModel;

        public MyAccommodationReservationsView(Guest1 guest1)
        {
            InitializeComponent();
            myAccommodationReservationsViewModel = new MyAccommodationReservationsViewModel(guest1);
            DataContext = myAccommodationReservationsViewModel;

        }
    }
}