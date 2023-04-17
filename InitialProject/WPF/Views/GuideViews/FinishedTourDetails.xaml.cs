using InitialProject.Model;
using InitialProject.WPF.ViewModels;
using System.Windows.Controls;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for FinishedTourDetails.xaml
    /// </summary>
    public partial class FinishedTourDetails : Page
    {
       
        public FinishedTourDetails(TourInstance selected)
        {
            InitializeComponent();
            DataContext = new FinishedTourDetailsViewModel(selected); ;

        }
    
    }
}
