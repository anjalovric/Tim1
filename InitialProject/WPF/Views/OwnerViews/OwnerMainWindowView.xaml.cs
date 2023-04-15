using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.ViewModels;
using InitialProject.WPF.ViewModels.OwnerViewModels;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for OwnerMainWindowView.xaml
    /// </summary>
    public partial class OwnerMainWindowView : Window
    {
        private Owner owner;
        private OwnerMainWindowViewModel ownerViewModel;
        public OwnerMainWindowView(User user)
        {
            InitializeComponent();
            ownerViewModel = new OwnerMainWindowViewModel(user);
            DataContext = ownerViewModel;
            OwnerService ownerService = new OwnerService();
            this.owner = ownerService.GetByUsername(user.Username);
            FrameForPages.Content = new OwnerOverviewView(owner);
        }
    }
}
