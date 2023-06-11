using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class ForumCommentsViewModel : INotifyPropertyChanged
    {
        private OneForumViewModel forum;
        public ObservableCollection<ForumComment> Comments { get; set; }
        private ForumComment selectedComment;
        public RelayCommand ReportCommand { get; set;}
        public RelayCommand NewCommentCommand { get; set; }
        public RelayCommand OKCommand { get; set; }
        public Owner Owner { get; set; }
        private ForumCommentService forumCommentService;
        private OwnerNotificationsService notificationsService;
        private string stackPanelVisibility;
        private string stackPanelMessage;
        private bool isOkPressedInDemo;
        private bool isAddNewCommentPressedInDemo;
        public ForumCommentsViewModel(OneForumViewModel forum, Owner owner)
        {
            notificationsService = new OwnerNotificationsService();
            Forum = forum;
            Owner = owner;
            SelectedComment = new ForumComment();
            MakeComments();
            ReportCommand = new RelayCommand(Report_Executed, ReportCanExecute);
            NewCommentCommand = new RelayCommand(NewComment_Executed, CanExecute);
            OKCommand = new RelayCommand(OK_Executed, CanExecute);
            DisplayNotificationPanel();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string StackPanelMessage
        {
            get { return stackPanelMessage; }
            set
            {
                if (value != stackPanelMessage)
                {
                    stackPanelMessage = value;
                    OnPropertyChanged();
                }
            }
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
        public string StackPanelVisibility
        {
            get { return stackPanelVisibility; }
            set
            {
                if (value != stackPanelVisibility)
                {
                    stackPanelVisibility = value;
                    OnPropertyChanged();
                }
            }
        }
        public OneForumViewModel Forum
        {
            get { return forum; }
            set
            {
                if (value != forum)
                {
                    forum = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsOkPressedInDemo
        {
            get { return isOkPressedInDemo; }
            set
            {
                if (value != isOkPressedInDemo)
                {
                    isOkPressedInDemo = value;
                    OnPropertyChanged();
                }
            }
        }
        public ForumComment SelectedComment
        {
            get { return selectedComment; }
            set
            {
                if (value != selectedComment)
                {
                    selectedComment = value;
                    OnPropertyChanged();
                }
            }
        }

        private void InitializeSelectedComment()
        {
            if (Comments.Count() > 0)
                SelectedComment = Comments[0];
        }

        private void MakeComments()
        {
            forumCommentService = new ForumCommentService();
            Comments = new ObservableCollection<ForumComment>(forumCommentService.GetAllByForumIdForOwner(Owner, Forum.Forum.Id));
            InitializeSelectedComment();
        }

        private bool CanExecute(object sender)
        {
            return true;
        }

        private bool ReportCanExecute(object sender)
        {
            return Forum.OwnerHasLocation;
        }
        private void Report_Executed(object sender)
        {
            if(SelectedComment != null && SelectedComment.Id!=0)
            {
                ReportingCommentView reportingCommentView = new ReportingCommentView(SelectedComment, Owner, Forum);
                reportingCommentView.Show();
                UpdateComments();
            }
            DisplayNotificationPanel();
        }

        private void OK_Executed(object sender)
        {
            StackPanelVisibility = "Hidden";
        }

        private void NewComment_Executed(object sender)
        {
            AddCommentView addCommentView = new AddCommentView(Forum, Owner);
            ForumService forumService = new ForumService();
            forumService.IncrementCommentsNumber(Forum.Forum);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = addCommentView;
        }

        private void UpdateComments()
        {
            Comments.Clear();
            foreach (ForumComment comment in forumCommentService.GetAllByForumIdForOwner(Owner,Forum.Forum.Id))
            {
                Comments.Add(comment);
            }
            InitializeSelectedComment();
        }

        private void DisplayNotificationPanel()
        {
            if (notificationsService.IsCommentAdded(Owner))
            {
                StackPanelMessage = "New comment successfully added!";
                StackPanelVisibility = "Visible";
                notificationsService.Delete(Domain.Model.OwnerNotificationType.COMMENT_ADDED, Owner);
            }
            else if (notificationsService.IsCommentReported(Owner))
            {
                StackPanelMessage = "Comment successfully reported!";
                StackPanelVisibility = "Visible";
                notificationsService.Delete(Domain.Model.OwnerNotificationType.COMMENT_REPORTED, Owner);
            }
            else
            {
                StackPanelVisibility = "Hidden";
            }
        }
    }
}
