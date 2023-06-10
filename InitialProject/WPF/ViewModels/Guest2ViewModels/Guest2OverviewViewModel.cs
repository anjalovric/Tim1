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
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class Guest2OverviewViewModel:INotifyPropertyChanged
    {
        public Model.Guest2 WindowGuest2 { get; set; }
        private bool buttonIsChecked;
        public bool ButtonIsChecked
        {
            get
            {
                buttonIsChecked = Properties.Settings.Default.IsToolTipEnabled;
                return buttonIsChecked;
            }
            set
            {
                Properties.Settings.Default.IsToolTipEnabled = value;
                Properties.Settings.Default.Save();
                buttonIsChecked = value;
                OnPropertyChanged("ButtonIsChecked");
            }
        }
        public Guest2 guest2 { get; set; }
        private Guest2Repository guest2Repository;
        public RelayCommand ShowCommand { get; set; }
        public RelayCommand VouchersCommand { get; set; }
        public RelayCommand ActiveToursCommand { get; set; }
        public RelayCommand SignOutCommand { get; set; }
        public RelayCommand ProfileCommand { get; set; }
        public RelayCommand FinishedToursCommand { get; set; }
        public RelayCommand RequestsCommand { get; set; }
        public RelayCommand NotificationsCommand { get; set; }
        private ContentControl ContentControl;
        private BitmapImage imageSource;
        public BitmapImage ImageSource
        {
            get { return imageSource; }
            set
            {
                if (value != imageSource)
                    imageSource = value;
                OnPropertyChanged("ImageSource");
            }

        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Guest2OverviewViewModel(User user,ContentControl contentControl  )
        {
            guest2Repository = new Guest2Repository();
            guest2 = new Model.Guest2();
           
            GetGuest2ByUser(user);
            string relative = FindImageRelativePath();
            ImageSource = new BitmapImage(new Uri(relative, UriKind.Relative));
            ContentControl = contentControl;
            MakeCommands();
            ContentControl.Content = new ShowToursView(guest2);
        }
        private string FindImageRelativePath()
        {
            return guest2.ImagePath;
        }
        private void MakeCommands()
        {
            ShowCommand = new RelayCommand(Show_Executed, CanExecute);
            VouchersCommand = new RelayCommand(Vouchers_Executed, CanExecute);
            ActiveToursCommand = new RelayCommand(ActiveTours_Executed, CanExecute);
            SignOutCommand = new RelayCommand(SignOut_Executed, CanExecute);
            FinishedToursCommand = new RelayCommand(ShowFinished_Executed, CanExecute);
            ProfileCommand = new RelayCommand(Profile_Executed, CanExecute);
            RequestsCommand = new RelayCommand(Requests_Executed, CanExecute);
            NotificationsCommand = new RelayCommand(Notifications_Executed, CanExecute);
            
        }

        private bool CanExecute(object sender)
        {
            return true;
        }
        
        private void Show_Executed(object sender)
        {
            ContentControl.Content=new ShowToursView(guest2);
        }
        private void Notifications_Executed(object sender)
        {
            ContentControl.Content = new NotificationsFormView(guest2);
        }
        private void Requests_Executed(object sender)
        {
            ContentControl.Content = new MyRequestsFormView(guest2);
        }
        private void Profile_Executed(object sender)
        {
            ContentControl.Content = new Guest2ProfileView(guest2);
        }
        private void Vouchers_Executed(object sender)
        {
            ContentControl.Content= new VoucherFormView(guest2);
        }
        private void ActiveTours_Executed(object sender)
        {
            ContentControl.Content = new ActiveToursFormView(guest2);
        }
        private void SignOut_Executed(object sender)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Application.Current.Windows.OfType<Guest2Overview>().FirstOrDefault().Close();
        }
        private void ShowFinished_Executed(object sender)
        {
            ContentControl.Content = new FinishedTourInstancesFormView(guest2);
        }
        private void GetGuest2ByUser(User user)
        {
            guest2 = guest2Repository.GetByUsername(user.Username);
        }
      
       
    }
}
