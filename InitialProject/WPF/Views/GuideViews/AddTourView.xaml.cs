using InitialProject.Model;
using InitialProject.WPF.ViewModels.GuideViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;
namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for AddTourView.xaml
    /// </summary>
    public partial class AddTourView : Page
    {
        public AddTourViewModel viewModel;
        public AddTourView(ObservableCollection<TourInstance> todayInstances,User user, ObservableCollection<TourInstance> futureInstances)
        {
            viewModel=new AddTourViewModel(todayInstances,user,futureInstances);
            InitializeComponent();
            DataContext = viewModel;
        }

    }
}
