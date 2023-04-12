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
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.WPF.ViewModels.Guest1ViewModels;
using NPOI.SS.Formula.PTG;

namespace InitialProject.WPF.Views.Guest1Views
{
    /// <summary>
    /// Interaction logic for Guest1Profile.xaml
    /// </summary>
    public partial class Guest1ProfileView : Page
    {
        private Guest1ProfileViewModel guest1ProfileViewModel;
        public Guest1ProfileView(Guest1 guest1)
        {
            InitializeComponent();
            guest1ProfileViewModel = new Guest1ProfileViewModel(guest1); 
            this.DataContext = guest1ProfileViewModel;
        }
    }
}
