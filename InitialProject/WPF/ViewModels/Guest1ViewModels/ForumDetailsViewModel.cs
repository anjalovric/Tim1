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

        private bool isVeryUseful;
        public bool IsVeryUseful
        {
            get { return isVeryUseful; }
            set
            {
                if (value != isVeryUseful)
                    isVeryUseful = value;
                OnPropertyChanged("IsVeryUseful");
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
        private bool isCommentingEnabled;
        public bool IsCommentingEnabled
        {
            get { return isCommentingEnabled; }
            set
            {
                if (value != isCommentingEnabled)
                {
                    isCommentingEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        private Visibility isClosingVisible;
        public Visibility IsClosingVisible
        {
            get { return isClosingVisible; }
            set
            {
                if (value != isClosingVisible)
                {
                    isClosingVisible = value;
                    OnPropertyChanged();
                }
            }
        }
        private Visibility isOpeningVisible;
        public Visibility IsOpeningVisible
        {
            get { return isOpeningVisible; }
            set
            {
                if (value != isOpeningVisible)
                {
                    isOpeningVisible = value;
                    OnPropertyChanged();
                }
            }
        }


        public string NewComment
        {
            get { return newComment; }
            set
            {
                if (value != newComment)
                {
                    newComment = value;
                    OnPropertyChanged("NewComment");
                }
            }
        }
        private string newComment;
        public RelayCommand CloseForumCommand { get; set; }
        public RelayCommand OpenForumCommand { get; set; }
        public RelayCommand BackCommand { get; set; } 
        public RelayCommand AddCommentCommand { get; set; }
        private ForumCommentService forumCommentService;
        private ForumService forumService;
        public ForumDetailsViewModel(Guest1 guest1, Forum forum)
        {
            this.guest1 = guest1;
            this.Forum = forum;
            Initialize();
            MakeCommands();
        }
        private void Initialize()
        {
            NewComment = "";
            IsVeryUseful = Forum.IsVeryUseful;
            IsCommentingEnabled = Forum.Opened;
            if (guest1.Id != Forum.Guest1.Id)
                IsClosingEnabled = false;
            else if (Forum.Opened == true)
                IsClosingEnabled = true;
            SetButtonsVisibility();
            forumCommentService = new ForumCommentService();
            forumService = new ForumService();
            Comments = new ObservableCollection<ForumComment>(forumCommentService.GetAllByForumId(Forum.Id));
            Comments = new ObservableCollection<ForumComment>(Comments.Reverse());
        }
        private void MakeCommands()
        {
            CloseForumCommand = new RelayCommand(CloseForum_Executed, CanExecute);
            BackCommand = new RelayCommand(Back_Executed, CanExecute);
            AddCommentCommand = new RelayCommand(AddComment_Executed, CanExecute);
            OpenForumCommand = new RelayCommand(OpenForum_Executed, CanExecute);
        }

        //other methods
        private void SetButtonsVisibility()
        {
            if (Forum.Opened == true)
            {
                IsClosingVisible = Visibility.Visible;
                IsOpeningVisible = Visibility.Hidden;

            }
            else
            {
                IsClosingVisible = Visibility.Hidden;
                IsOpeningVisible = Visibility.Visible;
            }
        }
        private void UpdateComments()
        {
            Comments = new ObservableCollection<ForumComment>(forumCommentService.GetAllByForumId(Forum.Id));
            Comments = new ObservableCollection<ForumComment>(Comments.Reverse());
            NewComment = "";
        }
        private void ShowMessageBoxForSuccessfullClosing()
        {
            Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("Forum successfully closed!", "/Resources/Images/lock.png");
            messageBox.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
            messageBox.ShowDialog();
        }
        private void ShowMessageBoxForSuccessfullOpening()
        {
            Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("Forum successfully opened!", "/Resources/Images/padlock-unlock.png");
            messageBox.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
            messageBox.ShowDialog();
        }

        //execute commands
        private void OpenForum_Executed(object sender)
        {
            Forum=forumService.Open(forum, guest1);       
            IsClosingVisible = Visibility.Visible;
            IsOpeningVisible = Visibility.Hidden;
            IsCommentingEnabled = Forum.Opened;
            if (guest1.Id != Forum.Guest1.Id)
                IsClosingEnabled = false;
            else
                IsClosingEnabled=true;
            ShowMessageBoxForSuccessfullOpening();
        }
        private void AddComment_Executed(object sender)
        {
            if (NewComment != "")
            {
                ForumComment newComment = new ForumComment(Forum, guest1, DateTime.Now, NewComment);
                forumCommentService.Add(newComment);
                forumService.IncrementCommentsNumber(Forum);
                Forum = forumService.SetIsVeryUseful(Forum);
                IsVeryUseful = Forum.IsVeryUseful;
                UpdateComments();
            }
            else
                ShowMessageBoxForInvalidCommenting();

        }

        private void Back_Executed(object sender)
        {
            ForumView forumView = new ForumView(guest1);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = forumView;
        } 
        private void CloseForum_Executed(object sender)
        {
            if(guest1.Id == Forum.Guest1.Id)    //ako je to gost koji je i napravio forum
            {
                forumService.Close(forum);
                ForumView forumView = new ForumView(guest1);
                Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = forumView;
                ShowMessageBoxForSuccessfullClosing();
            }
        }

        //Message box - no comment entered
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
