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
using DotLiquid.Tags;
using System.Windows.Threading;
using InitialProject.Model;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using NPOI.SS.Formula.Functions;

namespace InitialProject.WPF.Views.OwnerViews
{
    /// <summary>
    /// Interaction logic for ForumsView.xaml
    /// </summary>
    public partial class ForumsView : Page
    {
        public ForumsViewModel ViewModel { get; set; }
        public ForumsView(Owner owner)
        {
            InitializeComponent();
            ViewModel = new ForumsViewModel(owner);
            this.DataContext = ViewModel;
        }
        
    }
}
