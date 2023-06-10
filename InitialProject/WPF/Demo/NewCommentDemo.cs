using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DotLiquid.Tags;
using System.Windows.Threading;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.Guest1Views;
using InitialProject.WPF.Views.OwnerViews;
using InitialProject.Domain.Model;

namespace InitialProject.WPF.Demo
{
    public class NewCommentDemo : INotifyPropertyChanged
    {
        private int increment = -1;
        private ForumCommentsView commentsView;
        private AddCommentView view;
        private AddCommentViewModel viewModel;
        private ForumComment comment;
        private DemoIsOffView demoIsOffView;
        private DemoIsOnView demoIsOnView;
        public NewCommentDemo()
        {
            commentsView = (ForumCommentsView?)Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content;
            view = new AddCommentView(commentsView.ViewModel.Forum, commentsView.ViewModel.Owner);
            viewModel = view.ViewModel;
            comment = new ForumComment();
            comment.User = commentsView.ViewModel.Owner;
            comment.IsOwnerComment = true;
            comment.Text = "Very nice location!";
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
            if(Increment == 1)
            {
                demoIsOnView = new DemoIsOnView();
                demoIsOnView.Show();
            }
            if(Increment == 3)
            {
                demoIsOnView.Close();
                commentsView.ViewModel.IsAddNewCommentPressedInDemo = true;
            }
            if(Increment == 4)
            {
                Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = view;
                commentsView.ViewModel.IsAddNewCommentPressedInDemo = false;
            }
            if(Increment == 5)
            {
                view.InputCommentInDemo();
            }
            if(Increment == 16)
            {
                viewModel.IsAddNewCommentPressedInDemo = true;
            }
            if(Increment == 17)
            {
                Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = commentsView;
                commentsView.ViewModel.StackPanelMessage = "New comment successfully added!";
                commentsView.ViewModel.StackPanelVisibility = "Visible";
                
                commentsView.ViewModel.Comments.Add(comment);
            }
            if(Increment == 19)
            {
                commentsView.ViewModel.IsOkPressedInDemo = true;
            }
            if(Increment ==20)
            {
                commentsView.ViewModel.StackPanelVisibility = "Hidden";
            }
            if(Increment == 22)
            {
                commentsView.ViewModel.Comments.Remove(comment);
                demoIsOffView = new DemoIsOffView();
                demoIsOffView.Show();
            }
            if(Increment == 24)
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
