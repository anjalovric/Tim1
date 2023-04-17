using InitialProject.Model;
using InitialProject.WPF.ViewModels;
using System.Windows.Controls;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for CancelView.xaml
    /// </summary>
    public partial class CancelView : Page
    {
        public CancelViewModel cancelViewModel;
        public CancelView(User guide)
        {
            InitializeComponent();
            cancelViewModel = new CancelViewModel(guide,this.TourListDataGrid);
            DataContext = cancelViewModel;       
        }
    }
}
