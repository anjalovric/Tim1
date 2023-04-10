using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.ViewModels;
using NPOI.SS.Formula.Functions;
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
