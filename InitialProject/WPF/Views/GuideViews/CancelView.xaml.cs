using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            cancelViewModel.CancelTour();
 
        }

 

    }
}
