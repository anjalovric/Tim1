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
using InitialProject.View.Owner;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for OwnerOverview.xaml
    /// </summary>
    public partial class OwnerOverview : Window
    {
        public OwnerOverview()
        {
            InitializeComponent();
        }

        private void AddAccommodationClick(object sender, RoutedEventArgs e)
        {
            AccommodationForm accommodationForm = new AccommodationForm();
            accommodationForm.Show();
        }
    }
}
