using InitialProject.Model;
using InitialProject.WPF.ViewModels.GuideViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for ComplexRequestView.xaml
    /// </summary>
    public partial class ComplexRequestView : Page
    {
        public ComplexRequestView(User loggedUser, ObservableCollection<TourInstance> todaysInstances, ObservableCollection<TourInstance> futureInstances)
        {
            InitializeComponent();
            DataContext = new ComplexRequestViewModel(loggedUser, todaysInstances, futureInstances);
        }
    }
}
