using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ReportingCommentViewModel
    {
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public ForumComment ForumComment { get; set; }
        private OneForumViewModel forum;
        public Owner Owner { get; set; }
        public ReportingCommentViewModel(ForumComment forumComment, Owner owner, OneForumViewModel forum)
        {
            ForumComment = forumComment;
            Owner = owner;
            this.forum = forum;
            CancelCommand = new RelayCommand(Cancel_Executed, CanExecute);
            DeleteCommand = new RelayCommand(Report_Executed, CanExecute);
        }

        private bool CanExecute(object sender)
        {
            return true;
        }

        private void Cancel_Executed(object sender)
        {
            Application.Current.Windows.OfType<ReportingCommentView>().FirstOrDefault().Close();
        }

        private void Report_Executed(object sender)
        {
            ForumCommentService forumCommentService = new ForumCommentService();
            forumCommentService.Report(ForumComment, Owner);

            Application.Current.Windows.OfType<ReportingCommentView>().FirstOrDefault().Close();
            ForumCommentsView forumCommentsView = new ForumCommentsView(forum, Owner);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = forumCommentsView;
        }
    }
}
