using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using InitialProject.View.Owner;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for OwnerOverview.xaml
    /// </summary>
    public partial class OwnerOverview : Window
    {
        public ObservableCollection<Accommodation> accommodations { get; set; }
        public OwnerOverview()
        {
            InitializeComponent();
            DataContext = this;
            AccommodationRepository accommodationRepository = new AccommodationRepository();
            accommodations = new ObservableCollection<Accommodation>(accommodationRepository.GetAll());
        }

        private void AddAccommodationClick(object sender, RoutedEventArgs e)
        {
            AccommodationForm accommodationForm = new AccommodationForm(accommodations);
            accommodationForm.Show();
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            this.Close();
        }

        private void Review_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            this.Close();
        }
    }
}
