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
using InitialProject.WPF.ViewModels.Guest1ViewModels;

namespace InitialProject.WPF.Views.Guest1Views
{
    /// <summary>
    /// Interaction logic for Guest1OkMessageBoxView.xaml
    /// </summary>
    public partial class Guest1OkMessageBoxView : Window
    {
        private Guest1OkMessageBoxViewModel guest1OkMessageBoxViewModel;
        public Guest1OkMessageBoxView(string messageText, string path)
        {
            InitializeComponent();
            guest1OkMessageBoxViewModel = new Guest1OkMessageBoxViewModel(messageText, path);
            DataContext = guest1OkMessageBoxViewModel;
        }
    }
}
