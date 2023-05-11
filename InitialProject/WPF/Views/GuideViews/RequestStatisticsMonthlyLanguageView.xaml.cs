using InitialProject.WPF.ViewModels.GuideViewModels;
using System.Windows.Controls;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for RequestStatisticsMonthly.xaml
    /// </summary>
    public partial class RequestStatisticsMonthlyLanguageView : Page
    {
        public RequestStatisticsMonthlyLanguageView(string Language,GuideOneYearRequestStatisticViewModel selectedYear)
        {
            InitializeComponent();
            DataContext=new RequestStatisticMonthlyLanguageViewModel(Language, selectedYear);
        }
    }
}
