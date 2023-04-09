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
        private Owner owner;
        public MenuView(Owner owner)
        {
            InitializeComponent();
            this.owner = owner;
        }

        private void MyProfileButton_Click(object sender, RoutedEventArgs e)
        {
            MyProfileView myProfile = new MyProfileView(owner);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = myProfile;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = NavigationService.GoBack;
        }

        private void MyAccommodationButton_Click(object sender, RoutedEventArgs e)
        {
            AccommodationView accommodationView = new AccommodationView(owner);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = accommodationView;
        }

        private void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().Close();
        }
    }
}
