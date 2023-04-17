using InitialProject.Model;
using InitialProject.WPF.ViewModels;
using System.Windows.Controls;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for TourInstanceReviewView.xaml
    /// </summary>
    public partial class TourInstanceReviewView : Page
    {
        private TourInstanceReviewViewModel viewModel;
        public TourInstanceReviewView(User guide)  
        {
            InitializeComponent();
            viewModel = new TourInstanceReviewViewModel(guide);
            DataContext = viewModel;


        }
    }
}
