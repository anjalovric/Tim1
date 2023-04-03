using System.Collections.ObjectModel;
using System.Windows.Controls;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.ViewModels;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for MyProfileView.xaml
    /// </summary>
    public partial class MyProfileView : Page
    {
        public MyProfileView(User user)
        {
            InitializeComponent();
            DataContext = new MyProfileViewModel(user);
        }
        
    }
}
