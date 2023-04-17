using InitialProject.Model;
using InitialProject.WPF.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for ReviewDetailsView.xaml
    /// </summary>
    public partial class ReviewDetailsView : Page
    {
        ReviewDetailsViewModel viewModel;
        public ReviewDetailsView(GuideAndTourReview review,ObservableCollection<GuideAndTourReview> Reviews,User user)
        {
            InitializeComponent();
            viewModel = new ReviewDetailsViewModel(review,Reviews,user);
            DataContext = viewModel;
        }

    }
}
