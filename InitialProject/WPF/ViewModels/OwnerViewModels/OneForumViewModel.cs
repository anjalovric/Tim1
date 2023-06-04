using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Domain.Model;
using InitialProject.Model;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class OneForumViewModel
    {
        public Forum Forum {get; set;}

        public bool OwnerHasLocation { get; set; }
        public int GuestComments { get; set; }
        public int OwnerComments { get; set; }

        public bool IsVeryUseful { get; set; }
         
        public OneForumViewModel()
        {
            Forum = new Forum();
        }

    }
}
