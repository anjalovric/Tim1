using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using InitialProject.Model;
using InitialProject.WPF.ViewModels;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for OwnerMainWindowView.xaml
    /// </summary>
    public partial class OwnerMainWindowView : Window
    {
        private User user;
        public OwnerMainWindowView(User user)
        {
            InitializeComponent();
            DataContext = new OwnerMainWindowViewModel(user);
            this.user = user;
        }
        private void BurgerMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuView menu = new MenuView(user);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = menu;
        }
    }
}
