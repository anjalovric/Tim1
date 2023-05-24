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
using InitialProject.WPF.ViewModels.Guest1ViewModels;

namespace InitialProject.WPF.Views.Guest1Views
{
    /// <summary>
    /// Interaction logic for AnywhereAnytime.xaml
    /// </summary>
    public partial class AnywhereAnytimeView : Page
    {
        private AnywhereAnytimeViewModel anywhereAnytimeViewModel;
        public AnywhereAnytimeView(Guest1 guest1)
        {
            InitializeComponent();
            anywhereAnytimeViewModel = new AnywhereAnytimeViewModel(guest1);
            this.DataContext = anywhereAnytimeViewModel;
        }
    }
}
