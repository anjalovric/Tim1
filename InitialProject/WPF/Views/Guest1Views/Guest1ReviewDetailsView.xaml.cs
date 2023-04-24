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
using InitialProject.WPF.ViewModels.Guest1ViewModels;

namespace InitialProject.WPF.Views.Guest1Views
{
    /// <summary>
    /// Interaction logic for Guest1ReviewDetailsView.xaml
    /// </summary>
    public partial class Guest1ReviewDetailsView : Page
    {
        private Guest1ReviewDetailsViewModel guest1ReviewDetailsViewModel;
        public Guest1ReviewDetailsView(Guest1 guest1, GuestReview selectedReview)
        {
            InitializeComponent();
            guest1ReviewDetailsViewModel = new Guest1ReviewDetailsViewModel(guest1, selectedReview);
            DataContext = guest1ReviewDetailsViewModel;
        }
    }
}
