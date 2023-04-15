using InitialProject.Model;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using System.Windows.Controls;

namespace InitialProject.WPF.Views.OwnerViews
{
    /// <summary>
    /// Interaction logic for DecliningRequestView.xaml
    /// </summary>
    public partial class DecliningRequestView : Page
    {
        public DecliningRequestViewModel decliningRequestViewModel;
        public DecliningRequestView(ReschedulingAccommodationRequest request)
        {
            InitializeComponent();
            decliningRequestViewModel = new DecliningRequestViewModel(request);
            DataContext = decliningRequestViewModel;
        }
    }
}
