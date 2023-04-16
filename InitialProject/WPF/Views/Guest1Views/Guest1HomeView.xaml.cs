using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.WPF.ViewModels.Guest1ViewModels;

namespace InitialProject.WPF.Views.Guest1Views
{
    /// <summary>
    /// Interaction logic for Guest1Home.xaml
    /// </summary>
    public partial class Guest1HomeView : Window
    {
        private Guest1HomeViewModel guest1HomeViewModel;
       
        public Guest1HomeView(User user)
        {
            InitializeComponent();
            guest1HomeViewModel = new Guest1HomeViewModel(user);
            DataContext=guest1HomeViewModel;
        }
    }
}