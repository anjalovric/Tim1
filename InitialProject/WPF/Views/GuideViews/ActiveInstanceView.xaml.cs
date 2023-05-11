using InitialProject.Model;
using InitialProject.WPF.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for ActiveInstanceView.xaml
    /// </summary>
    public partial class ActiveInstanceView : Page
    {
        ActiveInstanceViewModel viewModel;
        public ActiveInstanceView(TourInstance active,ObservableCollection<TourInstance> finishedInstances,User user, ObservableCollection<TourInstance> tours,ObservableCollection <TourInstance> cancelable)
        {
            InitializeComponent();
            viewModel = new ActiveInstanceViewModel(active,finishedInstances,user,tours,cancelable);
            DataContext = viewModel;
        }
    }
}
