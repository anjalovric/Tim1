using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using System.Windows;
using InitialProject.WPF.Views;
using InitialProject.Service;

namespace InitialProject.WPF.ViewModels
{
    public class OwnerMainWindowViewModel
    {
        private Owner owner;
        public OwnerMainWindowViewModel(User user)
        {
            OwnerService ownerService = new OwnerService();
            owner = ownerService.GetByUsername(user.Username);
        }
       
        public void MakeMenu()
        {
            MenuView menu = new MenuView(owner);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = menu;
        }

    }
}
