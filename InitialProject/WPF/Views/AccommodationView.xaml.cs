using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using InitialProject.Model;
using InitialProject.WPF.ViewModels;
using SixLabors.ImageSharp.Metadata.Profiles.Xmp;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for AccommodationView.xaml
    /// </summary>
    public partial class AccommodationView : Page
    {
        public AccommodationView(User user)
        {
            InitializeComponent();
            DataContext = new AccommodationViewModel(user);
        }

        private void NewAccommodationButton_Click(object sender, RoutedEventArgs e)
        {
            AccommodationInputFormView accommodationInputFormView = new AccommodationInputFormView();
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = accommodationInputFormView;

        }
    }
}
