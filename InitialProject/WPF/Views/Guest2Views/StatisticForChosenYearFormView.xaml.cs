using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
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
using System.Windows.Shapes;
using InitialProject.WPF.ViewModels.Guest2ViewModels;

namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for StatisticForChoosenYear.xaml
    /// </summary>
    public partial class StatisticForChosenYearFormView : Window
    {
        public StatisticForChosenYearFormView(Model.Guest2 guest2,string year)
        {
            InitializeComponent();
            DataContext = new StatisticForChosenYearViewModel(guest2,year,this);
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);
        }
    }
}
