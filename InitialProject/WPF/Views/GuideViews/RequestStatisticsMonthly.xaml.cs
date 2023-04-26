using InitialProject.WPF.ViewModels.GuideViewModels;
using System.Windows.Controls;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for RequestStatisticsMonthly.xaml
    /// </summary>
    public partial class RequestStatisticsMonthly : Page
    {
        public RequestStatisticsMonthly(string Language,GuideOneYearRequestStatisticViewModel selectedYear)
        {
            InitializeComponent();
            DataContext=new MonthlyRequestStatisticsViewModel(Language, selectedYear);
        }
    }
}
