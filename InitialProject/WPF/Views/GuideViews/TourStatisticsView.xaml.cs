using InitialProject.Model;
using InitialProject.WPF.ViewModels;
using System.Windows.Controls;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for TourStatisticsView.xaml
    /// </summary>
    public partial class TourStatisticsView : Page
    {
       public TourStatisticsViewModel viewModel;

        public TourStatisticsView(User user)
        {

            InitializeComponent();
            viewModel = new TourStatisticsViewModel(user,this.ChosenYear);
            DataContext = viewModel;

        }
    }
}
