using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.Guest1Views;
using InitialProject.WPF.Views.OwnerViews;

namespace InitialProject.WPF.Demo
{
    public class ForumReportCommentDemo : INotifyPropertyChanged
    {
        private int increment = -1;
        private ForumsViewModel viewModel;
        private ForumsView view;

        private ForumCommentsView commentsView;
        private ForumCommentsViewModel commentsViewModel;
        private DemoIsOffView demoIsOffView;
        private DemoIsOnView demoIsOnView;
        public ForumReportCommentDemo()
        {
            OwnerService ownerService = new OwnerService();
            Owner owner = ownerService.GetAll()[0];
            view = new ForumsView(owner);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = view;
            viewModel = view.ViewModel;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void PlayDemo()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += new EventHandler(Timer_Tick);
            dispatcherTimer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            Increment++;
            if(Increment ==1)
            {
                demoIsOnView = new DemoIsOnView();
                demoIsOnView.Show();
            }
            if(Increment == 3)
            {
                demoIsOnView.Close();
                viewModel.SelectedForum = viewModel.Forums[0];
                view.AllForumsListBox.SelectedItem = viewModel.SelectedForum;
            }
            if(Increment == 4)
            {
                commentsView = new ForumCommentsView(viewModel.SelectedForum, viewModel.profileOwner);
                Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = commentsView;
                commentsViewModel = commentsView.ViewModel;
            }
            if(Increment == 5)
            {
                commentsViewModel.SelectedComment = commentsViewModel.Comments[2];
                commentsView.CommentsListBox.SelectedItem = commentsViewModel.SelectedComment;
            }
            if(Increment == 6)
            {
                ReportingCommentView reportingCommentView = new ReportingCommentView(commentsViewModel.SelectedComment, commentsViewModel.Owner, commentsViewModel.Forum);
                reportingCommentView.Show();
                reportingCommentView.PressYesInDemo();
            }
            if(Increment == 11)
            {
                commentsViewModel.SelectedComment.ReportsNumber++;
            }
            if(Increment == 12)
            {
                commentsViewModel.StackPanelMessage = "Comment successfully reported!";
                commentsViewModel.StackPanelVisibility = "Visible";
            }
            if(Increment == 13)
            {
                commentsViewModel.IsOkPressedInDemo = true;
            }
            if(Increment == 15)
            {
                commentsViewModel.IsOkPressedInDemo = false;
                commentsViewModel.StackPanelVisibility = "Hidden";
            }
            if(Increment == 17)
            {
                Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = view;
                commentsViewModel.SelectedComment.ReportsNumber--;
            }
            if(Increment == 18)
            {
                demoIsOffView = new DemoIsOffView();
                demoIsOffView.Show();
            }
            if(Increment == 20)
            {
                demoIsOffView.Close();
            }
        }

        public int Increment
        {
            get { return increment; }
            set
            {
                if (value != increment)
                {
                    increment = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
