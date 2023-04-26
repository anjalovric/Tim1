using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using System;
using System.Collections.Generic;
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
using InitialProject.WPF.ViewModels.Guest2ViewModels;

namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for Guest2ProfileView.xaml
    /// </summary>
    public partial class Guest2ProfileView : UserControl
    {
        public Guest2ProfileView(Model.Guest2 guest2)
        {
            InitializeComponent();
            DataContext = new Guest2ProfileViewModel(guest2);
        }
    }
}
