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
using System.Windows.Navigation;
using System.Windows.Shapes;
using InitialProject.Model;
using InitialProject.WPF.ViewModels;

namespace InitialProject.WPF.Views.OwnerViews
{
    /// <summary>
    /// Interaction logic for GuestReviewFormView.xaml
    /// </summary>
    public partial class GuestReviewFormView : Page
    {
        public GuestReviewFormView(AccommodationReservation reservation)
        {
            InitializeComponent();
            DataContext = new GuestReviewFormViewModel(reservation);
        }
    }
}
