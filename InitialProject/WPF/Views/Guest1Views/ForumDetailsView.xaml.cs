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
using System.Windows.Navigation;
using System.Windows.Shapes;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.WPF.ViewModels.Guest1ViewModels;

namespace InitialProject.WPF.Views.Guest1Views
{
    /// <summary>
    /// Interaction logic for ForumDetailsView.xaml
    /// </summary>
    public partial class ForumDetailsView : Page
    {
        private ForumDetailsViewModel forumDetailsViewModel;
        public ForumDetailsView(Guest1 guest1, Forum forum)
        {
            InitializeComponent();
            forumDetailsViewModel = new ForumDetailsViewModel(guest1, forum);
            this.DataContext = forumDetailsViewModel;
        }
    }
}
