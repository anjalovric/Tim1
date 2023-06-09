using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest1Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
using InitialProject.WPF.ViewModels.Guest2ViewModels;
using InitialProject.Help;

namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for ShowTours.xaml
    /// </summary>
    public partial class ShowToursView : UserControl
    {
        private ShowToursViewModel viewModel;

        public ShowToursView(Guest2 guest2)
        {
            InitializeComponent();
            viewModel=new ShowToursViewModel(guest2,this);
            DataContext = viewModel;
        }
        

        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);
        }

    }

}
