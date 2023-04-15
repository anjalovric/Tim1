using InitialProject.Model;
using InitialProject.WPF.ViewModels;
using System.Windows;

namespace InitialProject.WPF.Views.GuideViews
{
    /// <summary>
    /// Interaction logic for GuideWindow.xaml
    /// </summary>
    public partial class GuideWindow : Window
    {
        public GuideWindow(User user)
        {
            InitializeComponent();
            DataContext = new GuideWindowViewModel(user);
        }

    }
}
