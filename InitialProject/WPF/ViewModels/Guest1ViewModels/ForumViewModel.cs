using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Service;
using InitialProject.WPF.Views.Guest1Views;
using System.Windows;
using InitialProject.Domain.Model;
using InitialProject.APPLICATION.UseCases;
using System.Windows.Controls;
using System.Reflection;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class ForumViewModel : INotifyPropertyChanged
    {
        private Guest1 guest1;
        private OwnerNotificationsService ownerNotificationsService;
        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> CitiesByCountry { get; set; }
        public RelayCommand CountryInputSelectionChangedCommand { get; set; }
        public RelayCommand NextCommand { get; set; }
        public RelayCommand ResetCommand { get; set; }
        public RelayCommand ViewCommand { get; set; }
        private bool isCityComboBoxEnabled;
        public bool IsCityComboBoxEnabled
        {
            get { return isCityComboBoxEnabled; }
            set
            {
                if (value != isCityComboBoxEnabled)
                {
                    isCityComboBoxEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        private string locationCountry;
        public string LocationCountry
        {
            get { return locationCountry; }
            set
            {
                if (value != locationCountry)
                {
                    locationCountry = value;
                    OnPropertyChanged("LocationCountry");
                }
            }
        }
        private string firstComment;
        public string FirstComment
        {
            get { return firstComment; }
            set
            {
                if (value != firstComment)
                {
                    firstComment = value;
                    OnPropertyChanged("FirstComment");
                }
            }
        }
        private string locationCity;
        public string LocationCity
        {
            get { return locationCity; }
            set
            {
                if (value != locationCity)
                {
                    locationCity = value;
                    OnPropertyChanged("LocationCity");
                }
            }
        }


        private ObservableCollection<Forum> forums;
        public ObservableCollection<Forum> Forums
        {
            get { return forums; }
            set
            {
                if (value != forums)
                    forums = value;
                OnPropertyChanged("Forums");
            }

        }

        private ForumService forumService;
        private LocationService locationService;
        private ForumCommentService forumCommentService;
        public ForumViewModel(Guest1 guest1)
        {
            this.guest1 = guest1;
            GetLocations();
            Initialize();
            MakeCommands();
        }

        private void Initialize()
        {
            forumService = new ForumService();
            forumCommentService = new ForumCommentService();
            Forums = new ObservableCollection<Forum>(forumService.GetAll());
            Forums = new ObservableCollection<Forum>(Forums.Reverse());
            ownerNotificationsService = new OwnerNotificationsService();
        }
        private void MakeCommands()
        {
            CountryInputSelectionChangedCommand = new RelayCommand(CountryInputSelectionChanged_Executed, CanExecute);
            NextCommand = new RelayCommand(Next_Executed, CanExecute);
            ResetCommand = new RelayCommand(Reset_Executed, CanExecute);
            ViewCommand = new RelayCommand(View_Executed, CanExecute);  
        }
        private void View_Executed(object sender)
        {
            Forum currentForum = ((Button)sender).DataContext as Forum;
            ForumDetailsView details = new ForumDetailsView(guest1, currentForum);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = details;
        }
        private void CountryInputSelectionChanged_Executed(object sender)
        {
            LocationService locationService = new LocationService();
            if (LocationCountry != null)
            {
                CitiesByCountry.Clear();
                foreach (string city in locationService.GetCitiesByCountry(LocationCountry))
                {
                    CitiesByCountry.Add(city);
                }
                IsCityComboBoxEnabled = true;
            }
        }
        private void Next_Executed(object sender)
        {
            if (IsInputValid())
            {
                if(forumService.ExistsOnLocation(LocationCountry, LocationCity))
                {
                    if (forumService.GetByLocation(LocationCountry, LocationCity).Opened)
                        ShowMessageBoxForOpenedForum();

                    else
                        ShowMessageBoxForClosedForum();
                }

                else
                    CreateForum();
            }
            else
                ShowMessageBoxForInvalidInput();
        }
        private void Reset_Executed(object sender)
        {
            ResetAllFields();
        }

        //other methods
        private void ResetAllFields()
        {
            LocationCity = null;
            LocationCountry = null;
            FirstComment = "";
            IsCityComboBoxEnabled = false;
        }

        private async void ShowMessageBoxForClosedForum()
        {
            Task<bool> result = ConfirmCommentingLockedForumMessageBox();
            bool IsYesClicked = await result;
            if (IsYesClicked)
            {
                Forum currentForum = forumService.GetByLocation(LocationCountry, LocationCity);
                currentForum = forumService.Open(currentForum, guest1);
                ForumComment newComment = new ForumComment(currentForum, guest1, DateTime.Now, FirstComment);
                forumCommentService.Add(newComment);
                forumService.IncrementCommentsNumber(currentForum);
                currentForum = forumService.SetIsVeryUseful(currentForum);
                ownerNotificationsService.AddNewForumNotification(currentForum.Location);
                ForumDetailsView details = new ForumDetailsView(guest1, currentForum);
                Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = details;
            }   
        }

        private async void ShowMessageBoxForOpenedForum()
        {
            Task<bool> result = ConfirmCommentingOpenedForumMessageBox();
            bool IsYesClicked = await result;
            if (IsYesClicked)
            {
                Forum currentForum = forumService.GetByLocation(LocationCountry, LocationCity);
                ForumComment newComment = new ForumComment(currentForum, guest1, DateTime.Now, FirstComment);
                forumCommentService.Add(newComment);
                forumService.IncrementCommentsNumber(currentForum);
                currentForum = forumService.SetIsVeryUseful(currentForum);
                ForumDetailsView details = new ForumDetailsView(guest1, currentForum);
                Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = details;
            }
        }
        public async Task<bool> ConfirmCommentingLockedForumMessageBox()
        {
            var result = new TaskCompletionSource<bool>();
            Guest1YesNoMessageBoxView messageBox = new Guest1YesNoMessageBoxView("Forum - " + LocationCity + " is locked. Click YES to unlock it and put your comment on it.", "/Resources/Images/info.png", result);
            messageBox.Owner = Application.Current.Windows.OfType<DatesForAccommodationReservationView>().FirstOrDefault();
            messageBox.ShowDialog();
            var returnedResult = await result.Task;
            return returnedResult;
        }

        public async Task<bool> ConfirmCommentingOpenedForumMessageBox()
        {
            var result = new TaskCompletionSource<bool>();
            Guest1YesNoMessageBoxView messageBox = new Guest1YesNoMessageBoxView("Forum - " + LocationCity + " already exists. Click YES to put your comment on it.", "/Resources/Images/info.png", result);
            messageBox.Owner = Application.Current.Windows.OfType<DatesForAccommodationReservationView>().FirstOrDefault();
            messageBox.ShowDialog();
            var returnedResult = await result.Task;
            return returnedResult;
        }

        private void CreateForum()
        {
            Location newLocation = locationService.GetByCityAndCountry(LocationCountry, LocationCity);
            Forum newForum = new Forum(newLocation, guest1);
            forumService.Add(newForum);
            CreateFirstComment(newForum);
            ForumDetailsView view = new ForumDetailsView(guest1, newForum);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = view;
        }
        private void CreateFirstComment(Forum newForum)
        {
            ForumComment comment = new ForumComment(newForum, guest1, DateTime.Now, FirstComment);
            forumCommentService.Add(comment);
        }
        private void GetLocations()
        {
            IsCityComboBoxEnabled = false;
            locationService = new LocationService();
            Countries = new ObservableCollection<string>(locationService.GetAllCountries());
            CitiesByCountry = new ObservableCollection<string>();
        }
        //validation for input
        private bool IsInputValid()
        {
            return LocationCity != null && LocationCountry != null && FirstComment != null && FirstComment != "";
        }
        //Message box - not all fields filled
        private void ShowMessageBoxForInvalidInput()
        {
            Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("You must fill all fields!", "/Resources/Images/exclamation.png");
            messageBox.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
            messageBox.ShowDialog();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
    }
}
