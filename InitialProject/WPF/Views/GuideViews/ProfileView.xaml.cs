using InitialProject.Model;
using InitialProject.WPF.ViewModels.GuideViewModels;
using System.Windows.Controls;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for ProfileView.xaml
    /// </summary>
    public partial class ProfileView : Page
    {
        public ProfileView(User loggedUser)
        {
            InitializeComponent();
            DataContext = new ProfileViewModel(loggedUser);
        }
    }
}
