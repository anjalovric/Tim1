﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.OwnerViews;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class ForumsViewModel : INotifyPropertyChanged
    {
        private Owner profileOwner;
        private ForumService forumService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<OneForumViewModel> Forums { get; set; }
        public ObservableCollection<OneForumViewModel> NewForums { get; set; }
        public RelayCommand ViewCommand { get; set; }
        private OneForumViewModel selectedForum;
        public ForumsViewModel(Owner owner)
        {
            profileOwner = owner;
            MakeForums();
            selectedForum = new OneForumViewModel();
            ViewCommand = new RelayCommand(View_Executed, CanExecute);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void MakeForums()
        {
            forumService = new ForumService();
            Forums = new ObservableCollection<OneForumViewModel>(forumService.getAllForOwnerDisplay(profileOwner));
            NewForums = new ObservableCollection<OneForumViewModel>(forumService.getNewForOwnerDisplay(profileOwner));
            forumService.MakeForumsOld(forumService.getNewForOwnerDisplay(profileOwner));
        }

        public OneForumViewModel SelectedForum
        {
            get { return selectedForum; }
            set
            {
                if (value != selectedForum)
                {
                    selectedForum = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool CanExecute(object sender)
        {
            return SelectedForum != null;
        }

        private void View_Executed(object sender)
        {
            ForumCommentsView forumCommentsView = new ForumCommentsView(SelectedForum);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = forumCommentsView;
        }
    }
}