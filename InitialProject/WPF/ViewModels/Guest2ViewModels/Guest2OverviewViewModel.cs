using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class Guest2OverviewViewModel
    {
        public Model.Guest2 WindowGuest2 { get; set; }
        private Guest2 guest2;
        private Guest2Repository guest2Repository;
        public RelayCommand ShowCommand { get; set; }
        public RelayCommand VouchersCommand { get; set; }
        public RelayCommand ActiveToursCommand { get; set; }
        public RelayCommand SignOutCommand { get; set; }
        public RelayCommand FinishedToursCommand { get; set; }
        private ContentControl ContentControl;
        public Guest2OverviewViewModel(User user,ContentControl contentControl)
        {
            guest2Repository = new Guest2Repository();
            guest2 = new Model.Guest2();
            GetGuest2ByUser(user);
            ContentControl = contentControl;
            MakeCommands();
            ContentControl.Content = new ShowTours(guest2);
        }
        private void MakeCommands()
        {
            ShowCommand = new RelayCommand(Show_Executed, CanExecute);
            VouchersCommand = new RelayCommand(Vouchers_Executed, CanExecute);
            ActiveToursCommand = new RelayCommand(ActiveTours_Executed, CanExecute);
            SignOutCommand = new RelayCommand(SignOut_Executed, CanExecute);
            FinishedToursCommand = new RelayCommand(ShowFinished_Executed, CanExecute);
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private void Show_Executed(object sender)
        {
            ContentControl.Content=new ShowTours(guest2);
        }
        private void Vouchers_Executed(object sender)
        {
            ContentControl.Content= new VoucherViewForm(guest2);
        }
        private void ActiveTours_Executed(object sender)
        {
            ContentControl.Content = new ActiveToursForm(guest2);

        }
        private void SignOut_Executed(object sender)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Application.Current.Windows.OfType<Guest2Overview>().FirstOrDefault().Close();
        }
        private void ShowFinished_Executed(object sender)
        {
            ContentControl.Content = new FinishedTourInstances(guest2);

        }
        private void GetGuest2ByUser(User user)
        {
            guest2 = guest2Repository.GetByUsername(user.Username);
        }
    }
}
