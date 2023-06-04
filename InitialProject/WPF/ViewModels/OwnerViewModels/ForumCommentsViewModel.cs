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
        public Owner Owner { get; set; }
        private ForumCommentService forumCommentService;
        public ForumCommentsViewModel(OneForumViewModel forum, Owner owner)
        {
            Forum = forum;
            Owner = owner;
            SelectedComment = new ForumComment();
            MakeComments();
            ReportCommand = new RelayCommand(Report_Executed, ReportCanExecute);
            NewCommentCommand = new RelayCommand(NewComment_Executed, CanExecute);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
            return SelectedComment != null && !SelectedComment.IsAlreadyReportedByThisOwner && Forum.OwnerHasLocation && !SelectedComment.WasOnLocation;
        }
        private void Report_Executed(object sender)
        {
            if(SelectedComment != null && SelectedComment.Id!=0)
            {
                forumCommentService.Report(SelectedComment, Owner);
                UpdateComments();
            }
        }

        private void NewComment_Executed(object sender)
        {
            AddCommentView addCommentView = new AddCommentView(Forum, Owner);
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
    }
}
