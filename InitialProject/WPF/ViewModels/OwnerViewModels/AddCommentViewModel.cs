using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using InitialProject.APPLICATION.UseCases;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.OwnerViews;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class AddCommentViewModel : INotifyPropertyChanged
    {
        public OneForumViewModel Forum { get; set; }
        public Owner Owner { get; set; }
        public RelayCommand ConfirmCommand { get; set; }
        private string commentText;
        private OwnerNotificationsService notificationsService;
        private bool isAddNewCommentPressedInDemo;

        public event PropertyChangedEventHandler? PropertyChanged;

        public AddCommentViewModel(OneForumViewModel forum, Owner owner)
        {
            Forum = forum;
            Owner = owner;
            notificationsService = new OwnerNotificationsService();
            ConfirmCommand = new RelayCommand(Confirm_Executed, CanExecute);
        }

        public string CommentText
        {
            get { return commentText; }
            set
            {
                if (value != commentText)
                {
                    commentText = value;
                    OnPropertyChanged();
                }
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsAddNewCommentPressedInDemo
        {
            get { return isAddNewCommentPressedInDemo; }
            set
            {
                if (value != isAddNewCommentPressedInDemo)
                {
                    isAddNewCommentPressedInDemo = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool CanExecute(object sender)
        {
            return true;
        }

        private void Confirm_Executed(object sender)
        {
            SaveComment();
            Forum.OwnerComments++;
            ForumCommentsView forumCommentsView = new ForumCommentsView(Forum, Owner);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = forumCommentsView;
        }

        private void SaveComment()
        {
            ForumComment comment = new ForumComment();
            UserService userService = new UserService();
            comment.Text = CommentText;
            comment.Forum = Forum.Forum;
            comment.User = userService.GetByUsername(Owner.Username);
            comment.ReportsNumber = 0;
            comment.IsOwnerComment = true;
            comment.CreatingDate = DateTime.Now;
            ForumCommentService forumCommentService = new ForumCommentService();
            forumCommentService.Add(comment);
            notificationsService.Add(OwnerNotificationType.COMMENT_ADDED, Owner);
        }
    }
}
