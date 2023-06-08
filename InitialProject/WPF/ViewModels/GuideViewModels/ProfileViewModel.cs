using InitialProject.APPLICATION.UseCases;
using InitialProject.Domain.Model;
using InitialProject.Model;
using InitialProject.Service;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels.GuideViewModels
{
    public class ProfileViewModel:INotifyPropertyChanged
    {
        private string toastVisibility;
        public string ToastVisibility
        {
            get => toastVisibility;
            set
            {
                if (value != toastVisibility)
                {
                    toastVisibility = value;
                    OnPropertyChanged("ToastVisibility");
                }
            }
        }
        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        private string surname;
        public string Surname
        {
            get => surname;
            set
            {
                if (value != surname)
                {
                    surname = value;
                    OnPropertyChanged("Surname");
                }
            }
        }
        private int age;
        public int Age
        {
            get => age;
            set
            {
                if (value != age)
                {
                    age = value;
                    OnPropertyChanged("Age");
                }
            }
        }
        private string email;
        public string Email
        {
            get => email;
            set
            {
                if (value != email)
                {
                    email = value;
                    OnPropertyChanged("Email");
                }
            }
        }
        private string username;
        public string Username
        {
            get => username;
            set
            {
                if (value != username)
                {
                    username = value;
                    OnPropertyChanged("Username");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private Guide loggedGuide;
        private GuideService guideService;
        private SuperGuideService superGuideService;
        private VoucherService voucherService;
        private User loggedUser;
        public ObservableCollection<SuperGuide> supreGuideTitles { get; set; }
        public RelayCommand YesCommand { get; set; }
        public RelayCommand NoCommand { get; set; }
        public RelayCommand DismissCommand { get; set; }
        public ProfileViewModel(User user)
        {
            guideService = new GuideService();
            voucherService= new VoucherService();
            loggedGuide = guideService.GetByUsername(user.Username);
            loggedUser = user;
            superGuideService = new SuperGuideService();
            SetUserData();
            supreGuideTitles = new ObservableCollection<SuperGuide>(superGuideService.UpdateSuperGuideStatus(loggedGuide));
            ToastVisibility = "hidden";
            MakeCommands();
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private void MakeCommands()
        {
            YesCommand = new RelayCommand(Yes_Executed, CanExecute);
            NoCommand = new RelayCommand(No_Executed, CanExecute);
            DismissCommand = new RelayCommand(DismissExecuted, CanExecute);
        }

        private void SetUserData()
        {
            Name = loggedGuide.Name;
            Surname = loggedGuide.LastName;
            Username = loggedGuide.Username;
            Age=loggedGuide.Age;
            Email= loggedGuide.Email;
        }

        private void Dismissal()
        {
            loggedGuide.Active = false;
            guideService.Update(loggedGuide);

            voucherService.SendVoucher(loggedGuide.Id);
            voucherService.ChangeAssignedGuide(loggedGuide.Id);

            GuideWindowViewModel guideWindowViewModel=new GuideWindowViewModel(loggedUser);
            guideWindowViewModel.SignOut();

        }
        private void Yes_Executed(object sender)
        {
            Dismissal();
        }
        private void No_Executed(object sender)
        {
            ToastVisibility = "hidden";
        }

        private void DismissExecuted(object sender)
        {
            ToastVisibility = "visible";
        }
    }
}
