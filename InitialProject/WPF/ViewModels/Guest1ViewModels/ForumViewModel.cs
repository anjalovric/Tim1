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

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class ForumViewModel : INotifyPropertyChanged
    {
        private Guest1 guest1;
        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> CitiesByCountry { get; set; }
        public RelayCommand CountryInputSelectionChangedCommand { get; set; }
        public RelayCommand NextCommand { get; set; }
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
            if (LocationCity != null) { }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
