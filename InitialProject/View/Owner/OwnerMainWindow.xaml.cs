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
using InitialProject.Model;

namespace InitialProject.View.Owner
{
    /// <summary>
    /// Interaction logic for OwnerMainWindow.xaml
    /// </summary>
    public partial class OwnerMainWindow : Window
    {
        private User user;
        public OwnerMainWindow(User user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void BurgerMenu_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu(user);
            FrameForPages.Content = menu;
        }
    }
}
