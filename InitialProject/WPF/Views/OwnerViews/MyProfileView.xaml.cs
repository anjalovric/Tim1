using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.ViewModels;
using InitialProject.WPF.ViewModels.OwnerViewModels;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for MyProfileView.xaml
    /// </summary>
    public partial class MyProfileView : Page
    {
        private MyProfileViewModel myProfileViewModel;
        public MyProfileView(Owner owner)
        {
            InitializeComponent();
            myProfileViewModel = new MyProfileViewModel(owner);
            DataContext = myProfileViewModel;
        }
    }
}
