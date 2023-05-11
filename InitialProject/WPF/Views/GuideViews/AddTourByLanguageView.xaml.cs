using InitialProject.Model;
using InitialProject.WPF.ViewModels.GuideViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for AddTourByLanguageView.xaml
    /// </summary>
    public partial class AddTourByLanguageView : Page
    {
        public AddTourByLanguageViewModel viewModel;
        public AddTourByLanguageView(ObservableCollection<TourInstance> todayInstances, User user, ObservableCollection<TourInstance> futureInstances, string selectedLanguage)
        {
            viewModel = new AddTourByLanguageViewModel(todayInstances, user, futureInstances, selectedLanguage);
            InitializeComponent();
            DataContext = viewModel;
        }


    }
}
