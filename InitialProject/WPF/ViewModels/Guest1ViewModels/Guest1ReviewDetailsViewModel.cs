using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Service;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class Guest1ReviewDetailsViewModel
    {
        private Guest1 guest1;
        public GuestReview SelectedReview {get;set;}
        public Guest1ReviewDetailsViewModel(Guest1 guest1, GuestReview selectedReview)
        {
            this.guest1 = guest1;
            SelectedReview = selectedReview;
        }
    }
}
