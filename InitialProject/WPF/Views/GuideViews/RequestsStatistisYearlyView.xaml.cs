using InitialProject.Model;
using InitialProject.WPF.ViewModels.GuideViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for RequestsStatistisYearly.xaml
    /// </summary>
    public partial class RequestsStatisticsYearlyView : Page
    {
        public RequestStatisticYearlyViewModel viewModel;
        public RequestsStatisticsYearlyView(ObservableCollection<TourInstance> todayInstances,User loggedUser,ObservableCollection<TourInstance>futureInstances)
        {
            InitializeComponent();
            viewModel = new RequestStatisticYearlyViewModel(todayInstances,loggedUser,futureInstances );
            DataContext = viewModel;
        }
    }
}
