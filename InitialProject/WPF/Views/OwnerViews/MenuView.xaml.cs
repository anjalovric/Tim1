using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using InitialProject.Model;
using InitialProject.WPF.ViewModels;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using InitialProject.WPF.Views;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : Page
    {
        private Owner owner;
        private MenuViewModel menuViewModel;
        public MenuView(Owner owner)
        {
            InitializeComponent();
            menuViewModel = new MenuViewModel(owner);
            DataContext = menuViewModel;
            this.owner = owner;
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = NavigationService.GoBack;
        }
      
    }
}
