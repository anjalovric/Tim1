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
    /// Interaction logic for Guest1MessageBoxView.xaml
    /// </summary>
    public partial class Guest1YesNoMessageBoxView : Window
    {
        private Guest1YesNoMessageBoxViewModel guest1MessageBoxViewModel;
        public Guest1YesNoMessageBoxView(string messageText, string path, TaskCompletionSource<bool> result)
        {
            InitializeComponent();
            guest1MessageBoxViewModel = new Guest1YesNoMessageBoxViewModel(messageText, path, result);
            DataContext = guest1MessageBoxViewModel;    
        }
    }
}
