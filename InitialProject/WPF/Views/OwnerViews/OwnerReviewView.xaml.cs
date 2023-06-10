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
using InitialProject.Model;
using InitialProject.WPF.ViewModels;
using InitialProject.WPF.ViewModels.OwnerViewModels;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for OwnerReviewView.xaml
    /// </summary>
    public partial class OwnerReviewView : Page
    {
        public OwnerReviewViewModel OwnerReviewViewModel { get; set; }
        public OwnerReviewView(OwnerReview ownerReview)
        {
            InitializeComponent();
            OwnerReviewViewModel = new OwnerReviewViewModel(ownerReview);
            DataContext = OwnerReviewViewModel;
        }

        public void PressButtons()
        {
            if(OwnerReviewViewModel.IsCancelPressedInDemo)
            {
                ExitButton.Background = new SolidColorBrush(Colors.LightBlue);
            }
            if(OwnerReviewViewModel.IsNextPicturePressedInDemo)
            {
                PreviousImageButton.Background = new SolidColorBrush(Colors.LightBlue);
            }
            else
            {
                PreviousImageButton.Background = new SolidColorBrush(Colors.Transparent);
            }
        }
    }
}
