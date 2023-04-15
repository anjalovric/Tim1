using System.Windows.Controls;
using InitialProject.Model;
using InitialProject.WPF.ViewModels;
using InitialProject.WPF.ViewModels.OwnerViewModels;

namespace InitialProject.WPF.Views.OwnerViews
{
    /// <summary>
    /// Interaction logic for GuestReviewView.xaml
    /// </summary>
    public partial class GuestReviewView : Page
    {
        public GuestReviewView(Owner owner)
        {
            InitializeComponent();
            DataContext = new GuestReviewViewModel(owner);
        }
    }
}
