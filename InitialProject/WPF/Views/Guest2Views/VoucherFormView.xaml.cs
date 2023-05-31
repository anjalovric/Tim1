using InitialProject.Model;
using InitialProject.WPF.ViewModels.Guest2ViewModels;
using InitialProject.Service;
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
using InitialProject.Help;

namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for VoucherViewForm.xaml
    /// </summary>
    public partial class VoucherFormView : UserControl
    {
        private Model.Guest2 guest2; 
        public VoucherFormView(Model.Guest2 guest)
        {
            InitializeComponent();
            this.guest2 = guest;
            DataContext = new VoucherViewModel(guest2);
        }
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                string str = ShowToursHelp.GetHelpKey((DependencyObject)focusedControl);
                ShowToursHelp.ShowHelpForVouchers(str, this);
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);
        }
    }
}
