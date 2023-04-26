using InitialProject.WPF.ViewModels.GuideViewModels;
using System.Windows.Controls;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for RequestsStatistisYearly.xaml
    /// </summary>
    public partial class RequestsStatistisYearly : Page
    {
        public YearlyTourStatistics viewModel;
        public RequestsStatistisYearly()
        {
            InitializeComponent();
            viewModel = new YearlyTourStatistics();
            DataContext = viewModel;
        }
        private void ComboBoxCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.ComboBoxCountry_SelectionChanged();
        }
    }
}
