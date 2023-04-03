using System.Collections.ObjectModel;
using System.Windows.Controls;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.ViewModels;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for MyProfile.xaml
    /// </summary>
    public partial class MyProfile : Page
    {
       
        public MyProfile(User user)
        {
            InitializeComponent();
            DataContext = new MyProfileViewModel(user);
            
        }
        
    }
}
