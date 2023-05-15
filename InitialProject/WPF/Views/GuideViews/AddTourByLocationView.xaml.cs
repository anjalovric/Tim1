using InitialProject.Model;
using InitialProject.WPF.ViewModels.GuideViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for AddTourByLocation.xaml
    /// </summary>
    public partial class AddTourByLocationView : Page
    {
        public AddTourByLocationView(ObservableCollection<TourInstance> todayInstances,User loggedUser,ObservableCollection<TourInstance>futureInstances,Location location)
        {
            InitializeComponent();
            DataContext=new AddTourByLocationViewModel(todayInstances,loggedUser,futureInstances,location);
        }


    }
}
