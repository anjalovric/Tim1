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
using DotLiquid.Tags;
using System.Windows.Threading;
using InitialProject.Model;
using InitialProject.WPF.ViewModels.OwnerViewModels;

namespace InitialProject.WPF.Views.OwnerViews
{
    /// <summary>
    /// Interaction logic for AddCommentView.xaml
    /// </summary>
    public partial class AddCommentView : Page
    {
        public AddCommentViewModel ViewModel { get; set; }
        private string fullText = "";
        private int currentIndex = 0;
        private int increment;
        public AddCommentView(OneForumViewModel forum, Owner owner)
        {
            InitializeComponent();
            ViewModel = new AddCommentViewModel(forum, owner);
            this.DataContext = ViewModel;
        }

        public void InputCommentInDemo()
        {
            fullText = "Very nice location!";
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(500);
            dispatcherTimer.Tick += new EventHandler(Timer_Tick);
            dispatcherTimer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            increment++;
            if (currentIndex < fullText.Length)
            {
                currentIndex++;
                Comment.Dispatcher.Invoke(() =>
                {
                    Comment.Text = fullText.Substring(0, currentIndex);
                });
            }
        }
    }
}
