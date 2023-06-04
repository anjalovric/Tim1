using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.GuideViews;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace InitialProject.WPF.ViewModels
{
    public class GuideWindowViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<string> Languages { get; set; }
        private HomeView homeView;
        private AddTourView addTourView;
        private CancelView cancelView;
        private TourStatisticsView tourStatisticsView;
        private User loggedUser;
        private bool themeIsChecked;
        public bool ThemeIsChecked
        {
            get {
                themeIsChecked = Properties.Settings.Default.theme;
                return themeIsChecked;
            }
            set
            {
                Properties.Settings.Default.theme = value;
                Properties.Settings.Default.Save();
                themeIsChecked = value;
                OnPropertyChanged("ThemeIsChecked");
                ThemeChanged();
            }
        }
        private string selectedLanguage;
        public string SelectedLanguage
        {
            get {
                selectedLanguage = Properties.Settings.Default.language;
                return selectedLanguage; }
            set
            {
                Properties.Settings.Default.language = value;
                Properties.Settings.Default.Save();
                selectedLanguage = value;
                OnPropertyChanged("selectedLanguage");
                ComboBox_SelectionChanged();
            }
        }
        public RelayCommand HomeCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand ActivatedCommand { get; set; }
        public RelayCommand TourStatisticsCommand { get; set; }
        public RelayCommand ReviewCommand { get; set; }
        public RelayCommand SignOutCommand { get; set; }
        public RelayCommand OrdinaryRequestCommand { get; set; }
        public RelayCommand RequestYearlyStatisticsCommand { get; set; }
        public RelayCommand MyProfileCommand { get; set; }
        public RelayCommand ComplexRequestCommand { get; private set; }
        public GuideWindowViewModel(User user) 
        {
            tourStatisticsView = new TourStatisticsView(user);
            cancelView = new CancelView(user);
            homeView = new HomeView(user, tourStatisticsView.viewModel.Instances,cancelView.cancelViewModel.TourInstances);
            loggedUser = user;


            SwitchFirstPage(loggedUser);
            MakeCommands();
            AddLanguages();
            ThemeChanged();
            ComboBox_SelectionChanged();
        }
        private void AddLanguages()
        {
            Languages = new ObservableCollection<string>();
            Languages.Add("ENG");
            Languages.Add("SRB");
           // SelectedLanguage = "ENG";
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void MakeCommands()
        {
            HomeCommand = new RelayCommand(Home_Executed, CanExecute);
            CancelCommand = new RelayCommand(Cancel_Executed, CanExecute);
            AddCommand = new RelayCommand(Add_Executed, CanExecute);
            ActivatedCommand = new RelayCommand(Activated_Executed, CanExecute);
            TourStatisticsCommand= new RelayCommand(TourStatitcs_Executed, CanExecute);
            ReviewCommand=new RelayCommand(Review_Executed, CanExecute);
            SignOutCommand = new RelayCommand(SignOut_Executed, CanExecute);
            OrdinaryRequestCommand= new RelayCommand(OrdinaryRequest_Executed,CanExecute);
            RequestYearlyStatisticsCommand = new RelayCommand(RequestYearlyStatistic_Executed, CanExecute);
            MyProfileCommand=new RelayCommand(MyProfile_Executed, CanExecute);
            ComplexRequestCommand = new RelayCommand(ComplexRequestCommand_Executed, CanExecute);
        }

        private bool CanExecute(object sender)
        {
            return true;
        }
        private void SwitchFirstPage(User user)
        {
            TourInstanceService tourInstanceService = new TourInstanceService();
            GuideService guideService = new GuideService();
            Guide guide = guideService.GetByUsername(user.Username);
            if (tourInstanceService.GetByActive(guide) == null)
            {
                Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = homeView;
            }
            else
            {
                ActiveInstanceView activeInstanceView = new ActiveInstanceView(tourInstanceService.GetByActive(guide), tourStatisticsView.viewModel.Instances, loggedUser, tourStatisticsView.viewModel.Instances, cancelView.cancelViewModel.TourInstances);

                Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = activeInstanceView;
            }
        }
        private void Cancel_Executed(object sender)
        {
            cancelView = new CancelView(loggedUser);
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = cancelView;
        }
        private void Activated_Executed(object sender)
        {
            TourInstanceService tourInstanceService = new TourInstanceService();
            GuideService guideService = new GuideService();
            Guide guide = guideService.GetByUsername(loggedUser.Username);
            if (tourInstanceService.GetByActive(guide) != null)
            {
                ActiveInstanceView activeInstanceView = new ActiveInstanceView(tourInstanceService.GetByActive(guide), tourStatisticsView.viewModel.Instances,loggedUser, tourStatisticsView.viewModel.Instances, cancelView.cancelViewModel.TourInstances);

                Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = activeInstanceView;
            }
        }
        private void Add_Executed(object sender)
        {
            addTourView = new AddTourView(homeView.viewModel.Tours, loggedUser, cancelView.cancelViewModel.TourInstances);
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = addTourView;
        }

        private void Home_Executed(object sender)
        {
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = homeView;
        }
        private void TourStatitcs_Executed(object sender)
        {
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = tourStatisticsView;
        }

        private void Review_Executed(object sender)
        {
            TourInstanceReviewView tourInstanceReviewView = new TourInstanceReviewView(loggedUser);
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = tourInstanceReviewView;
        }
        private void RequestYearlyStatistic_Executed(object sender)
        {
            RequestsStatisticsYearlyView requestsStatistisYearly = new RequestsStatisticsYearlyView(homeView.viewModel.Tours,loggedUser, cancelView.cancelViewModel.TourInstances);
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = requestsStatistisYearly;
        }
        private void SignOut_Executed(object sender)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Close();
        }
        public void SignOut()
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Close();
        }
        private void OrdinaryRequest_Executed(object sender)
        {
           OrdinaryRequestOverviewView ordinaryRequestOverviewView = new OrdinaryRequestOverviewView(loggedUser, homeView.viewModel.Tours, cancelView.cancelViewModel.TourInstances);
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = ordinaryRequestOverviewView;
        }

        private void ComplexRequestCommand_Executed(object sender)
        {
            ComplexRequestView complexRequestOverviewView = new ComplexRequestView(loggedUser, homeView.viewModel.Tours, cancelView.cancelViewModel.TourInstances);
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = complexRequestOverviewView;
        }
        private void MyProfile_Executed(object sender)
        {
            ProfileView myProfileView = new ProfileView(loggedUser);
            Application.Current.Windows.OfType<GuideWindow>().FirstOrDefault().Main.Content = myProfileView;

        }
        private void ThemeChanged()
        {
            var app = (App)Application.Current;
            if (ThemeIsChecked.Equals(true))
                app.ChangeTheme(new Uri("Resources/DarkTheme.xaml", UriKind.Relative));
            else
                app.ChangeTheme(new Uri("Resources/LightTheme.xaml", UriKind.Relative));
        }
        private void ComboBox_SelectionChanged()
        {
            var app = (App)Application.Current;
            if (SelectedLanguage.Equals("SRB"))
            {
                app.ChangeLanguage("sr-LATN");
            }
            else
            {
                app.ChangeLanguage("en-US");
            }
        }
    }
}
