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
using InitialProject.Service;
using InitialProject.WPF.Views.Guest1Views;
using System.Windows;

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
        private bool isClosingEnabled;
        public bool IsClosingEnabled
        {
            get { return isClosingEnabled; }
            set
            {
                if (value != isClosingEnabled)
                {
                    isClosingEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        public string NewComment { get; set; }
        public RelayCommand CloseForumCommand { get; set; }
        public RelayCommand BackCommand { get; set; } 
        public RelayCommand AddCommentCommand { get; set; }
        private ForumCommentService forumCommentService;
        private ForumService forumService;
        public ForumDetailsViewModel(Guest1 guest1, Forum forum)
        {
            this.guest1 = guest1;
            this.Forum = forum;
            if(guest1.Id!=forum.Guest1.Id)
                IsClosingEnabled = false;
            else
                IsClosingEnabled = true;
            forumCommentService = new ForumCommentService();
            NewComment = "";

            MakeCommands();
            forumService = new ForumService();
            Comments = new ObservableCollection<ForumComment>(forumCommentService.GetAllByForumId(Forum.Id));

        }
        private void AddComment_Executed(object sender)
        {
            if (NewComment != "")
            {
                ForumComment newComment = new ForumComment(Forum, guest1, DateTime.Now, NewComment);
                forumCommentService.Add(newComment);
                forumService.IncrementCommentsNumber(Forum);
                Comments.Add(newComment);

            }
            else
                ShowMessageBoxForInvalidCommenting();

        }

        private void Back_Executed(object sender)
        {
            ForumView forumView = new ForumView(guest1);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = forumView;
        }


        private void MakeCommands()
        {
            CloseForumCommand = new RelayCommand(CloseForum_Executed, CanExecute);
            BackCommand = new RelayCommand(Back_Executed, CanExecute);  
            AddCommentCommand = new RelayCommand(AddComment_Executed, CanExecute);  
        }
        private void CloseForum_Executed(object sender)
        {
            if(guest1.Id == Forum.Guest1.Id)    //ako je to gost koji je i napravio forum
            {
                forumService.Close(forum);
                ForumView forumView = new ForumView(guest1);
                Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = forumView;
                ShowMessageBoxForSuccess();
            }
        }
        
        private void ShowMessageBoxForSuccess()
        {
            Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("Forum successfully closed!", "/Resources/Images/done.png");
            messageBox.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
            messageBox.ShowDialog();
        }

        private void ShowMessageBoxForInvalidCommenting()
        {
            Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("You must enter a comment text!", "/Resources/Images/exclamation.png");
            messageBox.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
            messageBox.ShowDialog();
        }
        private bool CanExecute(object sender)
        {
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
