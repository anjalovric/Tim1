using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using InitialProject.APPLICATION.UseCases;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.WPF.Views.Guest1Views;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class ForumDetailsViewModel : INotifyPropertyChanged
    {
        private Guest1 guest1;
        private ObservableCollection<ForumComment> comments;
        public ObservableCollection<ForumComment> Comments
        {
            get { return comments; }
            set
            {
                if (value != comments)
                    comments = value;
                OnPropertyChanged("Comments");
            }

        }
        private Forum forum;
        public Forum Forum
        {
            get { return forum; }
            set
            {
                if (value != forum)
                {
                    forum = value;
                    OnPropertyChanged("Forum");
                }
            }
        }

        private ForumCommentService forumCommentService;
        public ForumDetailsViewModel(Guest1 guest1, Forum newForum)
        {
            this.guest1 = guest1;
            this.Forum = newForum;
            forumCommentService = new ForumCommentService();
            Comments = new ObservableCollection<ForumComment>(forumCommentService.GetAllByForumId(Forum.Id));

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
