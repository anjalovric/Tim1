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
using DotLiquid.Tags;
using System.Windows.Threading;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.WPF.ViewModels.OwnerViewModels;

namespace InitialProject.WPF.Views.OwnerViews
{
    /// <summary>
    /// Interaction logic for ReportingCommentView.xaml
    /// </summary>
    public partial class ReportingCommentView : Window
    {
        public ReportingCommentViewModel ViewModel { get; set; }
        private int increment = 0;
        public ReportingCommentView(ForumComment forumComent, Owner owner, OneForumViewModel forum)
        {
            InitializeComponent();
            this.Owner = Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault();
            ViewModel = new ReportingCommentViewModel(forumComent, owner, forum);
            DataContext = ViewModel;
        }

        public void PressYesInDemo()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += new EventHandler(Timer_Tick);
            dispatcherTimer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            increment++;
            if (increment == 1)
            {
                YesButton.Background = new SolidColorBrush(Colors.AliceBlue);
            }
            if (increment == 2)
            {
                YesButton.Background = new SolidColorBrush(Colors.Transparent);
                this.Close();
            }
        }
    }
}
