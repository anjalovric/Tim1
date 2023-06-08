
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Service;
using InitialProject.View;
using InitialProject.View.Owner;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.GuideViews;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using InitialProject.WPF.Views.Guest1Views;

using InitialProject.WPF.Views.Guest2Views;

namespace InitialProject
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {

        private readonly UserRepository _repository;

        private string _username;
       

        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SignInForm()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new UserRepository();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _repository.GetByUsername(Username);
            if (user != null)
            {
                if(user.Password == txtPassword.Password)
                {
                    if(user.Role==Model.Role.OWNER)
                    {
                        OwnerMainWindowView ownerMainWindow = new OwnerMainWindowView(user);
                        ownerMainWindow.Show();
                        Close();
                    }else if(user.Role == Model.Role.GUIDE)
                    {
                        GuideService guideService = new GuideService();
                        Guide guide=guideService.GetByUsername(Username);
                        if (guide.Active)
                        {
                            GuideWindow guideMainWindow = new GuideWindow(user);
                            guideMainWindow.Show();
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("There isn't user with that username!");
                        }
                    }
                    else if (user.Role == Model.Role.GUEST1)
                    {
                        Guest1HomeView guest1Overview = new Guest1HomeView(user);
                        guest1Overview.Show();
                        Close();
                    }
                    else if (user.Role == Model.Role.GUEST2)
                    {
                        Guest2Overview guest2Overview = new Guest2Overview(user);
                        guest2Overview.Show();
                        Help.Wizard wizard = new Help.Wizard();
                        wizard.Show();
                    Close();
                } 

                } 
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }
            
        }
    }
}
