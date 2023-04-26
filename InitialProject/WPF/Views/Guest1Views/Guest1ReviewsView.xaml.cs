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
    /// Interaction logic for Guest1Reviews.xaml
    /// </summary>
    public partial class Guest1ReviewsView : Page
    {
        private Guest1ReviewsViewModel guest1ReviewsViewModel;
        public Guest1ReviewsView(Guest1 guest1)
        {
            InitializeComponent();
            guest1ReviewsViewModel = new Guest1ReviewsViewModel(guest1);
            this.DataContext = guest1ReviewsViewModel;
        }
    }
}
