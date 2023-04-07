using InitialProject.Model;
using InitialProject.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for ReviewDetailsView.xaml
    /// </summary>
    public partial class ReviewDetailsView : Page
    {
        ReviewDetailsViewModel viewModel;
        public ReviewDetailsView(GuideAndTourReview review)
        {
            InitializeComponent();
            viewModel = new ReviewDetailsViewModel(review);
            DataContext = viewModel;
        }
    }
}
