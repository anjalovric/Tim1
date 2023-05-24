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

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class ForumViewModel : INotifyPropertyChanged
    {
        private Guest1 guest1;
        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> CitiesByCountry { get; set; }
        public RelayCommand CountryInputSelectionChangedCommand { get; set; }
        public RelayCommand NextCommand { get; set; }
        public RelayCommand ResetCommand { get; set; }
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
        public ForumViewModel(Guest1 guest1)
        {
            this.guest1 = guest1;
            GetLocations();
            MakeCommands();
        }

        private void MakeCommands()
        {
            CountryInputSelectionChangedCommand = new RelayCommand(CountryInputSelectionChanged_Executed, CanExecute);
            NextCommand = new RelayCommand(Next_Executed, CanExecute);
            ResetCommand = new RelayCommand(Reset_Executed, CanExecute);    
        }
        private bool CanExecute(object sender)
        {
            return true;
        }
        private void GetLocations()
        {
            IsCityComboBoxEnabled = false;
            LocationService locationService = new LocationService();
            Countries = new ObservableCollection<string>(locationService.GetAllCountries());
            CitiesByCountry = new ObservableCollection<string>();
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
            if (LocationCity != null && LocationCountry != null && FirstComment != null && FirstComment != "")
            {
                //dodati provjeru dal vec postoji ta lokacija za forum !!!!!!!!!!!!!
                CreateForum();
            }
            else
                ShowMessageBoxForInvalidInput();
        }
        private void Reset_Executed(object sender)
        {
            //Accommodations.Clear();
            // foreach (Accommodation accommodation in accommodationService.GetAll())
            //Accommodations.Add(accommodation);

            ResetAllFields();
            //SortAccommodationBySuperOwners();
        }
        private void ResetAllFields()
        {
            LocationCity = null;
            LocationCountry = null;
            FirstComment = "";
            IsCityComboBoxEnabled = false;
        }

        private void ShowMessageBoxForInvalidInput()
        {
            Guest1OkMessageBoxView messageBox = new Guest1OkMessageBoxView("You must fill all fields!", "/Resources/Images/exclamation.png");
            messageBox.Owner = Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault();
            messageBox.ShowDialog();
        }

        private void CreateForum()
        {
            LocationService locationService = new LocationService();
            Location newLocation = locationService.GetByCityAndCountry(LocationCountry, LocationCity);
            Forum newForum = new Forum(newLocation, guest1);
            ForumService forumService = new ForumService();
            forumService.Add(newForum);
            CreateFirstComment(newForum);
            ForumDetailsView view = new ForumDetailsView(guest1, newForum);
            Application.Current.Windows.OfType<Guest1HomeView>().FirstOrDefault().Main.Content = view;
        }
        private void CreateFirstComment(Forum newForum)
        {
            ForumCommentService forumCommentService = new ForumCommentService();
            ForumComment comment = new ForumComment(newForum, guest1, DateTime.Now, FirstComment);
            forumCommentService.Add(comment);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
