using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using System.Windows;
using InitialProject.WPF.Views;

namespace InitialProject.WPF.ViewModels
{
    public class OwnerMainWindowViewModel
    {
        private User user;
        public OwnerMainWindowViewModel(User user)
        {
            this.user = user;
        }
       
    }
}
