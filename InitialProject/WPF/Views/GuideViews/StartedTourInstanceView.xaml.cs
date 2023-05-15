using InitialProject.Model;
using InitialProject.WPF.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for StartedTourInstanceView.xaml
    /// </summary>
    public partial class StartedTourInstanceView : Page 
    {
        StartedTourInstanceViewModel viewModel;
        public StartedTourInstanceView(TourInstance selectedInstance, ObservableCollection<TourInstance> tours, ObservableCollection<TourInstance> finishedInstances,int GuideId)
        {
            InitializeComponent();
            viewModel=new StartedTourInstanceViewModel(selectedInstance, tours, finishedInstances, GuideId);
            DataContext = viewModel;

        }


    }
}
