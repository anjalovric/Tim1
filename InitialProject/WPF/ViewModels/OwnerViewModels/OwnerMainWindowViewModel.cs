using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using System.Windows;
using InitialProject.WPF.Views;
using InitialProject.Service;
using System.Windows.Navigation;
using System.Windows.Controls;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class OwnerMainWindowViewModel
    {
        private Owner owner;
        public RelayCommand BurgerMenuCommand { get; set; }
        public RelayCommand MyAccommodationCommand { get; set; }
        public RelayCommand HomePageCommand { get; set; }
        public RelayCommand MyProfileCommand { get; set; }
        public OwnerMainWindowViewModel(User user)
        {
            OwnerService ownerService = new OwnerService();
            owner = ownerService.GetByUsername(user.Username);
            MakeCommands();
        }

        private void MakeCommands()
        {
            BurgerMenuCommand = new RelayCommand(BurgerMenu_Executed, CanExecute);
            MyAccommodationCommand = new RelayCommand(MyAccommodation_Executed, CanExecute);
            HomePageCommand = new RelayCommand(HomePage_Executed, CanExecute);
            MyProfileCommand = new RelayCommand(MyProfile_Executed, CanExecute);
        }

        private bool CanExecute(object sender)
        {
            return true;
        }

        private void BurgerMenu_Executed(object sender)
        {
            MenuView menu = new MenuView(owner);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = menu;
        }

        private void MyAccommodation_Executed(object sender)
        {
            AccommodationView accommodationView = new AccommodationView(owner);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = accommodationView;
        }

        private void HomePage_Executed(object sender)
        {
            OwnerOverviewView ownerOverviewView = new OwnerOverviewView(owner);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = ownerOverviewView;
        }

        private void MyProfile_Executed(object sender)
        {
            MyProfileView myProfileView = new MyProfileView(owner);
            Application.Current.Windows.OfType<OwnerMainWindowView>().FirstOrDefault().FrameForPages.Content = myProfileView;
        }
    }
}
