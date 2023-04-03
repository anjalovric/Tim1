using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using InitialProject.Model;
using InitialProject.WPF.Views;

namespace InitialProject.View.Owner
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        private User user;
        public Menu(User user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void MyProfileButton_Click(object sender, RoutedEventArgs e)
        {
            MyProfile myProfile = new MyProfile(user);
            Application.Current.Windows.OfType<OwnerMainWindow>().FirstOrDefault().FrameForPages.Content = myProfile;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Windows.OfType<OwnerMainWindow>().FirstOrDefault().FrameForPages.Content = NavigationService.GoBack;
        }
    }
}
