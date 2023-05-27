using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class ForumsViewModel
    {
        private Owner profileOwner;
        private ForumService forumService;
        public ObservableCollection<OneForumViewModel> Forums { get; set; }
        public ObservableCollection<OneForumViewModel> NewForums { get; set; }
        public RelayCommand ViewCommand { get; set; }


        public ForumsViewModel(Owner owner)
        {
            profileOwner = owner;
            MakeForums();
            ViewCommand = new RelayCommand(View_Executed, CanExecute);
        }

        private void MakeForums()
        {
            forumService = new ForumService();
            Forums = new ObservableCollection<OneForumViewModel>(forumService.getAllForOwnerDisplay(profileOwner));
            NewForums = new ObservableCollection<OneForumViewModel>(forumService.getNewForOwnerDisplay(profileOwner));
            forumService.MakeForumsOld(forumService.getNewForOwnerDisplay(profileOwner));
        }

        private bool CanExecute(object sender)
        {
            return true;
        }

        private void View_Executed(object sender)
        {
            
        }
    }
}
