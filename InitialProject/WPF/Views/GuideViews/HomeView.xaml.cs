using InitialProject.Model;
using InitialProject.WPF.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : Page
    {
        public HomeViewModel viewModel;
        public HomeView(User user, ObservableCollection<TourInstance> Instances)
        {
            InitializeComponent();
            viewModel= new HomeViewModel(user, Instances);
            DataContext = viewModel;

        }

    }
}
