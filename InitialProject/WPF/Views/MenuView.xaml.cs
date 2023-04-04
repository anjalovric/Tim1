using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using InitialProject.Model;
using InitialProject.WPF.Views;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : Page
    {
        private User user;
        public MenuView(User user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void MyProfileButton_Click(object sender, RoutedEventArgs e)
        {
            MyProfileView myProfile = new MyProfileView(user);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = myProfile;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = NavigationService.GoBack;
        }

        private void MyAccommodationButton_Click(object sender, RoutedEventArgs e)
        {
            AccommodationView accommodationView = new AccommodationView(user);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = accommodationView;
        }
    }
}
