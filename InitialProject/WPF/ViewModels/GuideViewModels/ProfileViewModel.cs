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
        public ObservableCollection<SuperGuide> supreGuideTitles { get; set; }
        public ProfileViewModel(User user)
        {
            guideService = new GuideService();
            loggedGuide = guideService.GetByUsername(user.Username);

            superGuideService = new SuperGuideService();
            SetUserData();
            supreGuideTitles = new ObservableCollection<SuperGuide>(superGuideService.UpdateSuperGuideStatus(loggedGuide));
        }

        private void SetUserData()
        {
            Name = loggedGuide.Name;
            Surname = loggedGuide.LastName;
            Username = loggedGuide.Username;
            Age=loggedGuide.Age;
            Email= loggedGuide.Email;
        }
    }
}
